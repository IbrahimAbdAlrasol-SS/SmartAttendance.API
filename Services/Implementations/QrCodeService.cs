using Microsoft.EntityFrameworkCore;
using QRCoder;
using SmartAttendance.API.Data;
using SmartAttendance.API.Models.DTOs;
using SmartAttendance.API.Models.Entities;
using SmartAttendance.API.Services.Interfaces;

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SmartAttendance.API.Services.Implementations
{
    public class QrCodeService : IQrCodeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _encryptionKey;
        private readonly Dictionary<string, DateTime> _blockedIPs;
        
        public QrCodeService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _encryptionKey = _configuration["QrCodee:EncryptionKey"] ?? "SmartAttendance-QR-Key-2024-MustBe32Chars";
            _blockedIPs = new Dictionary<string, DateTime>();
        }
        
        public async Task<ApiResponse<QrCodeResponse>> GenerateQrCodeAsync(GenerateQrRequest request, string ipAddress, string userAgent)
        {
            try
            {
                // التحقق من صحة الجلسة
                var session = await _context.Sessions
                    .Include(s => s.CourseAssignment)
                    .FirstOrDefaultAsync(s => s.Id == request.SessionId);
                    
                if (session == null)
                {
                    return ApiResponse<QrCodeResponse>.ErrorResult("الجلسة غير موجودة", new List<string> { "404" });
                }
                
                // التحقق من حالة الجلسة
                if (session.Status != "Active" && session.Status != "Scheduled")
                {
                    return ApiResponse<QrCodeResponse>.ErrorResult("لا يمكن توليد QR Code لهذه الجلسة", new List<string> { "400" });
                }
                
                // إنهاء QR Codes السابقة
                await ExpireAllSessionQrCodesAsync(request.SessionId);
                
                // توليد البيانات الآمنة
                var salt = GenerateSalt();
                var qrData = new
                {
                    SessionId = request.SessionId,
                    Timestamp = DateTime.UtcNow.Ticks,
                    Salt = salt,
                    DeviceFingerprint = request.DeviceFingerprint
                };
                
                var jsonData = JsonSerializer.Serialize(qrData);
                var encryptedData = EncryptData(jsonData);
                var qrCode = GenerateSecureQrCode(encryptedData, salt);
                var securityHash = GenerateSecurityHash(qrCode, salt, ipAddress);
                
                // إنشاء QR Session
                var qrSession = new QrSession
                {
                    SessionId = request.SessionId,
                    QrCode = qrCode,
                    EncryptedData = encryptedData,
                    GeneratedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(request.ExpirationMinutes),
                    Status = "Active",
                    DeviceFingerprint = request.DeviceFingerprint,
                    IpAddress = ipAddress,
                    MaxUsage = request.MaxUsage,
                    SecurityHash = securityHash,
                    Salt = salt
                };
                
                _context.QrSessions.Add(qrSession);
                await _context.SaveChangesAsync();
                
                // توليد صورة QR Code
                var qrImageBase64 = GenerateQrImage(qrCode);
                
                var response = new QrCodeResponse
                {
                    QrCode = qrCode,
                    QrImageBase64 = qrImageBase64,
                    GeneratedAt = qrSession.GeneratedAt,
                    ExpiresAt = qrSession.ExpiresAt,
                    MaxUsage = qrSession.MaxUsage,
                    SecurityLevel = "High"
                };
                
                return ApiResponse<QrCodeResponse>.SuccessResult(response, "تم توليد QR Code بنجاح");
            }
            catch (Exception ex)
            {
                return ApiResponse<QrCodeResponse>.ErrorResult($"خطأ في توليد QR Code: {ex.Message}", new List<string> { "500" });
            }
        }
        
        public async Task<ApiResponse<QrValidationResponse>> ValidateQrCodeAsync(ValidateQrRequest request, string ipAddress, string userAgent)
        {
            try
            {
                // التحقق من الحظر
                if (IsIpBlocked(ipAddress))
                {
                    return ApiResponse<QrValidationResponse>.ErrorResult("IP محظور مؤقتاً", new List<string> { "429" });
                }
                
                // البحث عن QR Session
                var qrSession = await _context.QrSessions
                    .Include(q => q.Session)
                    .FirstOrDefaultAsync(q => q.QrCode == request.QrCode && q.Status == "Active");
                    
                if (qrSession == null)
                {
                    await LogUsageAttempt(null, request.StudentId, ipAddress, userAgent, "Failed", "QR Code غير صحيح");
                    return ApiResponse<QrValidationResponse>.ErrorResult("QR Code غير صحيح", new List<string> { "400" });
                }
                
                // التحقق من انتهاء الصلاحية
                if (qrSession.ExpiresAt <= DateTime.UtcNow)
                {
                    qrSession.Status = "Expired";
                    await _context.SaveChangesAsync();
                    await LogUsageAttempt(qrSession.Id, request.StudentId, ipAddress, userAgent, "Failed", "QR Code منتهي الصلاحية");
                    return ApiResponse<QrValidationResponse>.ErrorResult("QR Code منتهي الصلاحية", new List<string> { "400" });
                }
                
                // التحقق من عدد الاستخدامات
                if (qrSession.UsageCount >= qrSession.MaxUsage)
                {
                    qrSession.Status = "Used";
                    await _context.SaveChangesAsync();
                    await LogUsageAttempt(qrSession.Id, request.StudentId, ipAddress, userAgent, "Failed", "تم استنفاد عدد الاستخدامات");
                    return ApiResponse<QrValidationResponse>.ErrorResult("تم استنفاد عدد الاستخدامات", new List<string> { "400" });
                }
                
                // التحقق من الأمان
                var expectedHash = GenerateSecurityHash(qrSession.QrCode, qrSession.Salt, qrSession.IpAddress ?? "");
                if (qrSession.SecurityHash != expectedHash)
                {
                    await LogUsageAttempt(qrSession.Id, request.StudentId, ipAddress, userAgent, "Blocked", "فشل في التحقق الأمني");
                    await BlockSuspiciousActivityAsync(ipAddress, 30);
                    return ApiResponse<QrValidationResponse>.ErrorResult("فشل في التحقق الأمني", new List<string> { "403" });
                }
                
                // تحديث عداد الاستخدام
                qrSession.UsageCount++;
                if (qrSession.UsageCount >= qrSession.MaxUsage)
                {
                    qrSession.Status = "Used";
                }
                
                await _context.SaveChangesAsync();
                await LogUsageAttempt(qrSession.Id, request.StudentId, ipAddress, userAgent, "Success", null);
                
                var response = new QrValidationResponse
                {
                    IsValid = true,
                    Message = "QR Code صحيح",
                    SessionId = qrSession.SessionId,
                    ValidatedAt = DateTime.UtcNow,
                    RemainingUsage = qrSession.MaxUsage - qrSession.UsageCount
                };
                
                return ApiResponse<QrValidationResponse>.SuccessResult(response, "تم التحقق من QR Code بنجاح");
            }
            catch (Exception ex)
            {
                return ApiResponse<QrValidationResponse>.ErrorResult($"خطأ في التحقق من QR Code: {ex.Message}", new List<string> { "500" });
            }
        }
        
        public async Task<ApiResponse<List<QrSessionDto>>> GetActiveQrCodesAsync(int sessionId)
        {
            try
            {
                var qrSessions = await _context.QrSessions
                    .Where(q => q.SessionId == sessionId && q.Status == "Active")
                    .Select(q => new QrSessionDto
                    {
                        Id = q.Id,
                        SessionId = q.SessionId,
                        Code = q.QrCode,
                        GeneratedAt = q.GeneratedAt,
                        ExpiresAt = q.ExpiresAt,
                        Status = q.Status,
                        UsageCount = q.UsageCount,
                        MaxUsage = q.MaxUsage
                    })
                    .ToListAsync();
        
                return ApiResponse<List<QrSessionDto>>.SuccessResult(qrSessions, "تم جلب QR Codes النشطة بنجاح");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<QrSessionDto>>.ErrorResult($"خطأ في جلب QR Codes: {ex.Message}", new List<string> { "500" });
            }
        }
        
        public async Task<ApiResponse<bool>> ExpireQrCodeAsync(int qrSessionId)
        {
            try
            {
                var qrSession = await _context.QrSessions.FindAsync(qrSessionId);
                if (qrSession == null)
                {
                    return ApiResponse<bool>.ErrorResult("QR Session غير موجود", new List<string> { "404" });
                }
        
                qrSession.Status = "Expired";
                await _context.SaveChangesAsync();
        
                return ApiResponse<bool>.SuccessResult(true, "تم إنهاء صلاحية QR Code بنجاح");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"خطأ في إنهاء صلاحية QR Code: {ex.Message}", new List<string> { "500" });
            }
        }
        
        public async Task<ApiResponse<List<QrUsageLogDto>>> GetQrUsageLogsAsync(int qrSessionId)
        {
            try
            {
                var logs = await _context.QrUsageLogs
                    .Where(l => l.QrSessionId == qrSessionId)
                    .OrderByDescending(l => l.AttemptTime)
                    .Select(l => new QrUsageLogDto
                    {
                        Id = l.Id,
                        QrSessionId = l.QrSessionId,
                        StudentId = l.StudentId,
                        IpAddress = l.IpAddress,
                        AttemptTime = l.AttemptTime,
                        Status = l.Status,
                        FailureReason = l.FailureReason
                    })
                    .ToListAsync();
        
                return ApiResponse<List<QrUsageLogDto>>.SuccessResult(logs, "تم جلب سجلات الاستخدام بنجاح");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<QrUsageLogDto>>.ErrorResult($"خطأ في جلب سجلات الاستخدام: {ex.Message}", new List<string> { "500" });
            }
        }
        
        public async Task<ApiResponse<Dictionary<string, object>>> GetQrSecurityStatsAsync(int sessionId)
        {
            try
            {
                var stats = await _context.QrUsageLogs
                    .Where(l => l.QrSession.SessionId == sessionId)
                    .GroupBy(l => l.Status)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToListAsync();
        
                var securityStats = new Dictionary<string, object>
                {
                    { "SessionId", sessionId },
                    { "TotalAttempts", stats.Sum(s => s.Count) },
                    { "SuccessfulAttempts", stats.FirstOrDefault(s => s.Status == "Success")?.Count ?? 0 },
                    { "FailedAttempts", stats.FirstOrDefault(s => s.Status == "Failed")?.Count ?? 0 },
                    { "BlockedAttempts", stats.FirstOrDefault(s => s.Status == "Blocked")?.Count ?? 0 }
                };
        
                return ApiResponse<Dictionary<string, object>>.SuccessResult(securityStats, "تم جلب إحصائيات الأمان بنجاح");
            }
            catch (Exception ex)
            {
                return ApiResponse<Dictionary<string, object>>.ErrorResult($"خطأ في جلب إحصائيات الأمان: {ex.Message}", new List<string> { "500" });
            }
        }
        
        public async Task<ApiResponse<int>> CleanupExpiredQrCodesAsync()
        {
            try
            {
                var expiredCodes = await _context.QrSessions
                    .Where(q => q.ExpiresAt <= DateTime.UtcNow && q.Status != "Expired")
                    .ToListAsync();
        
                foreach (var code in expiredCodes)
                {
                    code.Status = "Expired";
                }
        
                await _context.SaveChangesAsync();
        
                return ApiResponse<int>.SuccessResult(expiredCodes.Count, $"تم تنظيف {expiredCodes.Count} QR Code منتهي الصلاحية");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.ErrorResult($"خطأ في تنظيف QR Codes: {ex.Message}", new List<string> { "500" });
            }
        }
        
        // تنفيذ ExpireAllSessionQrCodesAsync كدالة عامة
        public async Task<ApiResponse<bool>> ExpireAllSessionQrCodesAsync(int sessionId)
        {
            try
            {
                var activeCodes = await _context.QrSessions
                    .Where(q => q.SessionId == sessionId && q.Status == "Active")
                    .ToListAsync();
        
                foreach (var code in activeCodes)
                {
                    code.Status = "Expired";
                }
        
                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, "تم إنهاء صلاحية جميع QR Codes بنجاح");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"خطأ في إنهاء صلاحية QR Codes: {ex.Message}", new List<string> { "500" });
            }
        }
        
        // تنفيذ BlockSuspiciousActivityAsync كدالة عامة
        public async Task<ApiResponse<bool>> BlockSuspiciousActivityAsync(string ipAddress, int minutes = 30)
        {
            try
            {
                _blockedIPs[ipAddress] = DateTime.UtcNow.AddMinutes(minutes);
                await Task.CompletedTask;
                return ApiResponse<bool>.SuccessResult(true, "تم حظر النشاط المشبوه بنجاح");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"خطأ في حظر النشاط المشبوه: {ex.Message}", new List<string> { "500" });
            }
        }
        
        #region Private Helper Methods
        
        private string GenerateSalt()
        {
            using var rng = RandomNumberGenerator.Create();
            var saltBytes = new byte[16];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }
        
        private string EncryptData(string data)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(_encryptionKey.PadRight(32).Substring(0, 32));
            aes.GenerateIV();
            
            using var encryptor = aes.CreateEncryptor();
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var encryptedBytes = encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
            
            var result = new byte[aes.IV.Length + encryptedBytes.Length];
            Array.Copy(aes.IV, 0, result, 0, aes.IV.Length);
            Array.Copy(encryptedBytes, 0, result, aes.IV.Length, encryptedBytes.Length);
            
            return Convert.ToBase64String(result);
        }
        
        private string GenerateSecureQrCode(string encryptedData, string salt)
        {
            var combinedData = $"{encryptedData}:{salt}:{DateTime.UtcNow.Ticks}";
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedData));
            return Convert.ToBase64String(hashBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }
        
        private string GenerateSecurityHash(string qrCode, string salt, string ipAddress)
        {
            var data = $"{qrCode}:{salt}:{ipAddress}:{_encryptionKey}";
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(hashBytes);
        }
        
        private string GenerateQrImage(string qrCode)
        {
            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(qrCode, QRCodeGenerator.ECCLevel.Q);
            using var qrCodeImage = new PngByteQRCode(qrCodeData);
            var qrCodeBytes = qrCodeImage.GetGraphic(20);
            return Convert.ToBase64String(qrCodeBytes);
        }
        
        private bool IsIpBlocked(string ipAddress)
        {
            if (_blockedIPs.ContainsKey(ipAddress))
            {
                if (_blockedIPs[ipAddress] > DateTime.UtcNow)
                {
                    return true;
                }
                _blockedIPs.Remove(ipAddress);
            }
            return false;
        }
        
        private async Task LogUsageAttempt(int? qrSessionId, int studentId, string ipAddress, string userAgent, string status, string? failureReason)
        {
            var log = new QrUsageLog
            {
                QrSessionId = qrSessionId ?? 0,
                StudentId = studentId,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                AttemptTime = DateTime.UtcNow,
                Status = status,
                FailureReason = failureReason
            };
            
            _context.QrUsageLogs.Add(log);
            await _context.SaveChangesAsync();
        }
        
        #endregion
        
        // تنفيذ الدوال المفقودة
        public async Task<ApiResponse<QrCodeResponse>> RegenerateQrCodeAsync(int sessionId, string ipAddress)
        {
            try
            {
                // التحقق من صحة الجلسة
                var session = await _context.Sessions
                    .Include(s => s.CourseAssignment)
                    .FirstOrDefaultAsync(s => s.Id == sessionId);
                    
                if (session == null)
                {
                    return ApiResponse<QrCodeResponse>.ErrorResult("الجلسة غير موجودة", new List<string> { "404" });
                }
                
                // التحقق من حالة الجلسة
                if (session.Status != "Active" && session.Status != "Scheduled")
                {
                    return ApiResponse<QrCodeResponse>.ErrorResult("لا يمكن توليد QR Code لهذه الجلسة", new List<string> { "400" });
                }
                
                // إنهاء QR Codes السابقة
                await ExpireAllSessionQrCodesAsync(sessionId);
                
                // توليد البيانات الآمنة
                var salt = GenerateSalt();
                var qrData = new
                {
                    SessionId = sessionId,
                    Timestamp = DateTime.UtcNow.Ticks,
                    Salt = salt,
                    DeviceFingerprint = "Regenerated"
                };
                
                var jsonData = JsonSerializer.Serialize(qrData);
                var encryptedData = EncryptData(jsonData);
                var qrCode = GenerateSecureQrCode(encryptedData, salt);
                var securityHash = GenerateSecurityHash(qrCode, salt, ipAddress);
                
                // إنشاء QR Session
                var qrSession = new QrSession
                {
                    SessionId = sessionId,
                    QrCode = qrCode,
                    EncryptedData = encryptedData,
                    GeneratedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(5), // افتراضي 5 دقائق
                    Status = "Active",
                    DeviceFingerprint = "Regenerated",
                    IpAddress = ipAddress,
                    MaxUsage = 1, // افتراضي استخدام واحد
                    SecurityHash = securityHash,
                    Salt = salt
                };
                
                _context.QrSessions.Add(qrSession);
                await _context.SaveChangesAsync();
                
                // توليد صورة QR Code
                var qrImageBase64 = GenerateQrImage(qrCode);
                
                var response = new QrCodeResponse
                {
                    QrCode = qrCode,
                    QrImageBase64 = qrImageBase64,
                    GeneratedAt = qrSession.GeneratedAt,
                    ExpiresAt = qrSession.ExpiresAt,
                    MaxUsage = qrSession.MaxUsage,
                    SecurityLevel = "High"
                };
                
                return ApiResponse<QrCodeResponse>.SuccessResult(response, "تم إعادة توليد QR Code بنجاح");
            }
            catch (Exception ex)
            {
                return ApiResponse<QrCodeResponse>.ErrorResult($"خطأ في إعادة توليد QR Code: {ex.Message}", new List<string> { "500" });
            }
        }
        
        public async Task<ApiResponse<bool>> IsQrCodeValidAsync(string qrCode)
        {
            try
            {
                var qrSession = await _context.QrSessions
                    .FirstOrDefaultAsync(q => q.QrCode == qrCode && q.Status == "Active");
                    
                if (qrSession == null)
                {
                    return ApiResponse<bool>.SuccessResult(false, "QR Code غير صحيح");
                }
                
                // التحقق من انتهاء الصلاحية
                if (qrSession.ExpiresAt <= DateTime.UtcNow)
                {
                    qrSession.Status = "Expired";
                    await _context.SaveChangesAsync();
                    return ApiResponse<bool>.SuccessResult(false, "QR Code منتهي الصلاحية");
                }
                
                // التحقق من عدد مرات الاستخدام
                if (qrSession.UsageCount >= qrSession.MaxUsage)
                {
                    return ApiResponse<bool>.SuccessResult(false, "تم استنفاد عدد مرات استخدام QR Code");
                }
                
                return ApiResponse<bool>.SuccessResult(true, "QR Code صالح");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"خطأ في التحقق من صلاحية QR Code: {ex.Message}", new List<string> { "500" });
            }
        }
        
        public async Task<ApiResponse<bool>> CleanupOldLogsAsync(int daysOld = 30)
        {
            try
            {
                var cutoffDate = DateTime.UtcNow.AddDays(-daysOld);
                var oldLogs = await _context.QrUsageLogs
                    .Where(l => l.AttemptTime < cutoffDate)
                    .ToListAsync();
                
                _context.QrUsageLogs.RemoveRange(oldLogs);
                await _context.SaveChangesAsync();
                
                return ApiResponse<bool>.SuccessResult(true, $"تم حذف {oldLogs.Count} سجل قديم");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"خطأ في حذف السجلات القديمة: {ex.Message}", new List<string> { "500" });
            }
        }
    }
}
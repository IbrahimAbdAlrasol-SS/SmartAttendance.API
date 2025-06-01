using SmartAttendance.API.Models.DTOs;
using SmartAttendance.API.Services.Interfaces;

namespace SmartAttendance.API.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FileService> _logger;
        private readonly string[] _allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB

        public FileService(IWebHostEnvironment environment, ILogger<FileService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public async Task<ApiResponse<string>> UploadImageAsync(IFormFile file, string folder, string? fileName = null)
        {
            try
            {
                if (!IsValidImageFile(file))
                {
                    return ApiResponse<string>.ErrorResult(
                        "نوع الملف غير مدعوم. الأنواع المدعومة: jpg, jpeg, png, gif", 
                        statusCode: 400);
                }

                if (file.Length > _maxFileSize)
                {
                    return ApiResponse<string>.ErrorResult(
                        "حجم الملف كبير جداً. الحد الأقصى 5 ميجابايت", 
                        statusCode: 400);
                }

                // إنشاء المجلد إذا لم يكن موجوداً
                var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads", folder);
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                // إنشاء اسم الملف
                var fileExtension = Path.GetExtension(file.FileName);
                var finalFileName = !string.IsNullOrEmpty(fileName) 
                    ? $"{fileName}_{DateTime.UtcNow:yyyyMMddHHmmss}{fileExtension}"
                    : $"{Guid.NewGuid()}{fileExtension}";

                var filePath = Path.Combine(uploadsPath, finalFileName);
                var relativePath = Path.Combine("uploads", folder, finalFileName).Replace("\\", "/");

                // حفظ الملف
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return ApiResponse<string>.SuccessResult(
                    relativePath, "تم رفع الملف بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file");
                return ApiResponse<string>.ErrorResult(
                    "حدث خطأ أثناء رفع الملف", statusCode: 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteFileAsync(string filePath)
        {
            try
            {
                var fullPath = Path.Combine(_environment.WebRootPath, filePath);
                
                if (File.Exists(fullPath))
                {
                    await Task.Run(() => File.Delete(fullPath));
                    return ApiResponse<bool>.SuccessResult(true, "تم حذف الملف بنجاح");
                }

                return ApiResponse<bool>.SuccessResult(false, "الملف غير موجود");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file {FilePath}", filePath);
                return ApiResponse<bool>.ErrorResult(
                    "حدث خطأ أثناء حذف الملف", statusCode: 500);
            }
        }

        public async Task<ApiResponse<byte[]>> GetFileAsync(string filePath)
        {
            try
            {
                var fullPath = Path.Combine(_environment.WebRootPath, filePath);
                
                if (!File.Exists(fullPath))
                {
                    return ApiResponse<byte[]>.ErrorResult(
                        "الملف غير موجود", statusCode: 404);
                }

                var fileBytes = await File.ReadAllBytesAsync(fullPath);
                return ApiResponse<byte[]>.SuccessResult(
                    fileBytes, "تم جلب الملف بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting file {FilePath}", filePath);
                return ApiResponse<byte[]>.ErrorResult(
                    "حدث خطأ أثناء جلب الملف", statusCode: 500);
            }
        }

        public bool IsValidImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return _allowedImageExtensions.Contains(extension);
        }

        public async Task<ApiResponse<string>> ProcessFaceImageAsync(IFormFile image, int studentId)
        {
            try
            {
                // رفع الصورة أولاً
                var uploadResult = await UploadImageAsync(image, "faces", $"student_{studentId}");
                
                if (!uploadResult.Success)
                {
                    return uploadResult;
                }

                // TODO: إضافة معالجة Face Recognition هنا
                // يمكن إضافة استدعاء لـ Python API للمعالجة
                // var faceEncoding = await CallPythonFaceRecognitionAPI(uploadResult.Data);

                return uploadResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing face image for student {StudentId}", studentId);
                return ApiResponse<string>.ErrorResult(
                    "حدث خطأ أثناء معالجة صورة الوجه", statusCode: 500);
            }
        }
    }
}
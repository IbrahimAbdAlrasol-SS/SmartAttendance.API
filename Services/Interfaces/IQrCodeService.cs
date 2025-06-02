using SmartAttendance.API.Models.DTOs;

namespace SmartAttendance.API.Services.Interfaces
{
    public interface IQrCodeService
    {
        // QR Generation
        Task<ApiResponse<QrCodeResponse>> GenerateQrCodeAsync(GenerateQrRequest request, string ipAddress, string userAgent);
        Task<ApiResponse<QrCodeResponse>> RegenerateQrCodeAsync(int sessionId, string ipAddress);
        
        // QR Validation
        Task<ApiResponse<QrValidationResponse>> ValidateQrCodeAsync(ValidateQrRequest request, string ipAddress, string userAgent);
        Task<ApiResponse<bool>> IsQrCodeValidAsync(string qrCode);
        
        // QR Management
        Task<ApiResponse<bool>> ExpireQrCodeAsync(int qrSessionId);
        Task<ApiResponse<bool>> ExpireAllSessionQrCodesAsync(int sessionId);
        Task<ApiResponse<List<QrSessionDto>>> GetActiveQrCodesAsync(int sessionId);
        
        // Security & Monitoring
        Task<ApiResponse<List<QrUsageLogDto>>> GetQrUsageLogsAsync(int qrSessionId);
        Task<ApiResponse<bool>> BlockSuspiciousActivityAsync(string ipAddress, int minutes = 30);
        // تغيير من
        // Task<ApiResponse<Dictionary<string, object>>> GetQrSecurityStatsAsync(int sessionId);
        // إلى
        Task<ApiResponse<Dictionary<string, object>>> GetQrSecurityStatsAsync(int sessionId);
        
        // Cleanup
        Task<ApiResponse<int>> CleanupExpiredQrCodesAsync();
        Task<ApiResponse<bool>> CleanupOldLogsAsync(int daysOld = 30);
    }
}
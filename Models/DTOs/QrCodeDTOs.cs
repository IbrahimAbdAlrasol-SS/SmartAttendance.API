using System.ComponentModel.DataAnnotations;

namespace SmartAttendance.API.Models.DTOs
{
    // Request DTOs
    public class GenerateQrRequest
    {
        [Required]
        public int SessionId { get; set; }
        
        [Range(1, 1440)] // 1 minute to 24 hours
        public int ExpirationMinutes { get; set; } = 15;
        
        [Range(1, 1000)]
        public int MaxUsage { get; set; } = 1;
        
        public string? DeviceFingerprint { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
    
    public class ValidateQrRequest
    {
        [Required]
        public string QrCode { get; set; } = string.Empty;
        
        [Required]
        public int StudentId { get; set; }
        
        public string? DeviceFingerprint { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
    
    // Response DTOs
    public class QrCodeResponse
    {
        public string QrCode { get; set; } = string.Empty;
        public string QrImageBase64 { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int MaxUsage { get; set; }
        public string SecurityLevel { get; set; } = string.Empty;
    }
    
    public class QrValidationResponse
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? SessionId { get; set; }
        public DateTime? ValidatedAt { get; set; }
        public int RemainingUsage { get; set; }
    }
    
    public class QrSessionDto
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int UsageCount { get; set; }
        public int MaxUsage { get; set; }
        public string SecurityLevel { get; set; } = string.Empty;
    }
    
    public class QrUsageLogDto
    {
        public int Id { get; set; }
        public int QrSessionId { get; set; }
        public int? StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public DateTime AttemptTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? FailureReason { get; set; }
    }
}
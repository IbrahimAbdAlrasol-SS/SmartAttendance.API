using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAttendance.API.Models.Entities
{
    public class QrSession : BaseEntity
    {
        [Required]
        [ForeignKey("Session")]
        public int SessionId { get; set; }
        
        [Required]
        [StringLength(500)]
        public string QrCode { get; set; } = string.Empty;
        
        [Required]
        [StringLength(1000)]
        public string EncryptedData { get; set; } = string.Empty;
        
        [Required]
        public DateTime GeneratedAt { get; set; }
        
        [Required]
        public DateTime ExpiresAt { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active"; // Active, Expired, Used
        
        [StringLength(100)]
        public string? DeviceFingerprint { get; set; }
        
        [StringLength(50)]
        public string? IpAddress { get; set; }
        
        public int UsageCount { get; set; } = 0;
        public int MaxUsage { get; set; } = 1;
        
        // Security Fields
        [Required]
        [StringLength(64)]
        public string SecurityHash { get; set; } = string.Empty;
        
        [Required]
        [StringLength(32)]
        public string Salt { get; set; } = string.Empty;
        
        // Navigation Properties
        public Session Session { get; set; } = null!;
        public ICollection<QrUsageLog> UsageLogs { get; set; } = new List<QrUsageLog>();
    }
}
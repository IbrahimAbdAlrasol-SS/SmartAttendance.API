using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAttendance.API.Models.Entities
{
    public class QrUsageLog : BaseEntity
    {
        [Required]
        [ForeignKey("QrSession")]
        public int QrSessionId { get; set; }
        
        [ForeignKey("Student")]
        public int? StudentId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string IpAddress { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string UserAgent { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string DeviceFingerprint { get; set; } = string.Empty;
        
        [Required]
        public DateTime AttemptTime { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty; // Success, Failed, Blocked
        
        [StringLength(200)]
        public string? FailureReason { get; set; }
        
        [Column(TypeName = "decimal(10,6)")]
        public decimal? Latitude { get; set; }
        
        [Column(TypeName = "decimal(10,6)")]
        public decimal? Longitude { get; set; }
        
        // Navigation Properties
        public QrSession QrSession { get; set; } = null!;
        public Student? Student { get; set; }
    }
}
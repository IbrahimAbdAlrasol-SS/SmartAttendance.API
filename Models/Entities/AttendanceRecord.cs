using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAttendance.API.Models.Entities
{
    public class AttendanceRecord : BaseEntity
    {
        [Required]
        [ForeignKey("Session")]
        public int SessionId { get; set; }
        
        [Required]
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        
        public System.DateTime? EntryTime { get; set; }
        public System.DateTime? ExitTime { get; set; }
        
        [Required]
        [StringLength(20)]
        public string AttendanceStatus { get; set; } = "Absent"; // Present, Absent, Late, Excused
        
        [Required]
        [StringLength(20)]
        public string DetectionMethod { get; set; } = "Manual"; // FaceRecognition, Manual, QRCode
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        [Range(0.0, 100.0)]
        public double? FaceConfidence { get; set; } // For face recognition
        
        public bool IsVerified { get; set; } = false;
        
        // Navigation Properties
        public Session Session { get; set; } = null!;
        public Student Student { get; set; } = null!;
    }
}
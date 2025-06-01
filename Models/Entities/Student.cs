using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAttendance.API.Models.Entities
{
    public class Student : BaseEntity
    {
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        
        [StringLength(20)]
        public string? StudentCode { get; set; }
        
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string Stage { get; set; } = string.Empty; // First, Second, Third, Fourth
        
        [Required]
        [StringLength(20)]
        public string StudyType { get; set; } = string.Empty; // Morning, Evening
        
        [Required]
        [StringLength(5)]
        public string Section { get; set; } = string.Empty; // A, B, C, D
        
        [StringLength(15)]
        public string? Phone { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        [StringLength(500)]
        public string? Address { get; set; }
        
        [StringLength(255)]
        public string? ProfileImage { get; set; }
        
        // Face Recognition Data
        [Column(TypeName = "TEXT")]
        public string? FaceEncodingData { get; set; }
        
        public DateTime? LastFaceUpdate { get; set; }
        
        // Navigation Properties
        public User User { get; set; } = null!;
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
    }
}
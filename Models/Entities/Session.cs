using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SmartAttendance.API.Models.Entities
{
    public class Session : BaseEntity
    {
        [Required]
        [ForeignKey("CourseAssignment")]
        public int CourseAssignmentId { get; set; }
        
        [Required]
        public System.DateTime SessionDate { get; set; }
        
        [Required]
        public System.TimeSpan StartTime { get; set; }
        
        [Required]
        public System.TimeSpan EndTime { get; set; }
        
        [StringLength(20)]
        public string Status { get; set; } = "Scheduled"; // Scheduled, Active, Completed, Cancelled
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public System.DateTime? ActualStartTime { get; set; }
        public System.DateTime? ActualEndTime { get; set; }
        
        // Statistics
        public int TotalStudents { get; set; } = 0;
        public int PresentStudents { get; set; } = 0;
        public int AbsentStudents { get; set; } = 0;
        
        // Navigation Properties
        public CourseAssignment CourseAssignment { get; set; } = null!;
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
    }
}
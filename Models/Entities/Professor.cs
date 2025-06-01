using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAttendance.API.Models.Entities
{
    public class Professor : BaseEntity
    {
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        
        [StringLength(20)]
        public string? EmployeeCode { get; set; }
        
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? Department { get; set; }
        
        [StringLength(100)]
        public string? Title { get; set; } // Dr., Prof., Mr., etc.
        
        [StringLength(15)]
        public string? Phone { get; set; }
        
        [StringLength(500)]
        public string? Bio { get; set; }
        
        [StringLength(255)]
        public string? ProfileImage { get; set; }
        
        // Navigation Properties
        public User User { get; set; } = null!;
        public ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();
    }
}
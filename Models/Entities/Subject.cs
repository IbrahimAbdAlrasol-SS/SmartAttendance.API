using System.ComponentModel.DataAnnotations;

namespace SmartAttendance.API.Models.Entities
{
    public class Subject : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string Code { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string Stage { get; set; } = string.Empty; // First, Second, Third, Fourth
        
        [Required]
        [StringLength(20)]
        public string StudyType { get; set; } = string.Empty; // Morning, Evening
        
        [Range(1, 6)]
        public int CreditHours { get; set; } = 3;
        
        [StringLength(500)]
        public string? Description { get; set; }
         
        public new bool IsActive { get; set; } = true;
        
        // Navigation Properties
        public ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();
    }
}
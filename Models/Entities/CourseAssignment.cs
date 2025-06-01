using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAttendance.API.Models.Entities
{
    public class CourseAssignment : BaseEntity
    {
        [Required]
        [ForeignKey("Professor")]
        public int ProfessorId { get; set; }
        
        [Required]
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        
        [Required]
        [StringLength(5)]
        public string Section { get; set; } = string.Empty; // A, B, C, D
        
        [Required]
        [StringLength(20)]
        public string AcademicYear { get; set; } = string.Empty; // 2024-2025
        
        [Required]
        [StringLength(20)]
        public string Semester { get; set; } = string.Empty; // الفصل الأول، الفصل الثاني
        
        public bool IsActive { get; set; } = true;
        
        // Navigation Properties
        public Professor Professor { get; set; } = null!;
        public Subject Subject { get; set; } = null!;
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
using System.ComponentModel.DataAnnotations;

namespace SmartAttendance.API.Models.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string UserType { get; set; } = string.Empty; // Admin, Student, Professor
        
        public bool EmailVerified { get; set; } = false;
        
        // Navigation Properties
        public Student? Student { get; set; }
        public Professor? Professor { get; set; }
    }
}
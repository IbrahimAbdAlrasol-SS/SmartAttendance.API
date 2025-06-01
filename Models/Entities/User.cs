using System.ComponentModel.DataAnnotations;

namespace SmartAttendance.API.Models.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20)]
        public string UserType { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        public bool EmailVerified { get; set; } = false;
        
        // Navigation Properties
        public Student? Student { get; set; }
        public Professor? Professor { get; set; }
    }
}
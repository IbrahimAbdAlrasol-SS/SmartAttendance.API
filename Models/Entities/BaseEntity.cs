using System.ComponentModel.DataAnnotations;

namespace SmartAttendance.API.Models.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public bool IsDeleted { get; set; } = false;
        
        public int? CreatedBy { get; set; }
        
        public int? UpdatedBy { get; set; }
    }
}
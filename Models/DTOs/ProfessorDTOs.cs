using System.ComponentModel.DataAnnotations;

namespace SmartAttendance.API.Models.DTOs
{
    public class ProfessorDto
    {
        public int Id { get; set; }
        public string? EmployeeCode { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Department { get; set; }
        public string? Title { get; set; }
        public string? Phone { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImage { get; set; }
        public bool IsActive { get; set; }
        public string? Email { get; set; }
    }

    public class CreateProfessorRequest
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم الكامل يجب أن يكون أقل من 100 حرف")]
        public string FullName { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "رقم الموظف يجب أن يكون أقل من 20 حرف")]
        public string? EmployeeCode { get; set; }

        [StringLength(100, ErrorMessage = "القسم يجب أن يكون أقل من 100 حرف")]
        public string? Department { get; set; }

        [StringLength(100, ErrorMessage = "اللقب يجب أن يكون أقل من 100 حرف")]
        public string? Title { get; set; }

        [StringLength(15, ErrorMessage = "رقم الهاتف يجب أن يكون أقل من 15 رقم")]
        public string? Phone { get; set; }

        [StringLength(500, ErrorMessage = "السيرة الذاتية يجب أن تكون أقل من 500 حرف")]
        public string? Bio { get; set; }
    }

    public class UpdateProfessorRequest
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم الكامل يجب أن يكون أقل من 100 حرف")]
        public string FullName { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "رقم الموظف يجب أن يكون أقل من 20 حرف")]
        public string? EmployeeCode { get; set; }

        [StringLength(100, ErrorMessage = "القسم يجب أن يكون أقل من 100 حرف")]
        public string? Department { get; set; }

        [StringLength(100, ErrorMessage = "اللقب يجب أن يكون أقل من 100 حرف")]
        public string? Title { get; set; }

        [StringLength(15, ErrorMessage = "رقم الهاتف يجب أن يكون أقل من 15 رقم")]
        public string? Phone { get; set; }

        [StringLength(500, ErrorMessage = "السيرة الذاتية يجب أن تكون أقل من 500 حرف")]
        public string? Bio { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class ProfessorListDto
    {
        public int Id { get; set; }
        public string? EmployeeCode { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Department { get; set; }
        public string? Title { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
        public string? Email { get; set; }
    }

    public class ProfessorSearchRequest
    {
        public string? SearchTerm { get; set; }
        public string? Department { get; set; }
        public bool? IsActive { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
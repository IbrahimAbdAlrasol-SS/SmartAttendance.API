using System.ComponentModel.DataAnnotations;

namespace SmartAttendance.API.Models.DTOs
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string? StudentCode { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Stage { get; set; } = string.Empty;
        public string StudyType { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? ProfileImage { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastFaceUpdate { get; set; }
        public string? Email { get; set; }
    }

    public class CreateStudentRequest
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم الكامل يجب أن يكون أقل من 100 حرف")]
        public string FullName { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "رقم الطالب يجب أن يكون أقل من 20 حرف")]
        public string? StudentCode { get; set; }

        [Required(ErrorMessage = "المرحلة مطلوبة")]
        public string Stage { get; set; } = string.Empty;

        [Required(ErrorMessage = "نوع الدراسة مطلوب")]
        public string StudyType { get; set; } = string.Empty;

        [Required(ErrorMessage = "الشعبة مطلوبة")]
        public string Section { get; set; } = string.Empty;

        [StringLength(15, ErrorMessage = "رقم الهاتف يجب أن يكون أقل من 15 رقم")]
        public string? Phone { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(500, ErrorMessage = "العنوان يجب أن يكون أقل من 500 حرف")]
        public string? Address { get; set; }
    }

    public class UpdateStudentRequest
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم الكامل يجب أن يكون أقل من 100 حرف")]
        public string FullName { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "رقم الطالب يجب أن يكون أقل من 20 حرف")]
        public string? StudentCode { get; set; }

        [Required(ErrorMessage = "المرحلة مطلوبة")]
        public string Stage { get; set; } = string.Empty;

        [Required(ErrorMessage = "نوع الدراسة مطلوب")]
        public string StudyType { get; set; } = string.Empty;

        [Required(ErrorMessage = "الشعبة مطلوبة")]
        public string Section { get; set; } = string.Empty;

        [StringLength(15, ErrorMessage = "رقم الهاتف يجب أن يكون أقل من 15 رقم")]
        public string? Phone { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(500, ErrorMessage = "العنوان يجب أن يكون أقل من 500 حرف")]
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class StudentListDto
    {
        public int Id { get; set; }
        public string? StudentCode { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Stage { get; set; } = string.Empty;
        public string StudyType { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
        public bool HasFaceData { get; set; }
        public string? Email { get; set; }
    }

    public class StudentSearchRequest
    {
        public string? SearchTerm { get; set; }
        public string? Stage { get; set; }
        public string? StudyType { get; set; }
        public string? Section { get; set; }
        public bool? IsActive { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class UpdateStudentFaceRequest
    {
        [Required(ErrorMessage = "بيانات الوجه مطلوبة")]
        public string FaceEncodingData { get; set; } = string.Empty;
    }
}
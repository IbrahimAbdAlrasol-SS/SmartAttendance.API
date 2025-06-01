using System.ComponentModel.DataAnnotations;

namespace SmartAttendance.API.Models.DTOs
{
    // Login DTOs
    public class LoginRequest
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public UserInfoDto User { get; set; } = null!;
    }

    // Register DTOs
    public class RegisterStudentRequest
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        public string Password { get; set; } = string.Empty;

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

    public class RegisterProfessorRequest
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        public string Password { get; set; } = string.Empty;

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

    // User Info DTO
    public class UserInfoDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool EmailVerified { get; set; }
        public StudentDto? Student { get; set; }
        public ProfessorDto? Professor { get; set; }
    }

    // Change Password DTO
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "كلمة المرور الحالية مطلوبة")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور الجديدة يجب أن تكون 6 أحرف على الأقل")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [Compare("NewPassword", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقتين")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    // Refresh Token DTO
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "Refresh Token مطلوب")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SmartAttendance.API.Services.Interfaces;
using SmartAttendance.API.Models.DTOs;
using SmartAttendance.API.Constants;
using System.Security.Claims;

namespace SmartAttendance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// جلب جميع الطلاب - للأساتذة والإداريين فقط
        /// </summary>
        [HttpGet]
        [Authorize(Roles = $"{UserRoles.Professor},{UserRoles.Admin}")]
        public async Task<ActionResult<ApiResponse<PagedResult<StudentListDto>>>> GetStudents(
            [FromQuery] StudentFilterRequest? filter = null)
        {
            var result = await _studentService.GetStudentsAsync(filter);
            return StatusCode(result.StatusCode ?? 500, result);
        }

        /// <summary>
        /// جلب طالب محدد بواسطة ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<StudentDto>>> GetStudent(int id)
        {
            // الطالب يمكنه رؤية بياناته فقط، الأساتذة يمكنهم رؤية أي طالب
            var currentUserId = GetCurrentUserId();
            var userType = GetCurrentUserType();

            if (userType == UserRoles.Student)
            {
                // التأكد أن الطالب يطلب بياناته فقط
                var currentStudent = await _studentService.GetStudentByUserIdAsync(currentUserId!.Value);
                if (currentStudent?.Data?.Id != id)
                {
                    return Forbid();
                }
            }

            var result = await _studentService.GetStudentByIdAsync(id);
            return StatusCode(result.StatusCode ?? 500, result);
        }

        /// <summary>
        /// إنشاء طالب جديد - للإداريين فقط
        /// </summary>
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<ApiResponse<StudentDto>>> CreateStudent([FromBody] CreateStudentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<StudentDto>.ErrorResult(
                    "بيانات غير صحيحة",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList(),
                    400));
            }

            var result = await _studentService.CreateStudentAsync(request);
            
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetStudent), new { id = result.Data!.Id }, result);
            }
            
            return StatusCode(result.StatusCode ?? 500, result);
        }

        /// <summary>
        /// تحديث بيانات طالب
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<StudentDto>>> UpdateStudent(int id, [FromBody] UpdateStudentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<StudentDto>.ErrorResult(
                    "بيانات غير صحيحة",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList(),
                    400));
            }

            // الطالب يمكنه تحديث بياناته فقط، الإداريين يمكنهم تحديث أي طالب
            var currentUserId = GetCurrentUserId();
            var userType = GetCurrentUserType();

            if (userType == UserRoles.Student)
            {
                var currentStudent = await _studentService.GetStudentByUserIdAsync(currentUserId!.Value);
                if (currentStudent?.Data?.Id != id)
                {
                    return Forbid();
                }
            }

            var result = await _studentService.UpdateStudentAsync(id, request);
            return StatusCode(result.StatusCode ?? 500, result);
        }

        /// <summary>
        /// حذف طالب - للإداريين فقط
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteStudent(int id)
        {
            var result = await _studentService.DeleteStudentAsync(id);
            return StatusCode(result.StatusCode ?? 500, result);
        }

        /// <summary>
        /// رفع صورة الوجه للطالب
        /// </summary>
        [HttpPost("{id}/face-image")]
        public async Task<ActionResult<ApiResponse<bool>>> UploadFaceImage(int id, IFormFile faceImage)
        {
            if (faceImage == null || faceImage.Length == 0)
            {
                return BadRequest(ApiResponse<bool>.ErrorResult(
                    "صورة الوجه مطلوبة", 
                    statusCode: 400));
            }

            // التحقق من صيغة الصورة
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(faceImage.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(ApiResponse<bool>.ErrorResult(
                    "صيغة الصورة غير مدعومة. الصيغ المدعومة: JPG, JPEG, PNG", 
                    statusCode: 400));
            }

            // التحقق من حجم الصورة (أقل من 5 ميجا)
            if (faceImage.Length > 5 * 1024 * 1024)
            {
                return BadRequest(ApiResponse<bool>.ErrorResult(
                    "حجم الصورة كبير جداً. الحد الأقصى 5 ميجابايت", 
                    statusCode: 400));
            }

            // الطالب يمكنه رفع صورته فقط، الإداريين يمكنهم رفع صورة لأي طالب
            var currentUserId = GetCurrentUserId();
            var userType = GetCurrentUserType();

            if (userType == UserRoles.Student)
            {
                var currentStudent = await _studentService.GetStudentByUserIdAsync(currentUserId!.Value);
                if (currentStudent?.Data?.Id != id)
                {
                    return Forbid();
                }
            }

            var result = await _studentService.UploadFaceImageAsync(id, faceImage);
            return StatusCode(result.StatusCode ?? 500, result);
        }

        /// <summary>
        /// حذف صورة الوجه للطالب
        /// </summary>
        [HttpDelete("{id}/face-image")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteFaceImage(int id)
        {
            // الطالب يمكنه حذف صورته فقط، الإداريين يمكنهم حذف صورة أي طالب
            var currentUserId = GetCurrentUserId();
            var userType = GetCurrentUserType();

            if (userType == UserRoles.Student)
            {
                var currentStudent = await _studentService.GetStudentByUserIdAsync(currentUserId!.Value);
                if (currentStudent?.Data?.Id != id)
                {
                    return Forbid();
                }
            }

            var result = await _studentService.DeleteFaceImageAsync(id);
            return StatusCode(result.StatusCode ?? 500, result);
        }

        /// <summary>
        /// البحث عن الطلاب
        /// </summary>
        [HttpGet("search")]
        [Authorize(Roles = $"{UserRoles.Professor},{UserRoles.Admin}")]
        public async Task<ActionResult<ApiResponse<List<StudentListDto>>>> SearchStudents([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(ApiResponse<List<StudentListDto>>.ErrorResult(
                    "نص البحث مطلوب", 
                    statusCode: 400));
            }

            var result = await _studentService.SearchStudentsAsync(query);
            return StatusCode(result.StatusCode ?? 500, result);
        }

        /// <summary>
        /// جلب إحصائيات الطلاب
        /// </summary>
        [HttpGet("statistics")]
        [Authorize(Roles = $"{UserRoles.Professor},{UserRoles.Admin}")]
        public async Task<ActionResult<ApiResponse<StudentStatisticsDto>>> GetStatistics()
        {
            var result = await _studentService.GetStudentStatisticsAsync();
            return StatusCode(result.StatusCode ?? 500, result);
        }

        /// <summary>
        /// جلب بيانات الطالب الحالي
        /// </summary>
        [HttpGet("my-profile")]
        [Authorize(Roles = UserRoles.Student)]
        public async Task<ActionResult<ApiResponse<StudentDto>>> GetMyProfile()
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var result = await _studentService.GetStudentByUserIdAsync(currentUserId.Value);
            return StatusCode(result.StatusCode ?? 500, result);
        }

        /// <summary>
        /// جلب طلاب شعبة معينة
        /// </summary>
        [HttpGet("by-section")]
        [Authorize(Roles = $"{UserRoles.Professor},{UserRoles.Admin}")]
        public async Task<ActionResult<ApiResponse<List<StudentListDto>>>> GetStudentsBySection(
            [FromQuery] string stage,
            [FromQuery] string studyType,
            [FromQuery] string section)
        {
            if (string.IsNullOrWhiteSpace(stage) || string.IsNullOrWhiteSpace(studyType) || string.IsNullOrWhiteSpace(section))
            {
                return BadRequest(ApiResponse<List<StudentListDto>>.ErrorResult(
                    "المرحلة ونوع الدراسة والشعبة مطلوبة", 
                    statusCode: 400));
            }

            var result = await _studentService.GetStudentsBySectionAsync(stage, studyType, section);
            return StatusCode(result.StatusCode ?? 500, result);
        }

        #region Helper Methods

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return null;
        }

        private string? GetCurrentUserType()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value;
        }

        #endregion
    }
}
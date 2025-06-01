using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SmartAttendance.API.Services;
using SmartAttendance.API.Models.DTOs;
using SmartAttendance.API.Constants;
using System.Security.Claims;

namespace SmartAttendance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// تسجيل دخول المستخدم
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<LoginResponse>.ErrorResult(
                    "بيانات غير صحيحة",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList(),
                    400));
            }

            var result = await _authService.LoginAsync(request);
            
            if (!result.Success)
            {
                return StatusCode(result.StatusCode ?? 500, result);
            }

            return Ok(result);
        }

        /// <summary>
        /// تسجيل طالب جديد
        /// </summary>
        [HttpPost("register/student")]
        public async Task<ActionResult<ApiResponse<UserInfoDto>>> RegisterStudent([FromBody] RegisterStudentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<UserInfoDto>.ErrorResult(
                    "بيانات غير صحيحة",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList(),
                    400));
            }

            var result = await _authService.RegisterStudentAsync(request);
            
            if (!result.Success)
            {
                return StatusCode(result.StatusCode ?? 500, result);
            }

            return CreatedAtAction(nameof(GetProfile), new { }, result);
        }

        /// <summary>
        /// تسجيل أستاذ جديد
        /// </summary>
        [HttpPost("register/professor")]
        public async Task<ActionResult<ApiResponse<UserInfoDto>>> RegisterProfessor([FromBody] RegisterProfessorRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<UserInfoDto>.ErrorResult(
                    "بيانات غير صحيحة",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList(),
                    400));
            }

            var result = await _authService.RegisterProfessorAsync(request);
            
            if (!result.Success)
            {
                return StatusCode(result.StatusCode ?? 500, result);
            }

            return CreatedAtAction(nameof(GetProfile), new { }, result);
        }

        /// <summary>
        /// تجديد الرمز المميز
        /// </summary>
        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.RefreshToken))
            {
                return BadRequest(ApiResponse<LoginResponse>.ErrorResult(
                    "Refresh token مطلوب", 
                    statusCode: 400));
            }

            var result = await _authService.RefreshTokenAsync(request.RefreshToken);
            
            if (!result.Success)
            {
                return StatusCode(result.StatusCode ?? 500, result);
            }

            return Ok(result);
        }

        /// <summary>
        /// تغيير كلمة المرور
        /// </summary>
        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<bool>>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<bool>.ErrorResult(
                    "بيانات غير صحيحة",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList(),
                    400));
            }

            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(ApiResponse<bool>.ErrorResult(
                    "غير مصرح لك بالوصول", 
                    statusCode: 401));
            }

            var result = await _authService.ChangePasswordAsync(userId.Value, request);
            
            if (!result.Success)
            {
                return StatusCode(result.StatusCode ?? 500, result);
            }

            return Ok(result);
        }

        /// <summary>
        /// جلب بيانات المستخدم الحالي
        /// </summary>
        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserInfoDto>>> GetProfile()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(ApiResponse<UserInfoDto>.ErrorResult(
                    "غير مصرح لك بالوصول", 
                    statusCode: 401));
            }

            var result = await _authService.GetUserProfileAsync(userId.Value);
            
            if (!result.Success)
            {
                return StatusCode(result.StatusCode ?? 500, result);
            }

            return Ok(result);
        }

        /// <summary>
        /// تسجيل خروج المستخدم
        /// </summary>
        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<bool>>> Logout()
        {
            // TODO: Implement token blacklisting
            // For now, client should just remove the token
            
            return Ok(ApiResponse<bool>.SuccessResult(true, "تم تسجيل الخروج بنجاح"));
        }

        /// <summary>
        /// التحقق من صحة الرمز المميز
        /// </summary>
        [HttpGet("validate-token")]
        [Authorize]
        // Either remove async:
        public ActionResult<ApiResponse<object>> ValidateToken()
        {
            return Ok(ApiResponse<object>.SuccessResult(new { valid = true }, "Token is valid"));
        }
        
        // Or add await operations if needed:
        

        /// <summary>
        /// جلب معلومات المستخدم الأساسية
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        public ActionResult<ApiResponse<object>> GetCurrentUserInfo()
        {
            var userId = GetCurrentUserId();
            var userEmail = GetCurrentUserEmail();
            var userType = GetCurrentUserType();
        
            if (userId == null)
            {
                return Unauthorized(ApiResponse<object>.ErrorResult(
                    "غير مصرح لك بالوصول", 
                    statusCode: 401));
            }
        
            return Ok(ApiResponse<object>.SuccessResult(new
            {
                id = userId.Value,
                email = userEmail,
                userType = userType,
                timestamp = DateTime.UtcNow
            }, "تم جلب بيانات المستخدم بنجاح"));
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

        private string? GetCurrentUserEmail()
        {
            return User.FindFirst(ClaimTypes.Email)?.Value;
        }

        private string? GetCurrentUserType()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value;
        }

        #endregion
    }
}
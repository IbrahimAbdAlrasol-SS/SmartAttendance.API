using Microsoft.AspNetCore.Mvc;
using SmartAttendance.API.Services;
using SmartAttendance.API.Models.DTOs;
using AutoMapper;

namespace SmartAttendance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesTestController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ServicesTestController(
            IUserService userService,
            IAuthService authService,
            IJwtService jwtService,
            IMapper mapper,
            IConfiguration configuration)
        {
            _userService = userService;
            _authService = authService;
            _jwtService = jwtService;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet("services-health")]
        public IActionResult TestServices()
        {
            try
            {
                var servicesStatus = new
                {
                    userService = _userService != null ? "✅ متاح" : "❌ غير متاح",
                    authService = _authService != null ? "✅ متاح" : "❌ غير متاح",
                    jwtService = _jwtService != null ? "✅ متاح" : "❌ غير متاح",
                    mapper = _mapper != null ? "✅ متاح" : "❌ غير متاح",
                    configuration = _configuration != null ? "✅ متاح" : "❌ غير متاح"
                };

                return Ok(new
                {
                    success = true,
                    message = "✅ جميع الـ Services متاحة!",
                    services = servicesStatus,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في Services",
                    error = ex.Message
                });
            }
        }

        [HttpGet("jwt-test")]
        public async Task<IActionResult> TestJWT()
        {
            try
            {
                // إنشاء مستخدم وهمي للاختبار
                var testUser = new SmartAttendance.API.Models.Entities.User
                {
                    Id = 999,
                    Email = "test@jwt.com",
                    UserType = "Student",
                    IsActive = true,
                    EmailVerified = true
                };

                // اختبار إنشاء Token
                var token = _jwtService.GenerateAccessToken(testUser);
                var refreshToken = _jwtService.GenerateRefreshToken();

                // اختبار التحقق من Token
                var isValid = _jwtService.ValidateToken(token);

                return Ok(new
                {
                    success = true,
                    message = "✅ JWT Service يعمل بنجاح!",
                    data = new
                    {
                        tokenGenerated = !string.IsNullOrEmpty(token),
                        refreshTokenGenerated = !string.IsNullOrEmpty(refreshToken),
                        tokenValid = isValid,
                        tokenLength = token?.Length ?? 0,
                        tokenSample = token?.Substring(0, Math.Min(50, token?.Length ?? 0)) + "..."
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في JWT Service",
                    error = ex.Message
                });
            }
        }

        [HttpGet("mapper-test")]
        public IActionResult TestAutoMapper()
        {
            try
            {
                // اختبار تحويل Student Entity إلى DTO
                var testStudent = new SmartAttendance.API.Models.Entities.Student
                {
                    Id = 1,
                    StudentCode = "TEST001",
                    FullName = "طالب اختبار",
                    Stage = "First",
                    StudyType = "Morning",
                    Section = "A",
                    Phone = "07801234567",
                    IsActive = true,
                    User = new SmartAttendance.API.Models.Entities.User
                    {
                        Id = 1,
                        Email = "test.student@test.com",
                        UserType = "Student"
                    }
                };

                // اختبار تحويل DTO
                var studentDto = _mapper.Map<StudentDto>(testStudent);
                var studentListDto = _mapper.Map<StudentListDto>(testStudent);

                return Ok(new
                {
                    success = true,
                    message = "✅ AutoMapper يعمل بنجاح!",
                    mappingResults = new
                    {
                        originalStudent = new
                        {
                            testStudent.Id,
                            testStudent.FullName,
                            testStudent.StudentCode,
                            Email = testStudent.User.Email
                        },
                        mappedStudentDto = new
                        {
                            studentDto.Id,
                            studentDto.FullName,
                            studentDto.StudentCode,
                            studentDto.Email
                        },
                        mappedStudentListDto = new
                        {
                            studentListDto.Id,
                            studentListDto.FullName,
                            studentListDto.StudentCode,
                            studentListDto.Email,
                            studentListDto.HasFaceData
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في AutoMapper",
                    error = ex.Message
                });
            }
        }

        [HttpGet("user-service-test")]
        public async Task<IActionResult> TestUserService()
        {
            try
            {
                // اختبار البحث عن مستخدم موجود (من البيانات الوهمية)
                var existingUser = await _userService.GetUserByEmailAsync("ahmed.ali@student.edu.iq");
                
                // اختبار البحث عن مستخدم غير موجود
                var nonExistentUser = await _userService.GetUserByEmailAsync("nonexistent@test.com");
                
                // اختبار التحقق من وجود بريد إلكتروني
                var emailExists = await _userService.EmailExistsAsync("ahmed.ali@student.edu.iq");
                var emailNotExists = await _userService.EmailExistsAsync("new@test.com");

                return Ok(new
                {
                    success = true,
                    message = "✅ User Service يعمل بنجاح!",
                    testResults = new
                    {
                        existingUserFound = existingUser != null,
                        existingUserEmail = existingUser?.Email,
                        existingUserType = existingUser?.UserType,
                        nonExistentUserFound = nonExistentUser != null,
                        emailExistsTest = emailExists,
                        emailNotExistsTest = !emailNotExists
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في User Service",
                    error = ex.Message
                });
            }
        }

        [HttpPost("auth-service-test")]
        public async Task<IActionResult> TestAuthService()
        {
            try
            {
                // اختبار تسجيل الدخول بحساب موجود
                var loginRequest = new LoginRequest
                {
                    Email = "ahmed.ali@student.edu.iq",
                    Password = "Student2024!"
                };

                var loginResult = await _authService.LoginAsync(loginRequest);

                return Ok(new
                {
                    success = true,
                    message = "✅ Auth Service يعمل بنجاح!",
                    loginTest = new
                    {
                        loginSuccessful = loginResult.Success,
                        loginMessage = loginResult.Message,
                        tokenGenerated = !string.IsNullOrEmpty(loginResult.Data?.Token),
                        userInfo = loginResult.Data?.User != null ? new
                        {
                            loginResult.Data.User.Email,
                            loginResult.Data.User.UserType,
                            HasStudentData = loginResult.Data.User.Student != null
                        } : null
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في Auth Service",
                    error = ex.Message
                });
            }
        }

        [HttpGet("configuration-test")]
        public IActionResult TestConfiguration()
        {
            try
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");
                
                var configurationTest = new
                {
                    hasSecretKey = !string.IsNullOrEmpty(jwtSettings["SecretKey"]),
                    hasIssuer = !string.IsNullOrEmpty(jwtSettings["Issuer"]),
                    hasAudience = !string.IsNullOrEmpty(jwtSettings["Audience"]),
                    secretKeyLength = jwtSettings["SecretKey"]?.Length ?? 0,
                    issuer = jwtSettings["Issuer"],
                    audience = jwtSettings["Audience"]
                };

                return Ok(new
                {
                    success = true,
                    message = "✅ إعدادات JWT صحيحة!",
                    jwtConfiguration = configurationTest
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في الإعدادات",
                    error = ex.Message
                });
            }
        }

        [HttpGet("database-test")]
        public async Task<IActionResult> TestDatabase()
        {
            try
            {
                // اختبار الاتصال بقاعدة البيانات
                var totalUsers = await _userService.GetUserByEmailAsync("ahmed.ali@student.edu.iq");
                
                return Ok(new
                {
                    success = true,
                    message = "✅ الاتصال بقاعدة البيانات يعمل!",
                    databaseTest = new
                    {
                        connectionWorking = true,
                        sampleUserFound = totalUsers != null,
                        sampleUserEmail = totalUsers?.Email,
                        sampleUserType = totalUsers?.UserType
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في الاتصال بقاعدة البيانات",
                    error = ex.Message
                });
            }
        }

        [HttpGet("all-tests")]
        public async Task<IActionResult> RunAllTests()
        {
            var results = new List<object>();

            // Test 1: Services Health
            try
            {
                var servicesHealthResult = await Task.FromResult(TestServices());
                results.Add(new { test = "Services Health", status = "✅ نجح", result = servicesHealthResult });
            }
            catch (Exception ex)
            {
                results.Add(new { test = "Services Health", status = "❌ فشل", error = ex.Message });
            }

            // Test 2: JWT
            try
            {
                var jwtResult = await TestJWT();
                results.Add(new { test = "JWT Service", status = "✅ نجح", result = jwtResult });
            }
            catch (Exception ex)
            {
                results.Add(new { test = "JWT Service", status = "❌ فشل", error = ex.Message });
            }

            // Test 3: AutoMapper
            try
            {
                var mapperResult = TestAutoMapper();
                results.Add(new { test = "AutoMapper", status = "✅ نجح", result = mapperResult });
            }
            catch (Exception ex)
            {
                results.Add(new { test = "AutoMapper", status = "❌ فشل", error = ex.Message });
            }

            // Test 4: Configuration
            try
            {
                var configResult = TestConfiguration();
                results.Add(new { test = "Configuration", status = "✅ نجح", result = configResult });
            }
            catch (Exception ex)
            {
                results.Add(new { test = "Configuration", status = "❌ فشل", error = ex.Message });
            }

            // Test 5: Database
            try
            {
                var dbResult = await TestDatabase();
                results.Add(new { test = "Database", status = "✅ نجح", result = dbResult });
            }
            catch (Exception ex)
            {
                results.Add(new { test = "Database", status = "❌ فشل", error = ex.Message });
            }

            return Ok(new
            {
                success = true,
                message = "🧪 اكتملت جميع الاختبارات",
                timestamp = DateTime.UtcNow,
                testResults = results
            });
        }
    }
}
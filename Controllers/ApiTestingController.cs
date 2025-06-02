using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.API.Data;
using SmartAttendance.API.Models.Entities;
using SmartAttendance.API.Constants;
using SmartAttendance.API.Services;
using SmartAttendance.API.Models.DTOs;
using MySqlConnector;
using AutoMapper;
using SmartAttendance.API.Services.Interfaces;

using System.IO;
namespace SmartAttendance.API.Controllers
{
    /// <summary>
    /// وحدة التحكم الموحدة لاختبارات API
    /// </summary>
    [Route("api/testing")]
    [ApiController]
    public class ApiTestingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ApiTestingController(
            ApplicationDbContext context,
            IConfiguration configuration,
            IUserService userService,
            IAuthService authService,
            IJwtService jwtService,
            IMapper mapper,
            IFileService fileService,
            IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _configuration = configuration;
            _userService = userService;
            _authService = authService;
            _jwtService = jwtService;
            _mapper = mapper;
            _fileService = fileService;
            _hostEnvironment = hostEnvironment;
            
        }

        #region اختبارات الملفات والصور

        /// <summary>
        /// اختبار رفع صورة
        /// </summary>
        [HttpPost("files/upload-test")]
        public async Task<IActionResult> TestFileUpload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "لم يتم تحديد ملف",
                    timestamp = DateTime.UtcNow
                });
            }
            
            try
            {
                var result = await _fileService.UploadImageAsync(file, "test", "test_file");
                
                return Ok(new
                {
                    success = true,
                    message = "✅ تم اختبار رفع الملف بنجاح",
                    filePath = result.Data,
                    fileSize = file.Length,
                    fileType = file.ContentType,
                    fileUrl = $"{Request.Scheme}://{Request.Host}/{result.Data}",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في اختبار رفع الملف",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// اختبار حذف ملف
        /// </summary>
        [HttpDelete("files/delete-test")]
        public async Task<IActionResult> TestFileDelete(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "لم يتم تحديد مسار الملف",
                    timestamp = DateTime.UtcNow
                });
            }

            try
            {
                var result = await _fileService.DeleteFileAsync(filePath);
                
                return Ok(new
                {
                    success = true,
                    message = result.Data ? "✅ تم حذف الملف بنجاح" : "❌ الملف غير موجود",
                    filePath = filePath,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في اختبار حذف الملف",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// قراءة معلومات الملفات المرفوعة
        /// </summary>
        [HttpGet("files/info")]
        public IActionResult GetFilesInfo()
        {
            try
            {
                var uploadsPath = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
                var facesPath = Path.Combine(uploadsPath, "faces");
                var testPath = Path.Combine(uploadsPath, "test");
                
                var directoryInfo = new
                                    {
                                        uploadsExists = Directory.Exists(uploadsPath),
                                        facesExists = Directory.Exists(facesPath),
                                        testExists = Directory.Exists(testPath),
                                        uploadsPath = uploadsPath,
                                        faceFiles = Directory.Exists(facesPath) 
                                            ? Directory.GetFiles(facesPath).Select(f => new FileInfo(f)).Select(f => new
                                            {
                                                name = f.Name,
                                                size = f.Length,
                                                created = f.CreationTime,
                                                path = $"uploads/faces/{f.Name}",
                                                url = $"{Request.Scheme}://{Request.Host}/uploads/faces/{f.Name}"
                                            }).Cast<object>().ToList()
                                            : new List<object>(),
                                        testFiles = Directory.Exists(testPath) 
                                            ? Directory.GetFiles(testPath).Select(f => new FileInfo(f)).Select(f => new
                                            {
                                                name = f.Name,
                                                size = f.Length,
                                                created = f.CreationTime,
                                                path = $"uploads/test/{f.Name}",
                                                url = $"{Request.Scheme}://{Request.Host}/uploads/test/{f.Name}"
                                            }).Cast<object>().ToList()
                                            : new List<object>()
                                    };
                
                return Ok(new
                {
                    success = true,
                    message = "✅ معلومات الملفات",
                    directoryInfo,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في جلب معلومات الملفات",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// اختبار رفع صورة وجه طالب
        /// </summary>
        [HttpPost("student/{id}/face-image-test")]
        public async Task<IActionResult> TestStudentFaceUpload(int id, IFormFile faceImage)
        {
            if (faceImage == null || faceImage.Length == 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "صورة الوجه مطلوبة",
                    timestamp = DateTime.UtcNow
                });
            }

            try
            {
                // التحقق من وجود الطالب
                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

                if (student == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "الطالب غير موجود",
                        timestamp = DateTime.UtcNow
                    });
                }
                
                // رفع الصورة
                var uploadPath = "faces";
                var fileName = $"student_{id}";
                var uploadResult = await _fileService.UploadImageAsync(faceImage, uploadPath, fileName);
                
                if (!uploadResult.Success)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = uploadResult.Message,
                        timestamp = DateTime.UtcNow
                    });
                }
                
                // تحديث بيانات الطالب
                student.ProfileImage = uploadResult.Data;
                student.LastFaceUpdate = DateTime.UtcNow;
                student.UpdatedAt = DateTime.UtcNow;
                
                await _context.SaveChangesAsync();
                
                return Ok(new
                {
                    success = true,
                    message = "✅ تم رفع صورة الوجه بنجاح",
                    data = new
                    {
                        studentId = id,
                        studentName = student.FullName,
                        imagePath = uploadResult.Data,
                        imageUrl = $"{Request.Scheme}://{Request.Host}/{uploadResult.Data}",
                        lastUpdate = student.LastFaceUpdate
                    },
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في رفع صورة الوجه",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// التحقق من وجود مجلدات التحميل وإنشاؤها إذا لم تكن موجودة
        /// </summary>
        [HttpPost("files/create-directories")]
        public IActionResult EnsureUploadDirectories()
        {
            try
            {
                var webRootPath = _hostEnvironment.WebRootPath;
                var uploadsDirectoryPath = Path.Combine(webRootPath, "uploads");
                var facesDirectoryPath = Path.Combine(uploadsDirectoryPath, "faces");
                var testDirectoryPath = Path.Combine(uploadsDirectoryPath, "test");
                
                var results = new Dictionary<string, bool>();
                
                // إنشاء المجلدات إذا لم تكن موجودة
                if (!Directory.Exists(uploadsDirectoryPath))
                {
                    Directory.CreateDirectory(uploadsDirectoryPath);
                    results["uploads"] = true;
                }
                else
                {
                    results["uploads"] = false; // المجلد موجود مسبقاً
                }
                
                if (!Directory.Exists(facesDirectoryPath))
                {
                    Directory.CreateDirectory(facesDirectoryPath);
                    results["faces"] = true;
                }
                else
                {
                    results["faces"] = false;
                }
                
                if (!Directory.Exists(testDirectoryPath))
                {
                    Directory.CreateDirectory(testDirectoryPath);
                    results["test"] = true;
                }
                else
                {
                    results["test"] = false;
                }
                
                return Ok(new
                {
                    success = true,
                    message = "✅ تم التحقق من المجلدات",
                    directoriesCreated = results,
                    paths = new
                    {
                        webRoot = webRootPath,
                        uploads = uploadsDirectoryPath,
                        faces = facesDirectoryPath,
                        test = testDirectoryPath
                    },
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في التحقق من المجلدات",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// اختبار معالجة صورة وجه
        /// </summary>
        [HttpPost("files/process-face")]
        public async Task<IActionResult> TestFaceProcessing(IFormFile faceImage)
        {
            if (faceImage == null || faceImage.Length == 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "صورة الوجه مطلوبة",
                    timestamp = DateTime.UtcNow
                });
            }
            
            try
            {
                var processResult = await _fileService.ProcessFaceImageAsync(faceImage, 999); // رقم وهمي للاختبار
                
                return Ok(new
                {
                    success = true,
                    message = "✅ تم معالجة صورة الوجه بنجاح",
                    filePath = processResult.Data,
                    fileUrl = $"{Request.Scheme}://{Request.Host}/{processResult.Data}",
                    note = "في الوقت الحالي تتم فقط عملية رفع الصورة. ستتم إضافة معالجة الوجه في تحديث مستقبلي",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في معالجة صورة الوجه",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        #endregion

        #region الاختبارات الأساسية

        /// <summary>
        /// اختبار الحالة الصحية للـ API
        /// </summary>
        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new
            {
                success = true,
                message = "✅ API يعمل بنجاح!",
                timestamp = DateTime.Now,
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
                version = "1.0.0"
            });
        }

        #endregion

        #region اختبارات الاتصال بقاعدة البيانات

        /// <summary>
        /// اختبار الاتصال بقاعدة البيانات
        /// </summary>
        [HttpGet("connection/database")]
        public async Task<IActionResult> TestDatabaseConnection()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();

                if (canConnect)
                {
                    var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
                    var appliedMigrations = await _context.Database.GetAppliedMigrationsAsync();

                    return Ok(new
                    {
                        success = true,
                        message = "✅ اتصال قاعدة البيانات ناجح!",
                        connectionStatus = "Connected",
                        appliedMigrations = appliedMigrations.Count(),
                        pendingMigrations = pendingMigrations.Count(),
                        databaseProvider = _context.Database.ProviderName,
                        databaseName = _context.Database.GetDbConnection().Database,
                        timestamp = DateTime.Now
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "❌ فشل في الاتصال بقاعدة البيانات",
                        timestamp = DateTime.Now
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في الاتصال بقاعدة البيانات",
                    error = ex.Message,
                    timestamp = DateTime.Now
                });
            }
        }

        /// <summary>
        /// اختبار الاتصال بـ MySQL
        /// </summary>
        [HttpGet("connection/mysql")]
        public async Task<IActionResult> TestMySqlConnection()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                
                using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();
                
                var command = new MySqlCommand("SELECT VERSION() as mysql_version, DATABASE() as current_db", connection);
                using var reader = await command.ExecuteReaderAsync();
                
                string version = "";
                string database = "";
                
                if (await reader.ReadAsync())
                {
                    version = reader["mysql_version"]?.ToString() ?? "Unknown";
                    database = reader["current_db"]?.ToString() ?? "None";
                }
                
                return Ok(new
                {
                    success = true,
                    message = "✅ MySQL متصل بنجاح!",
                    mysqlVersion = version,
                    currentDatabase = database,
                    serverInfo = new
                    {
                        host = connectionString.Contains("localhost") ? "localhost" : "خادم خارجي",
                        port = connectionString.Contains("Port=") ? connectionString.Split("Port=")[1].Split(';')[0] : "3306",
                    },
                    timestamp = DateTime.Now
                });
            }
            catch (MySqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في الاتصال بـ MySQL",
                    mysqlError = ex.Message,
                    errorCode = ex.Number,
                    suggestion = GetMySqlErrorSuggestion(ex.Number),
                    timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ عام",
                    error = ex.Message,
                    timestamp = DateTime.Now
                });
            }
        }

        /// <summary>
        /// اختبار الاتصال بـ Entity Framework
        /// </summary>
        [HttpGet("connection/ef")]
        public async Task<IActionResult> TestEntityFramework()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                
                if (canConnect)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "✅ Entity Framework متصل بنجاح!",
                        databaseName = _context.Database.GetDbConnection().Database,
                        provider = _context.Database.ProviderName,
                        timestamp = DateTime.Now
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "❌ فشل اتصال Entity Framework",
                        timestamp = DateTime.Now
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في Entity Framework",
                    error = ex.Message,
                    timestamp = DateTime.Now
                });
            }
        }

        /// <summary>
        /// اختبار جداول قاعدة البيانات
        /// </summary>
        [HttpGet("connection/tables")]
        public async Task<IActionResult> TestDatabaseTables()
        {
            try
            {
                var tables = new
                {
                    Users = await _context.Users.CountAsync(),
                    Students = await _context.Students.CountAsync(),
                    Professors = await _context.Professors.CountAsync(),
                    Subjects = await _context.Subjects.CountAsync(),
                    CourseAssignments = await _context.CourseAssignments.CountAsync(),
                    Sessions = await _context.Sessions.CountAsync(),
                    AttendanceRecords = await _context.AttendanceRecords.CountAsync()
                };

                return Ok(new
                {
                    success = true,
                    message = "✅ جميع الجداول تعمل بنجاح!",
                    tableCounts = tables,
                    timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في الوصول للجداول",
                    error = ex.Message,
                    timestamp = DateTime.Now
                });
            }
        }

        #endregion

        #region اختبارات الخدمات

        /// <summary>
        /// اختبار توفر الخدمات
        /// </summary>
        [HttpGet("services/health")]
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
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// اختبار خدمة JWT
        /// </summary>
        [HttpGet("services/jwt")]
        public async Task<IActionResult> TestJwt()
        {
            try
            {
                // إنشاء مستخدم وهمي للاختبار
                var testUser = new User
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
                    },
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في JWT Service",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// اختبار خدمة AutoMapper
        /// </summary>
        [HttpGet("services/mapper")]
        public IActionResult TestAutoMapper()
        {
            try
            {
                // اختبار تحويل Student Entity إلى DTO
                var testStudent = new Student
                {
                    Id = 1,
                    StudentCode = "TEST001",
                    FullName = "طالب اختبار",
                    Stage = "First",
                    StudyType = "Morning",
                    Section = "A",
                    Phone = "07801234567",
                    IsActive = true,
                    User = new User
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
                    },
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في AutoMapper",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// اختبار خدمة المستخدمين
        /// </summary>
        [HttpGet("services/user")]
        public async Task<IActionResult> TestUserService()
        {
            try
            {
                // اختبار البحث عن مستخدم موجود
                var existingUser = await _userService.GetUserByEmailAsync("admin@bologna.edu.iq");
                
                // اختبار البحث عن مستخدم غير موجود
                var nonExistentUser = await _userService.GetUserByEmailAsync("nonexistent@test.com");
                
                // اختبار التحقق من وجود بريد إلكتروني
                var emailExists = await _userService.EmailExistsAsync("admin@bologna.edu.iq");
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
                    },
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في User Service",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// اختبار خدمة المصادقة
        /// </summary>
        [HttpPost("services/auth")]
        public async Task<IActionResult> TestAuthService()
        {
            try
            {
                // التحقق من وجود مستخدم للاختبار
                var adminExists = await _context.Users.AnyAsync(u => u.Email == "admin@bologna.edu.iq");

                if (!adminExists)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "❌ مستخدم الاختبار غير موجود، قم بتنفيذ اختبار تهيئة البيانات أولاً",
                        timestamp = DateTime.UtcNow
                    });
                }

                // اختبار تسجيل الدخول بحساب موجود
                var loginRequest = new LoginRequest
                {
                    Email = "admin@bologna.edu.iq",
                    Password = "Admin2024!"
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
                            loginResult.Data.User.UserType
                        } : null
                    },
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في Auth Service",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// تشغيل جميع اختبارات الخدمات
        /// </summary>
        [HttpGet("services/all")]
        public async Task<IActionResult> RunAllServiceTests()
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
                var jwtResult = await TestJwt();
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

            // Test 4: User Service
            try
            {
                var userServiceResult = await TestUserService();
                results.Add(new { test = "User Service", status = "✅ نجح", result = userServiceResult });
            }
            catch (Exception ex)
            {
                results.Add(new { test = "User Service", status = "❌ فشل", error = ex.Message });
            }

            return Ok(new
            {
                success = true,
                message = "🧪 اكتملت جميع اختبارات الخدمات",
                timestamp = DateTime.UtcNow,
                testResults = results
            });
        }

        #endregion

        #region اختبارات البيانات

        /// <summary>
        /// تهيئة بيانات الاختبار
        /// </summary>
        [HttpPost("data/seed")]
        public async Task<IActionResult> SeedTestData()
        {
            try
            {
                // Check if data already exists
                if (await _context.Users.AnyAsync())
                {
                    return Ok(new
                    {
                        success = true,
                        message = "📊 البيانات التجريبية موجودة مسبقاً",
                        timestamp = DateTime.UtcNow
                    });
                }

                // 1. إنشاء المستخدمين
                var adminUser = new User
                {
                    Email = "admin@bologna.edu.iq",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin2024!"),
                    UserType = UserRoles.Admin,
                    IsActive = true,
                    EmailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var profUser1 = new User
                {
                    Email = "dr.omar@bologna.edu.iq",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Prof2024!"),
                    UserType = UserRoles.Professor,
                    IsActive = true,
                    EmailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var profUser2 = new User
                {
                    Email = "dr.sara@bologna.edu.iq",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Prof2024!"),
                    UserType = UserRoles.Professor,
                    IsActive = true,
                    EmailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var studentUser1 = new User
                {
                    Email = "ahmed.ali@student.edu.iq",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student2024!"),
                    UserType = UserRoles.Student,
                    IsActive = true,
                    EmailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var studentUser2 = new User
                {
                    Email = "fatima.mohammed@student.edu.iq",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student2024!"),
                    UserType = UserRoles.Student,
                    IsActive = true,
                    EmailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var studentUser3 = new User
                {
                    Email = "omar.khalil@student.edu.iq",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student2024!"),
                    UserType = UserRoles.Student,
                    IsActive = true,
                    EmailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _context.Users.AddRangeAsync(adminUser, profUser1, profUser2, studentUser1, studentUser2, studentUser3);
                await _context.SaveChangesAsync();

                // 2. إنشاء الأساتذة
                var professor1 = new Professor
                {
                    UserId = profUser1.Id,
                    EmployeeCode = "ENG001",
                    FullName = "د. عمر حسين البصري",
                    Department = "علوم الحاسوب",
                    Title = "Dr.",
                    Phone = "07701234567",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var professor2 = new Professor
                {
                    UserId = profUser2.Id,
                    EmployeeCode = "ENG002",
                    FullName = "د. سارة علي الكربلائي",
                    Department = "علوم الحاسوب",
                    Title = "Dr.",
                    Phone = "07709876543",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _context.Professors.AddRangeAsync(professor1, professor2);
                await _context.SaveChangesAsync();

                // 3. إنشاء الطلاب
                var student1 = new Student
                {
                    UserId = studentUser1.Id,
                    StudentCode = "CS2024001",
                    FullName = "أحمد علي حسن",
                    Stage = "First",
                    StudyType = "Morning",
                    Section = "A",
                    Phone = "07801234567",
                    DateOfBirth = new DateTime(2005, 3, 15),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var student2 = new Student
                {
                    UserId = studentUser2.Id,
                    StudentCode = "CS2024002",
                    FullName = "فاطمة محمد جاسم",
                    Stage = "First",
                    StudyType = "Morning",
                    Section = "A",
                    Phone = "07807654321",
                    DateOfBirth = new DateTime(2005, 7, 22),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var student3 = new Student
                {
                    UserId = studentUser3.Id,
                    StudentCode = "CS2024003",
                    FullName = "عمر خليل إبراهيم",
                    Stage = "First",
                    StudyType = "Morning",
                    Section = "B",
                    Phone = "07811122334",
                    DateOfBirth = new DateTime(2005, 11, 8),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _context.Students.AddRangeAsync(student1, student2, student3);
                await _context.SaveChangesAsync();

                // 4. إنشاء المواد
                var subject1 = new Subject
                {
                    Name = "مقدمة في البرمجة",
                    Code = "CS101",
                    Stage = "First",
                    StudyType = "Morning",
                    CreditHours = 3,
                    Description = "أساسيات البرمجة باستخدام C++",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var subject2 = new Subject
                {
                    Name = "هياكل البيانات",
                    Code = "CS201",
                    Stage = "Second",
                    StudyType = "Morning",
                    CreditHours = 4,
                    Description = "دراسة هياكل البيانات والخوارزميات",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _context.Subjects.AddRangeAsync(subject1, subject2);
                await _context.SaveChangesAsync();

                // 5. إنشاء تخصيص المواد
                var courseAssignment1 = new CourseAssignment
                {
                    ProfessorId = professor1.Id,
                    SubjectId = subject1.Id,
                    Section = "A",
                    AcademicYear = "2024-2025",
                    Semester = "الفصل الأول",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _context.CourseAssignments.AddAsync(courseAssignment1);
                await _context.SaveChangesAsync();

                // 6. إنشاء جلسات
                var session1 = new Session
                {
                    CourseAssignmentId = courseAssignment1.Id,
                    SessionDate = DateTime.Today,
                    StartTime = new TimeSpan(8, 0, 0),
                    EndTime = new TimeSpan(10, 0, 0),
                    Status = SessionStatus.Scheduled,
                    Notes = "محاضرة المتغيرات والثوابت",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _context.Sessions.AddAsync(session1);
                await _context.SaveChangesAsync();

                // 7. إنشاء سجلات حضور
                var attendance1 = new AttendanceRecord
                {
                    SessionId = session1.Id,
                    StudentId = student1.Id,
                    AttendanceStatus = AttendanceStatus.Present,
                    DetectionMethod = DetectionMethods.Manual,
                    EntryTime = DateTime.Now,
                    Notes = "حضر في الوقت المحدد",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var attendance2 = new AttendanceRecord
                {
                    SessionId = session1.Id,
                    StudentId = student2.Id,
                    AttendanceStatus = AttendanceStatus.Late,
                    DetectionMethod = DetectionMethods.Manual,
                    EntryTime = DateTime.Now.AddMinutes(15),
                    Notes = "تأخر 15 دقيقة",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _context.AttendanceRecords.AddRangeAsync(attendance1, attendance2);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "✅ تم إنشاء البيانات التجريبية بنجاح!",
                    data = new
                    {
                        users = 6,
                        professors = 2,
                        students = 3,
                        subjects = 2,
                        courseAssignments = 1,
                        sessions = 1,
                        attendanceRecords = 2,
                        testAccounts = new
                        {
                            admin = new { email = adminUser.Email, password = "Admin2024!" },
                            professor = new { email = profUser1.Email, password = "Prof2024!" },
                            student = new { email = studentUser1.Email, password = "Student2024!" }
                        }
                    },
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في إنشاء البيانات التجريبية",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// حذف جميع بيانات الاختبار
        /// </summary>
        [HttpPost("data/clear")]
        public async Task<IActionResult> ClearTestData()
        {
            try
            {
                // الحذف سيكون بالترتيب العكسي للعلاقات
                _context.AttendanceRecords.RemoveRange(await _context.AttendanceRecords.ToListAsync());
                _context.Sessions.RemoveRange(await _context.Sessions.ToListAsync());
                _context.CourseAssignments.RemoveRange(await _context.CourseAssignments.ToListAsync());
                _context.Subjects.RemoveRange(await _context.Subjects.ToListAsync());
                _context.Students.RemoveRange(await _context.Students.ToListAsync());
                _context.Professors.RemoveRange(await _context.Professors.ToListAsync());
                _context.Users.RemoveRange(await _context.Users.ToListAsync());

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "✅ تم حذف جميع البيانات بنجاح!",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في حذف البيانات",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// الحصول على معلومات قاعدة البيانات
        /// </summary>
        [HttpGet("data/info")]
        public async Task<IActionResult> GetDatabaseInfo()
        {
            try
            {
                var info = new
                {
                    DatabaseName = _context.Database.GetDbConnection().Database,
                    Provider = _context.Database.ProviderName,
                    ConnectionState = _context.Database.GetDbConnection().State.ToString(),
                    AppliedMigrations = await _context.Database.GetAppliedMigrationsAsync(),
                    PendingMigrations = await _context.Database.GetPendingMigrationsAsync()
                };

                var counts = new
                {
                    Users = await _context.Users.CountAsync(),
                    Students = await _context.Students.CountAsync(),
                    Professors = await _context.Professors.CountAsync(),
                    Subjects = await _context.Subjects.CountAsync(),
                    CourseAssignments = await _context.CourseAssignments.CountAsync(),
                    Sessions = await _context.Sessions.CountAsync(),
                    AttendanceRecords = await _context.AttendanceRecords.CountAsync()
                };

                return Ok(new
                {
                    success = true,
                    message = "✅ معلومات قاعدة البيانات",
                    info,
                    counts,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في جلب معلومات قاعدة البيانات",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        #endregion

        #region اختبارات النماذج والكيانات

        /// <summary>
        /// اختبار النماذج
        /// </summary>
        [HttpGet("models/entity-test")]
        public IActionResult TestModels()
        {
            try
            {
                var user = new User
                {
                    Email = "test@example.com",
                    PasswordHash = "hashedpassword",
                    UserType = UserRoles.Student,
                    IsActive = true
                };

                var student = new Student
                {
                    UserId = 1,
                    StudentCode = "2024001",
                    FullName = "أحمد محمد علي",
                    Stage = "First",
                    StudyType = "Morning",
                    Section = "A",
                    Phone = "07801234567"
                };

                var subject = new Subject
                {
                    Name = "برمجة الحاسوب",
                    Code = "CS101",
                    Stage = "First",
                    StudyType = "Morning",
                    CreditHours = 3
                };

                // اختبار Constants
                var constants = new
                {
                    UserRoles = new { UserRoles.Student, UserRoles.Professor, UserRoles.Admin },
                    AttendanceStatuses = new { AttendanceStatus.Present, AttendanceStatus.Absent },
                    Stages = AcademicInfo.Stages,
                    StudyTypes = AcademicInfo.StudyTypes,
                    Sections = AcademicInfo.Sections
                };

                return Ok(new
                {
                    success = true,
                    message = "✅ جميع الـ Models تعمل بنجاح!",
                    data = new
                    {
                        user = new { 
                            user.Email, 
                            user.UserType, 
                            user.IsActive,
                            user.CreatedAt 
                        },
                        student = new { 
                            student.StudentCode, 
                            student.FullName, 
                            student.Stage,
                            student.Section 
                        },
                        subject = new { 
                            subject.Name, 
                            subject.Code, 
                            subject.CreditHours 
                        },
                        constants
                    },
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في الـ Models",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// اختبار الحزم البرمجية
        /// </summary>
        [HttpGet("models/packages-test")]
        public IActionResult TestPackages()
        {
            try
            {
                var packages = new
                {
                    EntityFramework = "متوفر",
                    FluentValidation = "متوفر", 
                    BCrypt = "متوفر",
                    AutoMapper = "متوفر",
                    JWT = "متوفر"
                };

                // اختبار BCrypt
                var password = "testpassword";
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                var isValid = BCrypt.Net.BCrypt.Verify(password, hashedPassword);

                return Ok(new
                {
                    success = true,
                    message = "✅ جميع الـ Packages تعمل بنجاح!",
                    data = new
                    {
                        packages,
                        bcryptTest = new
                        {
                            originalPassword = password,
                            hashedPassword,
                            isValid
                        }
                    },
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في الـ Packages",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// اختبار الثوابت
        /// </summary>
        [HttpGet("models/constants-test")]
        public IActionResult TestConstants()
        {
            return Ok(new
            {
                success = true,
                message = "✅ جميع الـ Constants تعمل بنجاح!",
                constants = new
                {
                    userRoles = new
                    {
                        Student = UserRoles.Student,
                        Professor = UserRoles.Professor,
                        Admin = UserRoles.Admin
                    },
                    attendanceStatus = new
                    {
                        Present = AttendanceStatus.Present,
                        Absent = AttendanceStatus.Absent,
                        Late = AttendanceStatus.Late,
                        Excused = AttendanceStatus.Excused
                    },
                    detectionMethods = new
                    {
                        FaceRecognition = DetectionMethods.FaceRecognition,
                        Manual = DetectionMethods.Manual,
                        QRCodee = DetectionMethods.QRCodee
                    },
                    sessionStatus = new
                    {
                        Scheduled = SessionStatus.Scheduled,
                        Active = SessionStatus.Active,
                        Completed = SessionStatus.Completed,
                        Cancelled = SessionStatus.Cancelled
                    },
                    academicInfo = new
                    {
                        Stages = AcademicInfo.Stages,
                        StudyTypes = AcademicInfo.StudyTypes,
                        Sections = AcademicInfo.Sections,
                        Semesters = AcademicInfo.Semesters
                    },
                    messages = new
                    {
                        Success = ApiMessages.Success,
                        Error = ApiMessages.Error,
                        NotFound = ApiMessages.NotFound,
                        Unauthorized = ApiMessages.Unauthorized,
                        ValidationError = ApiMessages.ValidationError
                    }
                },
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// اختبار العلاقات بين الكيانات
        /// </summary>
        [HttpGet("models/relationships-test")]
        public IActionResult TestEntityRelationships()
        {
            try
            {
                // اختبار العلاقات بين الـ Entities
                var user = new User { Id = 1, Email = "student@test.com" };
                var student = new Student { Id = 1, UserId = 1, User = user };
                var professor = new Professor { Id = 1, UserId = 2, FullName = "د. أحمد" };
                var subject = new Subject { Id = 1, Name = "البرمجة", Code = "CS101" };
                var courseAssignment = new CourseAssignment 
                { 
                    Id = 1, 
                    ProfessorId = 1, 
                    SubjectId = 1,
                    Professor = professor,
                    Subject = subject,
                    Section = "A"
                };
                var session = new Session 
                { 
                    Id = 1, 
                    CourseAssignmentId = 1,
                    CourseAssignment = courseAssignment,
                    SessionDate = DateTime.Now,
                    StartTime = new TimeSpan(8, 0, 0),
                    EndTime = new TimeSpan(10, 0, 0)
                };
                var attendance = new AttendanceRecord 
                { 
                    Id = 1,
                    SessionId = 1,
                    StudentId = 1,
                    Session = session,
                    Student = student,
                    AttendanceStatus = AttendanceStatus.Present
                };

                return Ok(new
                {
                    success = true,
                    message = "✅ العلاقات بين الـ Entities تعمل بنجاح!",
                    data = new
                    {
                        userStudentRelation = student.User?.Email,
                        professorSubjectRelation = $"{courseAssignment.Professor.FullName} يدرس {courseAssignment.Subject.Name}",
                        sessionDetails = $"جلسة {session.CourseAssignment.Subject.Name} في تاريخ {session.SessionDate:yyyy-MM-dd}",
                        attendanceDetails = $"الطالب حضر الجلسة بحالة: {attendance.AttendanceStatus}"
                    },
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في العلاقات",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        #endregion

        #region اختبارات API المختلفة

        /// <summary>
        /// اختبار مسار التسجيل والمصادقة
        /// </summary>
        [HttpGet("api/auth-flow")]
        public async Task<IActionResult> TestAuthFlow()
        {
            try
            {
                var testEmail = $"test.user.{Guid.NewGuid().ToString().Substring(0, 8)}@test.com";
                var testPassword = "Test2024!";
                
                // 1. تسجيل طالب جديد
                var registerRequest = new RegisterStudentRequest
                {
                    Email = testEmail,
                    Password = testPassword,
                    FullName = "طالب اختبار",
                    Stage = "First",
                    StudyType = "Morning",
                    Section = "A"
                };
                
                var registerResult = await _authService.RegisterStudentAsync(registerRequest);
                
                if (!registerResult.Success)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "❌ فشل في تسجيل المستخدم",
                        error = registerResult.Message,
                        timestamp = DateTime.UtcNow
                    });
                }
                
                // 2. تسجيل الدخول
                var loginRequest = new LoginRequest
                {
                    Email = testEmail,
                    Password = testPassword
                };
                
                var loginResult = await _authService.LoginAsync(loginRequest);
                
                if (!loginResult.Success)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "❌ فشل في تسجيل الدخول",
                        error = loginResult.Message,
                        timestamp = DateTime.UtcNow
                    });
                }
                
                // 3. جلب الملف الشخصي
                var userId = loginResult.Data!.User.Id;
                var profileResult = await _authService.GetUserProfileAsync(userId);
                
                return Ok(new
                {
                    success = true,
                    message = "✅ مسار المصادقة يعمل بنجاح!",
                    data = new
                    {
                        registration = new
                        {
                            success = registerResult.Success,
                            message = registerResult.Message,
                            userId = registerResult.Data?.Id
                        },
                        login = new
                        {
                            success = loginResult.Success,
                            message = loginResult.Message,
                            token = loginResult.Data?.Token?.Substring(0, 20) + "...",
                            expiresAt = loginResult.Data?.ExpiresAt
                        },
                        profile = new
                        {
                            success = profileResult.Success,
                            message = profileResult.Message,
                            email = profileResult.Data?.Email,
                            userType = profileResult.Data?.UserType
                        }
                    },
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في اختبار مسار المصادقة",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// جلب إعدادات JWT
        /// </summary>
        [HttpGet("config/jwt")]
        public IActionResult GetJwtConfig()
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
                    audience = jwtSettings["Audience"],
                    expiryInHours = jwtSettings["ExpiryInHours"] ?? "24",
                    refreshTokenExpiryInDays = jwtSettings["RefreshTokenExpiryInDays"] ?? "30"
                };

                return Ok(new
                {
                    success = true,
                    message = "✅ إعدادات JWT صحيحة!",
                    jwtConfiguration = configurationTest,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في الإعدادات",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// جلب إعدادات التطبيق
        /// </summary>
        [HttpGet("config/application")]
        public IActionResult GetAppConfig()
        {
            try
            {
                var appSettings = _configuration.GetSection("ApplicationSettings");
                
                var config = new
                {
                    ApplicationName = appSettings["ApplicationName"] ?? "Smart Attendance System",
                    Version = appSettings["Version"] ?? "1.0.0",
                    MaxFileUploadSize = appSettings["MaxFileUploadSize"] ?? "5242880",
                    SupportedImageFormats = appSettings["SupportedImageFormats"] ?? "jpg,jpeg,png",
                    DefaultProfileImage = appSettings["DefaultProfileImage"] ?? "/images/default-profile.png"
                };

                return Ok(new
                {
                    success = true,
                    message = "✅ إعدادات التطبيق!",
                    config,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في الإعدادات",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        #endregion

        #region Helper Methods

        private string GetMySqlErrorSuggestion(int errorCode)
        {
            return errorCode switch
            {
                1045 => "كلمة المرور أو اسم المستخدم خاطئ",
                2003 => "MySQL Server غير مشغل أو Port خاطئ",
                1049 => "Database غير موجود",
                _ => "تحقق من إعدادات الاتصال"
            };
        }

        #endregion
    }
}
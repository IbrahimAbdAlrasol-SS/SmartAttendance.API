using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.API.Data;
using SmartAttendance.API.Models.Entities;
using SmartAttendance.API.Constants;

namespace SmartAttendance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseTestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DatabaseTestController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("connection-test")]
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
                        databaseProvider = _context.Database.ProviderName
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "❌ فشل في الاتصال بقاعدة البيانات"
                    });
                }
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

        [HttpGet("tables-test")]
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
                    tableCounts = tables
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في الوصول للجداول",
                    error = ex.Message
                });
            }
        }

        [HttpPost("seed-test-data")]
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
                        message = "📊 البيانات التجريبية موجودة مسبقاً"
                    });
                }

                // Create test admin user
                var adminUser = new User
                {
                    Email = "admin@smartattendance.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    UserType = UserRoles.Admin,
                    IsActive = true,
                    EmailVerified = true
                };

                // Create test student user
                var studentUser = new User
                {
                    Email = "student@test.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student123!"),
                    UserType = UserRoles.Student,
                    IsActive = true,
                    EmailVerified = true
                };

                // Create test professor user
                var professorUser = new User
                {
                    Email = "professor@test.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Professor123!"),
                    UserType = UserRoles.Professor,
                    IsActive = true,
                    EmailVerified = true
                };

                await _context.Users.AddRangeAsync(adminUser, studentUser, professorUser);
                await _context.SaveChangesAsync();

                // Create test student
                var student = new Student
                {
                    UserId = studentUser.Id,
                    StudentCode = "2024001",
                    FullName = "أحمد محمد علي",
                    Stage = "First",
                    StudyType = "Morning",
                    Section = "A",
                    Phone = "07801234567",
                    IsActive = true
                };

                // Create test professor
                var professor = new Professor
                {
                    UserId = professorUser.Id,
                    EmployeeCode = "EMP001",
                    FullName = "د. سارة أحمد",
                    Department = "علوم الحاسوب",
                    Title = "Dr.",
                    Phone = "07709876543",
                    IsActive = true
                };

                // Create test subject
                var subject = new Subject
                {
                    Name = "برمجة الحاسوب",
                    Code = "CS101",
                    Stage = "First",
                    StudyType = "Morning",
                    CreditHours = 3,
                    Description = "مقدمة في برمجة الحاسوب",
                    IsActive = true
                };

                await _context.Students.AddAsync(student);
                await _context.Professors.AddAsync(professor);
                await _context.Subjects.AddAsync(subject);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "✅ تم إنشاء البيانات التجريبية بنجاح!",
                    data = new
                    {
                        adminEmail = adminUser.Email,
                        studentEmail = studentUser.Email,
                        professorEmail = professorUser.Email,
                        studentCode = student.StudentCode,
                        subjectCode = subject.Code
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في إنشاء البيانات التجريبية",
                    error = ex.Message
                });
            }
        }

        [HttpGet("database-info")]
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

                return Ok(new
                {
                    success = true,
                    message = "✅ معلومات قاعدة البيانات",
                    info
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في جلب معلومات قاعدة البيانات",
                    error = ex.Message
                });
            }
        }
    }
}
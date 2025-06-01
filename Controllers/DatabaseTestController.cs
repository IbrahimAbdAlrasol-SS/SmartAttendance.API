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

                // 1. إنشاء المستخدمين
                var adminUser = new User
                {
                    Email = "admin@engineering.edu.iq",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin2024!"),
                    UserType = UserRoles.Admin,
                    IsActive = true,
                    EmailVerified = true
                };

                var profUser1 = new User
                {
                    Email = "dr.omar@engineering.edu.iq",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Prof2024!"),
                    UserType = UserRoles.Professor,
                    IsActive = true,
                    EmailVerified = true
                };

                var profUser2 = new User
                {
                    Email = "dr.sara@engineering.edu.iq",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Prof2024!"),
                    UserType = UserRoles.Professor,
                    IsActive = true,
                    EmailVerified = true
                };

                var studentUser1 = new User
                {
                    Email = "ahmed.ali@student.edu.iq",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student2024!"),
                    UserType = UserRoles.Student,
                    IsActive = true,
                    EmailVerified = true
                };

                var studentUser2 = new User
                {
                    Email = "fatima.mohammed@student.edu.iq",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student2024!"),
                    UserType = UserRoles.Student,
                    IsActive = true,
                    EmailVerified = true
                };

                var studentUser3 = new User
                {
                    Email = "omar.khalil@student.edu.iq",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student2024!"),
                    UserType = UserRoles.Student,
                    IsActive = true,
                    EmailVerified = true
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
                    IsActive = true
                };

                var professor2 = new Professor
                {
                    UserId = profUser2.Id,
                    EmployeeCode = "ENG002",
                    FullName = "د. سارة علي الكربلائي",
                    Department = "علوم الحاسوب",
                    Title = "Dr.",
                    Phone = "07709876543",
                    IsActive = true
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
                    IsActive = true
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
                    IsActive = true
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
                    IsActive = true
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
                    IsActive = true
                };

                var subject2 = new Subject
                {
                    Name = "هياكل البيانات",
                    Code = "CS201",
                    Stage = "Second",
                    StudyType = "Morning",
                    CreditHours = 4,
                    Description = "دراسة هياكل البيانات والخوارزميات",
                    IsActive = true
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
                    IsActive = true
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
                    Notes = "محاضرة المتغيرات والثوابت"
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
                    Notes = "حضر في الوقت المحدد"
                };

                var attendance2 = new AttendanceRecord
                {
                    SessionId = session1.Id,
                    StudentId = student2.Id,
                    AttendanceStatus = AttendanceStatus.Late,
                    DetectionMethod = DetectionMethods.Manual,
                    EntryTime = DateTime.Now.AddMinutes(15),
                    Notes = "تأخر 15 دقيقة"
                };

                await _context.AttendanceRecords.AddRangeAsync(attendance1, attendance2);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "✅ تم إنشاء البيانات التجريبية الحقيقية بنجاح!",
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
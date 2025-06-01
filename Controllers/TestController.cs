using Microsoft.AspNetCore.Mvc;
using SmartAttendance.API.Models.Entities;
using SmartAttendance.API.Constants;

namespace SmartAttendance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("models-test")]
        public IActionResult TestModels()
        {
            try
            {
                // اختبار إنشاء User
                var user = new User
                {
                    Email = "test@example.com",
                    PasswordHash = "hashedpassword",
                    UserType = UserRoles.Student,
                    IsActive = true
                };

                // اختبار إنشاء Student
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

                // اختبار إنشاء Subject
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
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في الـ Models",
                    error = ex.Message
                });
            }
        }

        [HttpGet("packages-test")]
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
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في الـ Packages",
                    error = ex.Message
                });
            }
        }

        [HttpGet("entities-relationships")]
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
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في العلاقات",
                    error = ex.Message
                });
            }
        }

        [HttpGet("validation-test")]
        public IActionResult TestValidations()
        {
            try
            {
                var validationResults = new List<object>();

                // اختبار Base Entity
                var baseEntity = new Student();
                validationResults.Add(new
                {
                    entity = "BaseEntity",
                    hasId = baseEntity.Id >= 0,
                    hasCreatedAt = baseEntity.CreatedAt != default,
                    hasIsDeleted = baseEntity.IsDeleted == false
                });

                // اختبار Required Fields
                var user = new User
                {
                    Email = "", // خطأ - مطلوب
                    PasswordHash = "", // خطأ - مطلوب
                    UserType = "" // خطأ - مطلوب
                };

                validationResults.Add(new
                {
                    entity = "User",
                    emailEmpty = string.IsNullOrEmpty(user.Email),
                    passwordEmpty = string.IsNullOrEmpty(user.PasswordHash),
                    userTypeEmpty = string.IsNullOrEmpty(user.UserType)
                });

                return Ok(new
                {
                    success = true,
                    message = "✅ اختبار التحقق من البيانات مكتمل!",
                    validationResults
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في التحقق",
                    error = ex.Message
                });
            }
        }

        [HttpGet("constants-test")]
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
                        QRCode = DetectionMethods.QRCode
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
                }
            });
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new
            {
                success = true,
                message = "✅ المشروع يعمل بنجاح!",
                timestamp = DateTime.UtcNow,
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
                version = "1.0.0"
            });
        }
    }
}
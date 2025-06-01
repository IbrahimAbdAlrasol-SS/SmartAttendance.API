using Microsoft.EntityFrameworkCore;
using AutoMapper;
using SmartAttendance.API.Data;
using SmartAttendance.API.Models.Entities;
using SmartAttendance.API.Models.DTOs;
using SmartAttendance.API.Services.Interfaces;
using SmartAttendance.API.Constants;

namespace SmartAttendance.API.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly ILogger<StudentService> _logger;

        public StudentService(
            ApplicationDbContext context,
            IMapper mapper,
            IFileService fileService,
            ILogger<StudentService> logger)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
            _logger = logger;
        }

        public async Task<ApiResponse<PagedResult<StudentListDto>>> GetStudentsAsync(StudentFilterRequest? filter = null)
        {
            try
            {
                var query = _context.Students
                    .Include(s => s.User)
                    .Where(s => !s.IsDeleted)
                    .AsQueryable();

                // تطبيق الفلاتر
                if (filter != null)
                {
                    if (!string.IsNullOrWhiteSpace(filter?.SearchTerm))
                    {
                        query = query.Where(s => 
                            s.FullName.Contains(filter.SearchTerm) ||
                            s.StudentCode!.Contains(filter.SearchTerm) ||
                            (s.User != null && s.User.Email != null && s.User.Email.Contains(filter.SearchTerm)));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Stage))
                    {
                        query = query.Where(s => s.Stage == filter.Stage);
                    }

                    if (!string.IsNullOrWhiteSpace(filter.StudyType))
                    {
                        query = query.Where(s => s.StudyType == filter.StudyType);
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Section))
                    {
                        query = query.Where(s => s.Section == filter.Section);
                    }

                    if (filter.IsActive.HasValue)
                    {
                        query = query.Where(s => s.IsActive == filter.IsActive.Value);
                    }

                    if (filter.HasFaceData.HasValue)
                    {
                        if (filter.HasFaceData.Value)
                        {
                            query = query.Where(s => !string.IsNullOrEmpty(s.FaceEncodingData));
                        }
                        else
                        {
                            query = query.Where(s => string.IsNullOrEmpty(s.FaceEncodingData));
                        }
                    }
                }

                // ترتيب النتائج
                query = filter?.SortBy?.ToLower() switch
                {
                    "studentcode" => filter.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(s => s.StudentCode)
                        : query.OrderBy(s => s.StudentCode),
                    "createdat" => filter.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(s => s.CreatedAt)
                        : query.OrderBy(s => s.CreatedAt),
                    _ => filter?.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(s => s.FullName)
                        : query.OrderBy(s => s.FullName)
                };

                var totalCount = await query.CountAsync();

                // تطبيق الـ pagination
                var pageNumber = filter?.PageNumber ?? 1;
                var pageSize = filter?.PageSize ?? 10;
                
                var students = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var studentDtos = _mapper.Map<List<StudentListDto>>(students);

                var result = new PagedResult<StudentListDto>
                {
                    Data = studentDtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                    HasNextPage = pageNumber < Math.Ceiling((double)totalCount / pageSize),
                    HasPreviousPage = pageNumber > 1
                };

                return ApiResponse<PagedResult<StudentListDto>>.SuccessResult(
                    result, "تم جلب الطلاب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting students");
                return ApiResponse<PagedResult<StudentListDto>>.ErrorResult(
                    "حدث خطأ أثناء جلب الطلاب", statusCode: 500);
            }
        }

        public async Task<ApiResponse<StudentDto>> GetStudentByIdAsync(int id)
        {
            try
            {
                var student = await _context.Students
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

                if (student == null)
                {
                    return ApiResponse<StudentDto>.ErrorResult(
                        "الطالب غير موجود", statusCode: 404);
                }

                var studentDto = _mapper.Map<StudentDto>(student);
                return ApiResponse<StudentDto>.SuccessResult(
                    studentDto, "تم جلب بيانات الطالب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student by id {StudentId}", id);
                return ApiResponse<StudentDto>.ErrorResult(
                    "حدث خطأ أثناء جلب بيانات الطالب", statusCode: 500);
            }
        }

        public async Task<ApiResponse<StudentDto>> GetStudentByUserIdAsync(int userId)
        {
            try
            {
                var student = await _context.Students
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.UserId == userId && !s.IsDeleted);

                if (student == null)
                {
                    return ApiResponse<StudentDto>.ErrorResult(
                        "الطالب غير موجود", statusCode: 404);
                }

                var studentDto = _mapper.Map<StudentDto>(student);
                return ApiResponse<StudentDto>.SuccessResult(
                    studentDto, "تم جلب بيانات الطالب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student by user id {UserId}", userId);
                return ApiResponse<StudentDto>.ErrorResult(
                    "حدث خطأ أثناء جلب بيانات الطالب", statusCode: 500);
            }
        }

        public async Task<ApiResponse<StudentDto>> GetStudentByCodeAsync(string studentCode)
        {
            try
            {
                var student = await _context.Students
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.StudentCode == studentCode && !s.IsDeleted);

                if (student == null)
                {
                    return ApiResponse<StudentDto>.ErrorResult(
                        "الطالب غير موجود", statusCode: 404);
                }

                var studentDto = _mapper.Map<StudentDto>(student);
                return ApiResponse<StudentDto>.SuccessResult(
                    studentDto, "تم جلب بيانات الطالب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student by code {StudentCode}", studentCode);
                return ApiResponse<StudentDto>.ErrorResult(
                    "حدث خطأ أثناء جلب بيانات الطالب", statusCode: 500);
            }
        }

        public async Task<ApiResponse<StudentDto>> CreateStudentAsync(CreateStudentRequest request)
        {
            try
            {
                // التحقق من عدم وجود طالب بنفس رقم الطالب
                if (!string.IsNullOrEmpty(request.StudentCode))
                {
                    var existingStudent = await _context.Students
                        .FirstOrDefaultAsync(s => s.StudentCode == request.StudentCode && !s.IsDeleted);

                    if (existingStudent != null)
                    {
                        return ApiResponse<StudentDto>.ErrorResult(
                            "رقم الطالب موجود مسبقاً", statusCode: 409);
                    }
                }

                // التحقق من عدم وجود البريد الإلكتروني
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email && !u.IsDeleted);

                if (existingUser != null)
                {
                    return ApiResponse<StudentDto>.ErrorResult(
                        "البريد الإلكتروني موجود مسبقاً", statusCode: 409);
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // إنشاء المستخدم
                    var user = new User
                    {
                        Email = request.Email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student2024!"), // كلمة مرور افتراضية
                        UserType = UserRoles.Student,
                        IsActive = true,
                        EmailVerified = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    // إنشاء الطالب
                    var student = _mapper.Map<Student>(request);
                    student.UserId = user.Id;
                    student.IsActive = true;
                    student.CreatedAt = DateTime.UtcNow;
                    student.UpdatedAt = DateTime.UtcNow;

                    // إنشاء رقم طالب تلقائي إذا لم يتم إدخاله
                    if (string.IsNullOrEmpty(student.StudentCode))
                    {
                        student.StudentCode = await GenerateStudentCodeAsync(request.Stage, request.StudyType);
                    }

                    _context.Students.Add(student);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    // جلب الطالب مع بيانات المستخدم
                    var createdStudent = await _context.Students
                        .Include(s => s.User)
                        .FirstOrDefaultAsync(s => s.Id == student.Id);

                    var studentDto = _mapper.Map<StudentDto>(createdStudent);
                    return ApiResponse<StudentDto>.SuccessResult(
                        studentDto, "تم إنشاء الطالب بنجاح");
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating student");
                return ApiResponse<StudentDto>.ErrorResult(
                    "حدث خطأ أثناء إنشاء الطالب", statusCode: 500);
            }
        }

        public async Task<ApiResponse<StudentDto>> UpdateStudentAsync(int id, UpdateStudentRequest request)
        {
            try
            {
                var student = await _context.Students
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

                if (student == null)
                {
                    return ApiResponse<StudentDto>.ErrorResult(
                        "الطالب غير موجود", statusCode: 404);
                }

                // تحديث بيانات الطالب
                _mapper.Map(request, student);
                student.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var studentDto = _mapper.Map<StudentDto>(student);
                return ApiResponse<StudentDto>.SuccessResult(
                    studentDto, "تم تحديث بيانات الطالب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating student {StudentId}", id);
                return ApiResponse<StudentDto>.ErrorResult(
                    "حدث خطأ أثناء تحديث بيانات الطالب", statusCode: 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteStudentAsync(int id)
        {
            try
            {
                var student = await _context.Students
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

                if (student == null)
                {
                    return ApiResponse<bool>.ErrorResult(
                        "الطالب غير موجود", statusCode: 404);
                }

                // حذف ناعم
                student.IsDeleted = true;
                student.User.IsDeleted = true;
                student.UpdatedAt = DateTime.UtcNow;
                student.User.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(
                    true, "تم حذف الطالب بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting student {StudentId}", id);
                return ApiResponse<bool>.ErrorResult(
                    "حدث خطأ أثناء حذف الطالب", statusCode: 500);
            }
        }

        public async Task<ApiResponse<bool>> UploadFaceImageAsync(int id, IFormFile faceImage)
        {
            try
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

                if (student == null)
                {
                    return ApiResponse<bool>.ErrorResult(
                        "الطالب غير موجود", statusCode: 404);
                }

                // رفع الصورة
                var uploadResult = await _fileService.UploadImageAsync(
                    faceImage, "faces", $"student_{id}");

                if (!uploadResult.Success)
                {
                    return ApiResponse<bool>.ErrorResult(
                        uploadResult.Message, statusCode: 400);
                }

                // تحديث مسار الصورة وبيانات الوجه
                student.ProfileImage = uploadResult.Data;
                student.LastFaceUpdate = DateTime.UtcNow;
                student.UpdatedAt = DateTime.UtcNow;

                // TODO: إضافة معالجة Face Encoding هنا
                // student.FaceEncodingData = await ProcessFaceEncoding(uploadResult.Data);

                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(
                    true, "تم رفع صورة الوجه بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading face image for student {StudentId}", id);
                return ApiResponse<bool>.ErrorResult(
                    "حدث خطأ أثناء رفع صورة الوجه", statusCode: 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteFaceImageAsync(int id)
        {
            try
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

                if (student == null)
                {
                    return ApiResponse<bool>.ErrorResult(
                        "الطالب غير موجود", statusCode: 404);
                }

                if (string.IsNullOrEmpty(student.ProfileImage))
                {
                    return ApiResponse<bool>.ErrorResult(
                        "لا توجد صورة وجه للطالب", statusCode: 400);
                }

                // حذف الصورة
                await _fileService.DeleteFileAsync(student.ProfileImage);

                // تحديث قاعدة البيانات
                student.ProfileImage = null;
                student.FaceEncodingData = null;
                student.LastFaceUpdate = null;
                student.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(
                    true, "تم حذف صورة الوجه بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting face image for student {StudentId}", id);
                return ApiResponse<bool>.ErrorResult(
                    "حدث خطأ أثناء حذف صورة الوجه", statusCode: 500);
            }
        }

        public async Task<ApiResponse<List<StudentListDto>>> SearchStudentsAsync(string query)
        {
            try
            {
                var students = await _context.Students
                    .Include(s => s.User)
                    .Where(s => !s.IsDeleted && (
                        s.FullName.Contains(query) ||
                        s.StudentCode!.Contains(query) ||
                        s.User.Email.Contains(query)))
                    .OrderBy(s => s.FullName)
                    .Take(20)
                    .ToListAsync();

                var studentDtos = _mapper.Map<List<StudentListDto>>(students);
                return ApiResponse<List<StudentListDto>>.SuccessResult(
                    studentDtos, "تم البحث بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching students with query {Query}", query);
                return ApiResponse<List<StudentListDto>>.ErrorResult(
                    "حدث خطأ أثناء البحث", statusCode: 500);
            }
        }

        public async Task<ApiResponse<StudentStatisticsDto>> GetStudentStatisticsAsync()
        {
            try
            {
                var totalStudents = await _context.Students.CountAsync(s => !s.IsDeleted);
                var activeStudents = await _context.Students.CountAsync(s => s.IsActive && !s.IsDeleted);
                var studentsWithFaceData = await _context.Students
                    .CountAsync(s => !string.IsNullOrEmpty(s.FaceEncodingData) && !s.IsDeleted);

                var studentsByStage = await _context.Students
                    .Where(s => !s.IsDeleted)
                    .GroupBy(s => s.Stage)
                    .Select(g => new { Stage = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Stage, x => x.Count);

                var studentsByStudyType = await _context.Students
                    .Where(s => !s.IsDeleted)
                    .GroupBy(s => s.StudyType)
                    .Select(g => new { StudyType = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.StudyType, x => x.Count);

                var studentsBySection = await _context.Students
                    .Where(s => !s.IsDeleted)
                    .GroupBy(s => s.Section)
                    .Select(g => new { Section = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Section, x => x.Count);

                var statistics = new StudentStatisticsDto
                {
                    TotalStudents = totalStudents,
                    ActiveStudents = activeStudents,
                    InactiveStudents = totalStudents - activeStudents,
                    StudentsWithFaceData = studentsWithFaceData,
                    StudentsWithoutFaceData = totalStudents - studentsWithFaceData,
                    StudentsByStage = studentsByStage,
                    StudentsByStudyType = studentsByStudyType,
                    StudentsBySection = studentsBySection
                };

                return ApiResponse<StudentStatisticsDto>.SuccessResult(
                    statistics, "تم جلب الإحصائيات بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student statistics");
                return ApiResponse<StudentStatisticsDto>.ErrorResult(
                    "حدث خطأ أثناء جلب الإحصائيات", statusCode: 500);
            }
        }

        public async Task<ApiResponse<List<StudentListDto>>> GetStudentsBySectionAsync(string stage, string studyType, string section)
        {
            try
            {
                var students = await _context.Students
                    .Include(s => s.User)
                    .Where(s => s.Stage == stage && 
                               s.StudyType == studyType && 
                               s.Section == section && 
                               !s.IsDeleted)
                    .OrderBy(s => s.FullName)
                    .ToListAsync();

                var studentDtos = _mapper.Map<List<StudentListDto>>(students);
                return ApiResponse<List<StudentListDto>>.SuccessResult(
                    studentDtos, "تم جلب طلاب الشعبة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting students by section");
                return ApiResponse<List<StudentListDto>>.ErrorResult(
                    "حدث خطأ أثناء جلب طلاب الشعبة", statusCode: 500);
            }
        }

        private async Task<string> GenerateStudentCodeAsync(string stage, string studyType)
        {
            var year = DateTime.Now.Year;
            var prefix = $"{stage[0]}{studyType[0]}{year}";
            
            var lastStudent = await _context.Students
                .Where(s => s.StudentCode!.StartsWith(prefix) && !s.IsDeleted)
                .OrderByDescending(s => s.StudentCode)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastStudent != null && lastStudent.StudentCode != null)
            {
                var lastNumberStr = lastStudent.StudentCode.Substring(prefix.Length);
                if (int.TryParse(lastNumberStr, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"{prefix}{nextNumber:D3}";
        }
    }
}
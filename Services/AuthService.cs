using Microsoft.EntityFrameworkCore;
using SmartAttendance.API.Data;
using SmartAttendance.API.Models.Entities;
using SmartAttendance.API.Models.DTOs;
using SmartAttendance.API.Constants;
using AutoMapper;

namespace SmartAttendance.API.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
        Task<ApiResponse<UserInfoDto>> RegisterStudentAsync(RegisterStudentRequest request);
        Task<ApiResponse<UserInfoDto>> RegisterProfessorAsync(RegisterProfessorRequest request);
        Task<ApiResponse<LoginResponse>> RefreshTokenAsync(string refreshToken);
        Task<ApiResponse<bool>> ChangePasswordAsync(int userId, ChangePasswordRequest request);
        Task<ApiResponse<UserInfoDto>> GetUserProfileAsync(int userId);
    }

    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public AuthService(
            ApplicationDbContext context,
            IUserService userService,
            IJwtService jwtService,
            IMapper mapper)
        {
            _context = context;
            _userService = userService;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
        {
            try
            {
                // التحقق من وجود المستخدم
                var user = await _userService.GetUserByEmailAsync(request.Email);
                if (user == null)
                {
                    return ApiResponse<LoginResponse>.ErrorResult(
                        "البريد الإلكتروني أو كلمة المرور غير صحيحة", 
                        statusCode: 401);
                }

                // التحقق من كلمة المرور
                if (!await _userService.VerifyPasswordAsync(user, request.Password))
                {
                    return ApiResponse<LoginResponse>.ErrorResult(
                        "البريد الإلكتروني أو كلمة المرور غير صحيحة", 
                        statusCode: 401);
                }

                // التحقق من حالة المستخدم
                if (!user.IsActive)
                {
                    return ApiResponse<LoginResponse>.ErrorResult(
                        "الحساب غير مفعل، يرجى التواصل مع الإدارة", 
                        statusCode: 403);
                }

                // إنشاء الرموز المميزة
                var accessToken = _jwtService.GenerateAccessToken(user);
                var refreshToken = _jwtService.GenerateRefreshToken();

                // تحديث refresh token في قاعدة البيانات (يمكن إضافة جدول للـ refresh tokens لاحقاً)

                var userInfo = _mapper.Map<UserInfoDto>(user);

                var loginResponse = new LoginResponse
                {
                    Token = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddHours(24),
                    User = userInfo
                };

                return ApiResponse<LoginResponse>.SuccessResult(loginResponse, "تم تسجيل الدخول بنجاح");
            }
            catch (Exception ex)
            {
                return ApiResponse<LoginResponse>.ErrorResult(
                    "حدث خطأ أثناء تسجيل الدخول", 
                    new List<string> { ex.Message }, 
                    500);
            }
        }

        public async Task<ApiResponse<UserInfoDto>> RegisterStudentAsync(RegisterStudentRequest request)
        {
            try
            {
                // التحقق من عدم وجود البريد الإلكتروني مسبقاً
                if (await _userService.EmailExistsAsync(request.Email))
                {
                    return ApiResponse<UserInfoDto>.ErrorResult(
                        "البريد الإلكتروني مستخدم مسبقاً", 
                        statusCode: 409);
                }

                // التحقق من رقم الطالب إذا تم إدخاله
                if (!string.IsNullOrEmpty(request.StudentCode))
                {
                    var existingStudent = await _context.Students
                        .AnyAsync(s => s.StudentCode == request.StudentCode);
                    
                    if (existingStudent)
                    {
                        return ApiResponse<UserInfoDto>.ErrorResult(
                            "رقم الطالب مستخدم مسبقاً", 
                            statusCode: 409);
                    }
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // إنشاء المستخدم
                    var user = _mapper.Map<User>(request);
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                    user.UserType = UserRoles.Student;

                    var createdUser = await _userService.CreateUserAsync(user);

                    // إنشاء بيانات الطالب
                    var student = _mapper.Map<Student>(request);
                    student.UserId = createdUser.Id;

                    // إنشاء رقم طالب تلقائي إذا لم يتم إدخاله
                    if (string.IsNullOrEmpty(student.StudentCode))
                    {
                        student.StudentCode = await GenerateStudentCodeAsync(request.Stage, request.StudyType);
                    }

                    _context.Students.Add(student);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    // إحضار المستخدم مع التفاصيل
                    var userWithDetails = await _userService.GetUserWithDetailsAsync(createdUser.Id);
                    var userInfo = _mapper.Map<UserInfoDto>(userWithDetails);

                    return ApiResponse<UserInfoDto>.SuccessResult(userInfo, "تم إنشاء حساب الطالب بنجاح");
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<UserInfoDto>.ErrorResult(
                    "حدث خطأ أثناء إنشاء الحساب", 
                    new List<string> { ex.Message }, 
                    500);
            }
        }

        public async Task<ApiResponse<UserInfoDto>> RegisterProfessorAsync(RegisterProfessorRequest request)
        {
            try
            {
                // التحقق من عدم وجود البريد الإلكتروني مسبقاً
                if (await _userService.EmailExistsAsync(request.Email))
                {
                    return ApiResponse<UserInfoDto>.ErrorResult(
                        "البريد الإلكتروني مستخدم مسبقاً", 
                        statusCode: 409);
                }

                // التحقق من رقم الموظف إذا تم إدخاله
                if (!string.IsNullOrEmpty(request.EmployeeCode))
                {
                    var existingProfessor = await _context.Professors
                        .AnyAsync(p => p.EmployeeCode == request.EmployeeCode);
                    
                    if (existingProfessor)
                    {
                        return ApiResponse<UserInfoDto>.ErrorResult(
                            "رقم الموظف مستخدم مسبقاً", 
                            statusCode: 409);
                    }
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // إنشاء المستخدم
                    var user = _mapper.Map<User>(request);
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                    user.UserType = UserRoles.Professor;

                    var createdUser = await _userService.CreateUserAsync(user);

                    // إنشاء بيانات الأستاذ
                    var professor = _mapper.Map<Professor>(request);
                    professor.UserId = createdUser.Id;

                    // إنشاء رقم موظف تلقائي إذا لم يتم إدخاله
                    if (string.IsNullOrEmpty(professor.EmployeeCode))
                    {
                        professor.EmployeeCode = await GenerateEmployeeCodeAsync();
                    }

                    _context.Professors.Add(professor);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    // إحضار المستخدم مع التفاصيل
                    var userWithDetails = await _userService.GetUserWithDetailsAsync(createdUser.Id);
                    var userInfo = _mapper.Map<UserInfoDto>(userWithDetails);

                    return ApiResponse<UserInfoDto>.SuccessResult(userInfo, "تم إنشاء حساب الأستاذ بنجاح");
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<UserInfoDto>.ErrorResult(
                    "حدث خطأ أثناء إنشاء الحساب", 
                    new List<string> { ex.Message }, 
                    500);
            }
        }

        public async Task<ApiResponse<LoginResponse>> RefreshTokenAsync(string refreshToken)
        {
            // TODO: Implement refresh token validation from database
            // For now, return error
            return ApiResponse<LoginResponse>.ErrorResult(
                "Refresh token غير صالح", 
                statusCode: 401);
        }

        public async Task<ApiResponse<bool>> ChangePasswordAsync(int userId, ChangePasswordRequest request)
        {
            try
            {
                var success = await _userService.ChangePasswordAsync(
                    userId, 
                    request.CurrentPassword, 
                    request.NewPassword);

                if (!success)
                {
                    return ApiResponse<bool>.ErrorResult(
                        "كلمة المرور الحالية غير صحيحة", 
                        statusCode: 400);
                }

                return ApiResponse<bool>.SuccessResult(true, "تم تغيير كلمة المرور بنجاح");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(
                    "حدث خطأ أثناء تغيير كلمة المرور", 
                    new List<string> { ex.Message }, 
                    500);
            }
        }

        public async Task<ApiResponse<UserInfoDto>> GetUserProfileAsync(int userId)
        {
            try
            {
                var user = await _userService.GetUserWithDetailsAsync(userId);
                if (user == null)
                {
                    return ApiResponse<UserInfoDto>.ErrorResult(
                        "المستخدم غير موجود", 
                        statusCode: 404);
                }

                var userInfo = _mapper.Map<UserInfoDto>(user);
                return ApiResponse<UserInfoDto>.SuccessResult(userInfo, "تم جلب بيانات المستخدم بنجاح");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserInfoDto>.ErrorResult(
                    "حدث خطأ أثناء جلب بيانات المستخدم", 
                    new List<string> { ex.Message }, 
                    500);
            }
        }

        private async Task<string> GenerateStudentCodeAsync(string stage, string studyType)
        {
            var year = DateTime.Now.Year;
            var prefix = $"{stage[0]}{studyType[0]}{year}";
            
            var lastStudent = await _context.Students
                .Where(s => s.StudentCode!.StartsWith(prefix))
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

        private async Task<string> GenerateEmployeeCodeAsync()
        {
            var year = DateTime.Now.Year;
            var prefix = $"EMP{year}";
            
            var lastProfessor = await _context.Professors
                .Where(p => p.EmployeeCode!.StartsWith(prefix))
                .OrderByDescending(p => p.EmployeeCode)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastProfessor != null && lastProfessor.EmployeeCode != null)
            {
                var lastNumberStr = lastProfessor.EmployeeCode.Substring(prefix.Length);
                if (int.TryParse(lastNumberStr, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"{prefix}{nextNumber:D3}";
        }
    }
}
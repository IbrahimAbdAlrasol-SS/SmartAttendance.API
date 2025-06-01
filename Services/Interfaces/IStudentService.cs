using SmartAttendance.API.Models.DTOs;

namespace SmartAttendance.API.Services.Interfaces
{
    public interface IStudentService
    {
        Task<ApiResponse<PagedResult<StudentListDto>>> GetStudentsAsync(StudentFilterRequest? filter = null);
        Task<ApiResponse<StudentDto>> GetStudentByIdAsync(int id);
        Task<ApiResponse<StudentDto>> GetStudentByUserIdAsync(int userId);
        Task<ApiResponse<StudentDto>> GetStudentByCodeAsync(string studentCode);
        Task<ApiResponse<StudentDto>> CreateStudentAsync(CreateStudentRequest request);
        Task<ApiResponse<StudentDto>> UpdateStudentAsync(int id, UpdateStudentRequest request);
        Task<ApiResponse<bool>> DeleteStudentAsync(int id);
        Task<ApiResponse<bool>> UploadFaceImageAsync(int id, IFormFile faceImage);
        Task<ApiResponse<bool>> DeleteFaceImageAsync(int id);
        Task<ApiResponse<List<StudentListDto>>> SearchStudentsAsync(string query);
        Task<ApiResponse<StudentStatisticsDto>> GetStudentStatisticsAsync();
        Task<ApiResponse<List<StudentListDto>>> GetStudentsBySectionAsync(string stage, string studyType, string section);
    }
}
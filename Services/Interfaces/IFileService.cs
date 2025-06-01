using SmartAttendance.API.Models.DTOs;

namespace SmartAttendance.API.Services.Interfaces
{
    public interface IFileService
    {
        Task<ApiResponse<string>> UploadImageAsync(IFormFile file, string folder, string? fileName = null);
        Task<ApiResponse<bool>> DeleteFileAsync(string filePath);
        Task<ApiResponse<byte[]>> GetFileAsync(string filePath);
        bool IsValidImageFile(IFormFile file);
        Task<ApiResponse<string>> ProcessFaceImageAsync(IFormFile image, int studentId);
    }
}
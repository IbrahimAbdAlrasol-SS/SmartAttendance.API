using Microsoft.AspNetCore.Mvc;
using SmartAttendance.API.Services.Interfaces;
using SmartAttendance.API.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.API.Data;

namespace SmartAttendance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaceTestController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public FaceTestController(
            IFileService fileService,
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _fileService = fileService;
            _context = context;
            _environment = environment;
        }

        /// <summary>
        /// رفع صورة اختبار
        /// </summary>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadTestImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest(ApiResponse<bool>.ErrorResult(
                    "الصورة مطلوبة", 
                    statusCode: 400));
            }

            // التحقق من صيغة الصورة
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(ApiResponse<bool>.ErrorResult(
                    "صيغة الصورة غير مدعومة. الصيغ المدعومة: JPG, JPEG, PNG", 
                    statusCode: 400));
            }

            // التحقق من حجم الصورة (أقل من 5 ميجا)
            if (image.Length > 5 * 1024 * 1024)
            {
                return BadRequest(ApiResponse<bool>.ErrorResult(
                    "حجم الصورة كبير جداً. الحد الأقصى 5 ميجابايت", 
                    statusCode: 400));
            }

            var result = await _fileService.UploadImageAsync(image, "test", "face_test");

            return Ok(new
            {
                success = true,
                message = "تم رفع الصورة بنجاح",
                imageUrl = $"{Request.Scheme}://{Request.Host}/{result.Data}",
                filePath = result.Data
            });
        }

        /// <summary>
        /// معلومات الصور المرفوعة
        /// </summary>
        [HttpGet("images")]
        public IActionResult GetUploadedImages()
        {
            try
            {
                var testDirectory = Path.Combine(_environment.WebRootPath, "uploads", "test");
                var facesDirectory = Path.Combine(_environment.WebRootPath, "uploads", "faces");
                
                if (!Directory.Exists(testDirectory))
                {
                    Directory.CreateDirectory(testDirectory);
                }
                
                if (!Directory.Exists(facesDirectory))
                {
                    Directory.CreateDirectory(facesDirectory);
                }
                
                var testImages = Directory.GetFiles(testDirectory)
                    .Select(f => new FileInfo(f))
                    .Select(f => new
                    {
                        name = f.Name,
                        size = f.Length,
                        created = f.CreationTime,
                        url = $"{Request.Scheme}://{Request.Host}/uploads/test/{f.Name}"
                    })
                    .ToList();
                
                var faceImages = Directory.GetFiles(facesDirectory)
                    .Select(f => new FileInfo(f))
                    .Select(f => new
                    {
                        name = f.Name,
                        size = f.Length,
                        created = f.CreationTime,
                        url = $"{Request.Scheme}://{Request.Host}/uploads/faces/{f.Name}"
                    })
                    .ToList();
                
                return Ok(new
                {
                    success = true,
                    message = "تم جلب معلومات الصور",
                    testImages,
                    faceImages
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "خطأ في جلب معلومات الصور",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// حذف جميع الصور الاختبارية
        /// </summary>
        [HttpDelete("cleanup")]
        public IActionResult CleanupTestImages()
        {
            try
            {
                var testDirectory = Path.Combine(_environment.WebRootPath, "uploads", "test");
                
                if (Directory.Exists(testDirectory))
                {
                    var files = Directory.GetFiles(testDirectory);
                    foreach (var file in files)
                    {
                        System.IO.File.Delete(file);
                    }
                }
                
                return Ok(new
                {
                    success = true,
                    message = "تم حذف جميع الصور الاختبارية",
                    deletedCount = Directory.Exists(testDirectory) ? Directory.GetFiles(testDirectory).Length : 0
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "خطأ في حذف الصور الاختبارية",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// رفع صورة وجه لطالب محدد
        /// </summary>
        [HttpPost("student/{id}/face")]
        public async Task<IActionResult> UploadStudentFace(int id, IFormFile faceImage)
        {
            if (faceImage == null || faceImage.Length == 0)
            {
                return BadRequest(ApiResponse<bool>.ErrorResult(
                    "صورة الوجه مطلوبة", 
                    statusCode: 400));
            }

            try
            {
                // التحقق من وجود الطالب
                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

                if (student == null)
                {
                    return NotFound(ApiResponse<bool>.ErrorResult(
                        "الطالب غير موجود", 
                        statusCode: 404));
                }

                // رفع الصورة
                var uploadResult = await _fileService.UploadImageAsync(
                    faceImage, "faces", $"student_{id}");

                if (!uploadResult.Success)
                {
                    return BadRequest(ApiResponse<bool>.ErrorResult(
                        uploadResult.Message, 
                        statusCode: 400));
                }

                // تحديث بيانات الطالب
                student.ProfileImage = uploadResult.Data;
                student.LastFaceUpdate = DateTime.UtcNow;
                student.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "تم رفع صورة وجه الطالب بنجاح",
                    studentName = student.FullName,
                    imageUrl = $"{Request.Scheme}://{Request.Host}/{uploadResult.Data}"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "خطأ في رفع صورة الوجه",
                    error = ex.Message
                });
            }
        }
    }
}
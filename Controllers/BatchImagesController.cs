using Microsoft.AspNetCore.Mvc;
using SmartAttendance.API.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartAttendance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchImagesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _environment;

        public BatchImagesController(
            IFileService fileService,
            IWebHostEnvironment environment)
        {
            _fileService = fileService;
            _environment = environment;
        }

        /// <summary>
        /// رفع مجموعة من الصور
        /// </summary>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadMultipleImages([FromForm] List<IFormFile> images)
        {
            if (images == null || images.Count == 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "لم يتم تحديد أي صور"
                });
            }

            var results = new List<object>();
            int successCount = 0;
            int failCount = 0;

            foreach (var image in images)
            {
                try
                {
                    if (image.Length == 0 || !_fileService.IsValidImageFile(image))
                    {
                        results.Add(new
                        {
                            fileName = image.FileName,
                            success = false,
                            message = "ملف غير صالح أو صيغة غير مدعومة"
                        });
                        failCount++;
                        continue;
                    }

                    var uploadResult = await _fileService.UploadImageAsync(
                        image, "batch", $"batch_{Guid.NewGuid()}");

                    if (uploadResult.Success)
                    {
                        results.Add(new
                        {
                            fileName = image.FileName,
                            success = true,
                            filePath = uploadResult.Data,
                            fileUrl = $"{Request.Scheme}://{Request.Host}/{uploadResult.Data}"
                        });
                        successCount++;
                    }
                    else
                    {
                        results.Add(new
                        {
                            fileName = image.FileName,
                            success = false,
                            message = uploadResult.Message
                        });
                        failCount++;
                    }
                }
                catch (Exception ex)
                {
                    results.Add(new
                    {
                        fileName = image.FileName,
                        success = false,
                        message = ex.Message
                    });
                    failCount++;
                }
            }

            return Ok(new
            {
                success = true,
                message = $"تم رفع {successCount} صورة بنجاح، و {failCount} صورة فشلت",
                totalImages = images.Count,
                successCount,
                failCount,
                results
            });
        }

        /// <summary>
        /// التحقق من المجلدات وإنشاء المجلدات المطلوبة
        /// </summary>
        [HttpPost("ensure-directories")]
        public IActionResult EnsureDirectories()
        {
            try
            {
                var webRootPath = _environment.WebRootPath;
                var uploadsPath = Path.Combine(webRootPath, "uploads");
                var batchPath = Path.Combine(uploadsPath, "batch");

                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                if (!Directory.Exists(batchPath))
                {
                    Directory.CreateDirectory(batchPath);
                }

                return Ok(new
                {
                    success = true,
                    message = "تم إنشاء المجلدات اللازمة",
                    paths = new
                    {
                        webRoot = webRootPath,
                        uploads = uploadsPath,
                        batch = batchPath
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "خطأ في إنشاء المجلدات",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// حذف جميع الصور المرفوعة دفعة واحدة
        /// </summary>
        [HttpDelete("cleanup")]
        public IActionResult CleanupBatchImages()
        {
            try
            {
                var batchDirectory = Path.Combine(_environment.WebRootPath, "uploads", "batch");
                int deletedCount = 0;

                if (Directory.Exists(batchDirectory))
                {
                    var files = Directory.GetFiles(batchDirectory);
                    deletedCount = files.Length;

                    foreach (var file in files)
                    {
                        System.IO.File.Delete(file);
                    }
                }

                return Ok(new
                {
                    success = true,
                    message = "تم حذف جميع الصور المرفوعة دفعة واحدة",
                    deletedCount
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "خطأ في حذف الصور",
                    error = ex.Message
                });
            }
        }
    }
}
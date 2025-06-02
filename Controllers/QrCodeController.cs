using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartAttendance.API.Models.DTOs;
using SmartAttendance.API.Services.Interfaces;

namespace SmartAttendance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QrCodeController : ControllerBase
    {
        private readonly IQrCodeService _qrCodeService;
        
        public QrCodeController(IQrCodeService qrCodeService)
        {
            _qrCodeService = qrCodeService;
        }
        
        /// <summary>
        /// توليد QR Code جديد للجلسة
        /// </summary>
        [HttpPost("generate")]
        [Authorize(Roles = "Professor,Admin")]
        public async Task<IActionResult> GenerateQrCode([FromBody] GenerateQrRequest request)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            
            var result = await _qrCodeService.GenerateQrCodeAsync(request, ipAddress, userAgent);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return StatusCode(result.StatusCode ?? 500, result);
        }
        
        /// <summary>
        /// التحقق من صحة QR Code
        /// </summary>
        [HttpPost("validate")]
        [Authorize(Roles = "Student,Professor,Admin")]
        public async Task<IActionResult> ValidateQrCode([FromBody] ValidateQrRequest request)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            
            var result = await _qrCodeService.ValidateQrCodeAsync(request, ipAddress, userAgent);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return StatusCode(result.StatusCode ?? 500, result);
        }
        
        /// <summary>
        /// الحصول على QR Codes النشطة للجلسة
        /// </summary>
        [HttpGet("session/{sessionId}/active")]
        [Authorize(Roles = "Professor,Admin")]
        public async Task<IActionResult> GetActiveQrCodes(int sessionId)
        {
            var result = await _qrCodeService.GetActiveQrCodesAsync(sessionId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return StatusCode(result.StatusCode ?? 500, result);
        }
        
        /// <summary>
        /// إنهاء صلاحية QR Code
        /// </summary>
        [HttpPost("{qrSessionId}/expire")]
        [Authorize(Roles = "Professor,Admin")]
        public async Task<IActionResult> ExpireQrCode(int qrSessionId)
        {
            var result = await _qrCodeService.ExpireQrCodeAsync(qrSessionId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return StatusCode(result.StatusCode ?? 500, result);
        }
        
        /// <summary>
        /// الحصول على سجلات استخدام QR Code
        /// </summary>
        [HttpGet("{qrSessionId}/usage-logs")]
        [Authorize(Roles = "Professor,Admin")]
        public async Task<IActionResult> GetUsageLogs(int qrSessionId)
        {
            var result = await _qrCodeService.GetQrUsageLogsAsync(qrSessionId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return StatusCode(result.StatusCode ?? 500, result);
        }
        
        /// <summary>
        /// الحصول على إحصائيات الأمان
        /// </summary>
        [HttpGet("session/{sessionId}/security-stats")]
        [Authorize(Roles = "Professor,Admin")]
        public async Task<IActionResult> GetSecurityStats(int sessionId)
        {
            var result = await _qrCodeService.GetQrSecurityStatsAsync(sessionId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return StatusCode(result.StatusCode ?? 500, result);
        }
        
        /// <summary>
        /// تنظيف QR Codes المنتهية الصلاحية
        /// </summary>
        [HttpPost("cleanup/expired")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CleanupExpiredQrCodes()
        {
            var result = await _qrCodeService.CleanupExpiredQrCodesAsync();
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return StatusCode(result.StatusCode ?? 500, result);
        }
    }
}
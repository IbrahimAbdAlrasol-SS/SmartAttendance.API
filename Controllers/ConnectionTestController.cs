using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.API.Data;
using MySqlConnector;

namespace SmartAttendance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionTestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public ConnectionTestController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new
            {
                success = true,
                message = "✅ API يعمل بنجاح!",
                timestamp = DateTime.Now
            });
        }

        [HttpGet("mysql-raw")]
        public async Task<IActionResult> TestMySQLRaw()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                
                using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();
                
                var command = new MySqlCommand("SELECT 'MySQL Connected!' as message, VERSION() as version", connection);
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    return Ok(new
                    {
                        success = true,
                        message = reader["message"]?.ToString(),
                        version = reader["version"]?.ToString(),
                        timestamp = DateTime.Now
                    });
                }
                
                return Ok(new { success = true, message = "MySQL Connected" });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    error = ex.Message,
                    suggestion = "تحقق من إعدادات MySQL"
                });
            }
        }

        [HttpGet("ef-test")]
        public async Task<IActionResult> TestEntityFramework()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                
                if (canConnect)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "✅ Entity Framework متصل بنجاح!",
                        databaseName = _context.Database.GetDbConnection().Database,
                        provider = _context.Database.ProviderName
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "❌ فشل اتصال Entity Framework"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في Entity Framework",
                    error = ex.Message
                });
            }
        }
    }
}
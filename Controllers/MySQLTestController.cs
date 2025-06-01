using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace SmartAttendance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MySQLTestController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MySQLTestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("test-connection")]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                
                using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();
                
                var command = new MySqlCommand("SELECT VERSION() as mysql_version, DATABASE() as current_db", connection);
                using var reader = await command.ExecuteReaderAsync();
                
                string version = "";
                string database = "";
                
                if (await reader.ReadAsync())
                {
                    version = reader["mysql_version"]?.ToString() ?? "Unknown";
                    database = reader["current_db"]?.ToString() ?? "None";
                }
                
                return Ok(new
                {
                    success = true,
                    message = "✅ MySQL متصل بنجاح!",
                    mysqlVersion = version,
                    currentDatabase = database,
                    serverInfo = new
                    {
                        host = "localhost",
                        port = 3306,
                        user = "root"
                    }
                });
            }
            catch (MySqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ في الاتصال بـ MySQL",
                    mysqlError = ex.Message,
                    errorCode = ex.Number,
                    suggestion = GetMySQLErrorSuggestion(ex.Number)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "❌ خطأ عام",
                    error = ex.Message
                });
            }
        }

        private string GetMySQLErrorSuggestion(int errorCode)
        {
            return errorCode switch
            {
                1045 => "كلمة المرور أو اسم المستخدم خاطئ",
                2003 => "MySQL Server غير مشغل أو Port خاطئ",
                1049 => "Database غير موجود",
                _ => "تحقق من إعدادات الاتصال"
            };
        }
    }
}
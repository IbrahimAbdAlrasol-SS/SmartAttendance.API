using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.FileProviders;
using System.Text;
using SmartAttendance.API.Data;
using SmartAttendance.API.Services; // Add this line
using SmartAttendance.API.Services.Interfaces;
using SmartAttendance.API.Services.Implementations;
using SmartAttendance.API.Mappings;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();

// Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "SmartAttendance-SecretKey-2024-MustBeAtLeast32Characters-Long";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "SmartAttendanceAPI",
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"] ?? "SmartAttendanceUsers",
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Register Services
try 
{
    builder.Services.AddScoped<IJwtService, JwtService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IStudentService, StudentService>();
    builder.Services.AddScoped<IFileService, FileService>();
    
    Console.WriteLine("✅ Services registered successfully");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error registering services: {ex.Message}");
}

// Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "Smart Attendance API", 
        Version = "v1",
        Description = "نظام الحضور الذكي - جامعة بولونيا"
    });
    
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Smart Attendance API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// Static Files Configuration
app.UseStaticFiles();

// Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Auto-migrate database and seed data on startup
if (app.Environment.IsDevelopment())
{
    try 
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        Console.WriteLine("🔄 Starting database migration...");
        await context.Database.MigrateAsync();
        Console.WriteLine("✅ Database migration completed successfully!");
        
        // Seed admin user
        await SeedAdminUser(context);
        Console.WriteLine("✅ Data seeding completed!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database error: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
    }
}

Console.WriteLine("🚀 Smart Attendance API is running!");
Console.WriteLine($"🌐 Swagger UI: https://localhost:{app.Urls.FirstOrDefault()?.Split(':').Last() ?? "7001"}");

app.Run();

// Helper method for seeding admin user
async Task SeedAdminUser(ApplicationDbContext context)
{
    try 
    {
        var adminExists = await context.Users.AnyAsync(u => u.Email == "admin@bologna.edu.iq");
        
        if (!adminExists)
        {
            var adminUser = new SmartAttendance.API.Models.Entities.User
            {
                Email = "admin@bologna.edu.iq",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin2024!"),
                UserType = SmartAttendance.API.Constants.UserRoles.Admin,
                IsActive = true,
                EmailVerified = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
            
            Console.WriteLine("👤 Admin user created:");
            Console.WriteLine("   📧 Email: admin@bologna.edu.iq");
            Console.WriteLine("   🔑 Password: Admin2024!");
        }
        else 
        {
            Console.WriteLine("👤 Admin user already exists");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Seeding error: {ex.Message}");
    }
}
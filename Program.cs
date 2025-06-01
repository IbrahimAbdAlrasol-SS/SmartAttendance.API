using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.FileProviders;
using System.Text;
using SmartAttendance.API.Data;
using SmartAttendance.API.Models.Entities;
using SmartAttendance.API.Services;
using SmartAttendance.API.Services.Interfaces;
using SmartAttendance.API.Services.Implementations;
using SmartAttendance.API.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"📊 Connection String: {connectionString}");

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
var secretKey = jwtSettings["SecretKey"] ?? "SmartAttendance-SecretKey-2024-MustBeAtLeast32Characters-Long-ForSecurity";

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
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IFileService, FileService>();

Console.WriteLine("✅ Services registered successfully");

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

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
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
}

app.UseHttpsRedirection();

// Static Files and Uploads Directory Configuration
var webRootPath = app.Environment.WebRootPath ?? "wwwroot";
var uploadsDirectoryPath = Path.Combine(webRootPath, "uploads");

// Create uploads directories if they don't exist
if (!Directory.Exists(uploadsDirectoryPath))
{
    Directory.CreateDirectory(uploadsDirectoryPath);
    Directory.CreateDirectory(Path.Combine(uploadsDirectoryPath, "faces"));
    Directory.CreateDirectory(Path.Combine(uploadsDirectoryPath, "documents"));
    Console.WriteLine($"📁 Created uploads directories");
}

app.UseStaticFiles();

// Directory browser for uploads in development
if (app.Environment.IsDevelopment() && Directory.Exists(uploadsDirectoryPath))
{
    app.UseDirectoryBrowser(new DirectoryBrowserOptions
    {
        FileProvider = new PhysicalFileProvider(uploadsDirectoryPath),
        RequestPath = "/uploads"
    });
}

// CORS
app.UseCors("AllowAll");

// Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Database initialization
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    try
    {
        Console.WriteLine("🔄 Checking database connection...");
        
        // Test connection
        if (await context.Database.CanConnectAsync())
        {
            Console.WriteLine("✅ Database connection successful!");
        }
        else
        {
            throw new Exception("Cannot connect to database");
        }

        // Apply pending migrations first
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            Console.WriteLine("🔄 Applying pending migrations...");
            await context.Database.MigrateAsync();
            Console.WriteLine("✅ Migrations applied successfully!");
        }
        else
        {
            Console.WriteLine("ℹ️ No pending migrations");
        }

        // Seed admin user
        await SeedAdminUser(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database error: {ex.Message}");
        
        try
        {
            Console.WriteLine("🔄 Attempting to recreate database...");
            await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();
            Console.WriteLine("✅ Database recreated successfully!");
            
            // Seed admin user after recreation
            await SeedAdminUser(context);
        }
        catch (Exception recreateEx)
        {
            Console.WriteLine($"❌ Failed to recreate database: {recreateEx.Message}");
        }
    }
}

Console.WriteLine("🚀 Smart Attendance API is running!");
Console.WriteLine($"🌐 Swagger UI: https://localhost:7001");
Console.WriteLine($"🌐 HTTP: http://localhost:5001");

app.Run();

// Helper method for seeding admin user
async Task SeedAdminUser(ApplicationDbContext context)
{
    try
    {
        var adminExists = await context.Users.AnyAsync(u => !u.IsDeleted && u.Email == "admin@bologna.edu.iq");
        
        if (!adminExists)
        {
            var adminUser = new User
            {
                Email = "admin@bologna.edu.iq",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin2024!"),
                UserType = "Admin",
                IsActive = true,
                EmailVerified = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();
            
            Console.WriteLine("👤 Admin user created:");
            Console.WriteLine("   📧 Email: admin@bologna.edu.iq");
            Console.WriteLine("   🔑 Password: Admin2024!");
        }
        else
        {
            Console.WriteLine("ℹ️ Admin user already exists");
        }
        
        Console.WriteLine("✅ Initial data seeding completed!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Seeding error: {ex.Message}");
    }
}
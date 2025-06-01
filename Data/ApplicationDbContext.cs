using Microsoft.EntityFrameworkCore;
using SmartAttendance.API.Models.Entities;

namespace SmartAttendance.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(e => e.UserType).IsRequired().HasMaxLength(20);
                
                // Remove default value constraints
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
            });

            // Student configuration
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.StudentCode).IsUnique();
                entity.Property(e => e.StudentCode).HasMaxLength(20);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Stage).IsRequired().HasMaxLength(20);
                entity.Property(e => e.StudyType).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Section).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.ProfileImage).HasMaxLength(255);
                
                // Foreign key
                entity.HasOne(e => e.User)
                      .WithOne(u => u.Student)
                      .HasForeignKey<Student>(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Professor configuration  
            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.EmployeeCode).IsUnique();
                entity.Property(e => e.EmployeeCode).HasMaxLength(20);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Department).HasMaxLength(100);
                entity.Property(e => e.Title).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.Bio).HasMaxLength(500);
                entity.Property(e => e.ProfileImage).HasMaxLength(255);
                
                // Foreign key
                entity.HasOne(e => e.User)
                      .WithOne(u => u.Professor)
                      .HasForeignKey<Professor>(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Subject configuration
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Stage).IsRequired().HasMaxLength(20);
                entity.Property(e => e.StudyType).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            // CourseAssignment configuration
            modelBuilder.Entity<CourseAssignment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Section).IsRequired().HasMaxLength(5);
                entity.Property(e => e.AcademicYear).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Semester).IsRequired().HasMaxLength(20);
                
                // Foreign keys
                entity.HasOne(e => e.Subject)
                      .WithMany()
                      .HasForeignKey(e => e.SubjectId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(e => e.Professor)
                      .WithMany()
                      .HasForeignKey(e => e.ProfessorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Session configuration
            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SessionDate).IsRequired();
                entity.Property(e => e.StartTime).IsRequired();
                entity.Property(e => e.EndTime).IsRequired();
                entity.Property(e => e.Status).HasMaxLength(20);
                entity.Property(e => e.Notes).HasMaxLength(500);
                
                // Foreign key
                entity.HasOne(e => e.CourseAssignment)
                      .WithMany(ca => ca.Sessions)
                      .HasForeignKey(e => e.CourseAssignmentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // AttendanceRecord configuration
            modelBuilder.Entity<AttendanceRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AttendanceStatus).IsRequired().HasMaxLength(20);
                entity.Property(e => e.DetectionMethod).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Notes).HasMaxLength(500);
                
                // Foreign keys
                entity.HasOne(e => e.Student)
                      .WithMany()
                      .HasForeignKey(e => e.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(e => e.Session)
                      .WithMany(s => s.AttendanceRecords)
                      .HasForeignKey(e => e.SessionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Models.BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entity = (Models.BaseEntity)entityEntry.Entity;
                
                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
                
                entity.UpdatedAt = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
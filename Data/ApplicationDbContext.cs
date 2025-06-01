using Microsoft.EntityFrameworkCore;
using SmartAttendance.API.Models.Entities;
using System.Linq.Expressions;
namespace SmartAttendance.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
                {
                    base.OnModelCreating(modelBuilder);
                    
                    // Apply configurations
                    ApplyEntityConfigurations(modelBuilder);
                }

        private void ApplyEntityConfigurations(ModelBuilder modelBuilder)
        {
            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(e => e.UserType).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
            });

            // Student Configuration
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StudentCode).HasMaxLength(20);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Stage).IsRequired().HasMaxLength(20);
                entity.Property(e => e.StudyType).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Section).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.ProfileImage).HasMaxLength(255);
                entity.Property(e => e.FaceEncodingData).HasColumnType("LONGTEXT");
                
                // Index for faster searches
                entity.HasIndex(e => e.StudentCode).IsUnique();
                entity.HasIndex(e => new { e.Stage, e.StudyType, e.Section });
                
                // Relationship with User
                entity.HasOne(s => s.User)
                      .WithOne(u => u.Student)
                      .HasForeignKey<Student>(s => s.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Professor Configuration
            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.EmployeeCode).HasMaxLength(20);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Department).HasMaxLength(100);
                entity.Property(e => e.Title).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.Bio).HasMaxLength(500);
                entity.Property(e => e.ProfileImage).HasMaxLength(255);
                
                // Index
                entity.HasIndex(e => e.EmployeeCode).IsUnique();
                
                // Relationship with User
                entity.HasOne(p => p.User)
                      .WithOne(u => u.Professor)
                      .HasForeignKey<Professor>(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Subject Configuration
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Stage).IsRequired().HasMaxLength(20);
                entity.Property(e => e.StudyType).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Description).HasMaxLength(500);
                
                // Index
                entity.HasIndex(e => e.Code).IsUnique();
                entity.HasIndex(e => new { e.Stage, e.StudyType });
            });

            // CourseAssignment Configuration
            modelBuilder.Entity<CourseAssignment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Section).IsRequired().HasMaxLength(5);
                entity.Property(e => e.AcademicYear).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Semester).IsRequired().HasMaxLength(20);
                
                // Relationships
                entity.HasOne(ca => ca.Professor)
                      .WithMany(p => p.CourseAssignments)
                      .HasForeignKey(ca => ca.ProfessorId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(ca => ca.Subject)
                      .WithMany(s => s.CourseAssignments)
                      .HasForeignKey(ca => ca.SubjectId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                // Index for unique constraint
                entity.HasIndex(e => new { e.ProfessorId, e.SubjectId, e.Section, e.AcademicYear, e.Semester })
                      .IsUnique();
            });

            // Session Configuration
            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("Scheduled");
                entity.Property(e => e.Notes).HasMaxLength(500);
                
                // Relationship
                entity.HasOne(s => s.CourseAssignment)
                      .WithMany(ca => ca.Sessions)
                      .HasForeignKey(s => s.CourseAssignmentId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                // Index
                entity.HasIndex(e => new { e.CourseAssignmentId, e.SessionDate });
            });

            // AttendanceRecord Configuration
            modelBuilder.Entity<AttendanceRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AttendanceStatus).IsRequired().HasMaxLength(20).HasDefaultValue("Absent");
                entity.Property(e => e.DetectionMethod).IsRequired().HasMaxLength(20).HasDefaultValue("Manual");
                entity.Property(e => e.Notes).HasMaxLength(500);
                
                // Relationships
                entity.HasOne(ar => ar.Session)
                      .WithMany(s => s.AttendanceRecords)
                      .HasForeignKey(ar => ar.SessionId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(ar => ar.Student)
                      .WithMany(s => s.AttendanceRecords)
                      .HasForeignKey(ar => ar.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                // Index for unique constraint (one record per student per session)
                entity.HasIndex(e => new { e.SessionId, e.StudentId }).IsUnique();
            });
        }

        private void ApplyGlobalQueryFilters(ModelBuilder modelBuilder)
        {
            // Apply soft delete filter to all entities that inherit from BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(GetSoftDeleteFilter(entityType.ClrType));
                }
            }
        }

        private static System.Linq.Expressions.LambdaExpression GetSoftDeleteFilter(Type entityType)
        {
            var parameter = System.Linq.Expressions.Expression.Parameter(entityType, "e");
            var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
            var condition = Expression.Equal(property, Expression.Constant(false));
            return System.Linq.Expressions.Expression.Lambda(condition, parameter);
        }

        // Override SaveChanges to automatically set audit fields
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}
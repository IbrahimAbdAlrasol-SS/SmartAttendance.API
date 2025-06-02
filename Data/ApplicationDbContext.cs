using Microsoft.EntityFrameworkCore;
using SmartAttendance.API.Models.Entities;

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
        public DbSet<QrSession> QrSessions { get; set; }
        public DbSet<QrUsageLog> QrUsageLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configurations
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.UserType).IsRequired().HasMaxLength(20);
                
                // تحديد نوع البيانات للتواريخ
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            // Student configurations
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasOne(s => s.User)
                      .WithOne(u => u.Student)
                      .HasForeignKey<Student>(s => s.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasIndex(s => s.StudentCode).IsUnique();
                entity.Property(s => s.FullName).IsRequired().HasMaxLength(100);
                
                // تحديد نوع البيانات للتواريخ
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            // Professor configurations
            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasOne(p => p.User)
                      .WithOne(u => u.Professor)
                      .HasForeignKey<Professor>(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasIndex(p => p.EmployeeCode).IsUnique();
                entity.Property(p => p.FullName).IsRequired().HasMaxLength(100);
                
                // تحديد نوع البيانات للتواريخ
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            // Subject configurations
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasIndex(s => s.Code).IsUnique();
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Code).IsRequired().HasMaxLength(20);
                
                // تحديد نوع البيانات للتواريخ
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            // CourseAssignment configurations
            modelBuilder.Entity<CourseAssignment>(entity =>
            {
                entity.HasOne(ca => ca.Professor)
                      .WithMany(p => p.CourseAssignments)
                      .HasForeignKey(ca => ca.ProfessorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ca => ca.Subject)
                      .WithMany(s => s.CourseAssignments)
                      .HasForeignKey(ca => ca.SubjectId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                // تحديد نوع البيانات للتواريخ
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            // Session configurations
            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasOne(s => s.CourseAssignment)
                      .WithMany(ca => ca.Sessions)
                      .HasForeignKey(s => s.CourseAssignmentId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                // تحديد نوع البيانات للتواريخ
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            // AttendanceRecord configurations
            modelBuilder.Entity<AttendanceRecord>(entity =>
            {
                entity.HasOne(ar => ar.Session)
                      .WithMany(s => s.AttendanceRecords)
                      .HasForeignKey(ar => ar.SessionId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ar => ar.Student)
                      .WithMany(s => s.AttendanceRecords)
                      .HasForeignKey(ar => ar.StudentId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Unique constraint: one attendance record per student per session
                entity.HasIndex(ar => new { ar.SessionId, ar.StudentId }).IsUnique();
                
                // تحديد نوع البيانات للتواريخ
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            // Global query filters for soft delete
            modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Student>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Professor>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Subject>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<CourseAssignment>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Session>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<AttendanceRecord>().HasQueryFilter(e => !e.IsDeleted);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entity = (BaseEntity)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.App.Models;
using Elfie.Serialization;

namespace ContosoUniversity.App.Data
{
    public class ContosoUniContextDb : DbContext
    {
        public ContosoUniContextDb(DbContextOptions<ContosoUniContextDb> options)
            : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Enrollment>()
                .HasKey(e => new { e.EnrollmentId, e.CourseId, e.StudentId });
        }
        // Add any additional configuration here if needed
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = ContosoUniDB.db");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;

namespace SchoolManagement.Data
{
    public class SchoolDbContext : DbContext
    {
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Pupil> Pupils { get; set; }
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
            : base(options)
        {
        }
        public SchoolDbContext()
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Pupils)
                .WithMany(p => p.Teachers)
                .UsingEntity<Dictionary<string, object>>(
                    "TeacherPupil",
                    j => j.HasOne<Pupil>().WithMany().HasForeignKey("PupilId"),
                    j => j.HasOne<Teacher>().WithMany().HasForeignKey("TeacherId"),
                    j =>
                    {
                        j.HasKey("TeacherId", "PupilId");
                        j.ToTable("TeacherPupil");
                    });
        }
    }

}
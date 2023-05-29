using DIPTERV.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace DIPTERV.Context
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TimeBlock> TimeBlocks { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<SubjectDivision> SubjectDivisions { get; set; }
        public DbSet<SchoolClass> SchoolClasses { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Course> Courses { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Teacher>()
                .HasMany(e => e.FreeBlocks)
                .WithMany();

            modelBuilder.Entity<TimeBlock>()
                .HasIndex(tb => new { tb.Day, tb.LessonNumber })
                .IsUnique();

            modelBuilder.Entity<Course>()
                .HasOne(x => x.Room)
                .WithMany()
                .HasForeignKey(x=> x.RoomId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            modelBuilder.Entity<Course>()
            .HasOne(x => x.SubjectDivision)
                .WithMany()
                .HasForeignKey(x => x.SubjectDivisinId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            modelBuilder.Entity<Course>()
            .HasOne(x => x.TimeBlock)
                .WithMany()
                .HasForeignKey(x => x.TimeBlockId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            modelBuilder.Entity <SchoolClass>()
            .HasOne(x => x.HeadMaster)
                        .WithMany()
                        .HasForeignKey(x => x.HeadMasterId)
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

            modelBuilder.Entity < SubjectDivision>()
                .HasOne(x => x.SchoolClass)
                        .WithMany()
                        .HasForeignKey(x => x.SchoolClassId)
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

            modelBuilder.Entity<SubjectDivision>()
            .HasOne(x => x.Teacher)
                .WithMany()
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();


        }
        
    }
}
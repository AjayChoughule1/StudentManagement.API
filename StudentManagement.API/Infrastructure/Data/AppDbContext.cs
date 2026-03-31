using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace StudentManagement.API.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<ClassRoom> ClassRooms => Set<ClassRoom>();
        public DbSet<Schedule> Schedules => Set<Schedule>();
        public DbSet<Attendance> Attendances => Set<Attendance>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            // ── Student ──────────────────────────────────────────────
            mb.Entity<Student>(e =>
            {
                e.HasKey(s => s.Id);
                e.Property(s => s.FirstName).IsRequired().HasMaxLength(100);
                e.Property(s => s.LastName).IsRequired().HasMaxLength(100);
                e.Property(s => s.RollNumber).IsRequired().HasMaxLength(20);
                e.HasIndex(s => s.RollNumber).IsUnique();
                e.Property(s => s.Email).HasMaxLength(200);
                e.Property(s => s.Phone).HasMaxLength(20);
                e.Property(s => s.Gender).HasMaxLength(10);
                e.Property(s => s.Address).HasMaxLength(500);

                e.HasOne(s => s.ClassRoom)
                 .WithMany(c => c.Students)
                 .HasForeignKey(s => s.ClassRoomId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Teacher ──────────────────────────────────────────────
            mb.Entity<Teacher>(e =>
            {
                e.HasKey(t => t.Id);
                e.Property(t => t.FirstName).IsRequired().HasMaxLength(100);
                e.Property(t => t.LastName).IsRequired().HasMaxLength(100);
                e.Property(t => t.Email).IsRequired().HasMaxLength(200);
                e.HasIndex(t => t.Email).IsUnique();
                e.Property(t => t.Phone).HasMaxLength(20);
                e.Property(t => t.Qualification).HasMaxLength(300);
            });

            // ── ClassRoom ────────────────────────────────────────────
            mb.Entity<ClassRoom>(e =>
            {
                e.HasKey(c => c.Id);
                e.Property(c => c.Name).IsRequired().HasMaxLength(100);
                e.Property(c => c.Section).IsRequired().HasMaxLength(10);
                e.Property(c => c.Description).HasMaxLength(500);
            });

            // ── Schedule ─────────────────────────────────────────────
            mb.Entity<Schedule>(e =>
            {
                e.HasKey(s => s.Id);
                e.Property(s => s.Subject).IsRequired().HasMaxLength(150);

                e.HasOne(s => s.Teacher)
                 .WithMany(t => t.Schedules)
                 .HasForeignKey(s => s.TeacherId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(s => s.ClassRoom)
                 .WithMany(c => c.Schedules)
                 .HasForeignKey(s => s.ClassRoomId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Attendance ───────────────────────────────────────────
            mb.Entity<Attendance>(e =>
            {
                e.HasKey(a => a.Id);
                e.Property(a => a.Remarks).HasMaxLength(500);
                e.Property(a => a.Status).HasConversion<string>();

                // Unique: one attendance record per student per day
                e.HasIndex(a => new { a.StudentId, a.Date }).IsUnique();

                e.HasOne(a => a.Student)
                 .WithMany(s => s.Attendances)
                 .HasForeignKey(a => a.StudentId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(a => a.ClassRoom)
                 .WithMany()
                 .HasForeignKey(a => a.ClassRoomId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(a => a.MarkedByTeacher)
                 .WithMany()
                 .HasForeignKey(a => a.MarkedByTeacherId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── User ─────────────────────────────────────────────────
            mb.Entity<User>(e =>
            {
                e.HasKey(u => u.Id);
                e.Property(u => u.Username).IsRequired().HasMaxLength(100);
                e.Property(u => u.Email).IsRequired().HasMaxLength(200);
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.PasswordHash).IsRequired();
                e.Property(u => u.Role).HasConversion<string>();

                e.HasOne(u => u.Teacher)
                 .WithOne(t => t.User)
                 .HasForeignKey<User>(u => u.TeacherId)
                 .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}

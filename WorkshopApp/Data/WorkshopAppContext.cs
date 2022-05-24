using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkshopApp.Areas.Identity.Data;
using WorkshopApp.Models;

namespace WorkshopApp.Data
{
    public class WorkshopAppContext : IdentityDbContext<WorkshopAppUser>
    {
        public WorkshopAppContext(DbContextOptions<WorkshopAppContext> options)
            : base(options)
        {
        }

        public DbSet<WorkshopApp.Models.Student> Student { get; set; }

        public DbSet<WorkshopApp.Models.Teacher> Teacher { get; set; }

        public DbSet<WorkshopApp.Models.Course> Course { get; set; }

        public DbSet<WorkshopApp.Models.Course> FirstCourses { get; set; }

        public DbSet<WorkshopApp.Models.Course> SecondCourses { get; set; }

        public DbSet<WorkshopApp.Models.Enrollment> Enrollment { get; set; }

       
        
           
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Enrollment>()
            .HasOne<Student>(p => p.Student)
            .WithMany(p => p.Courses)
            .HasForeignKey(p => p.StudentId);

            builder.Entity<Enrollment>()
            .HasOne<Course>(p => p.Course)
            .WithMany(p => p.Students)
            .HasForeignKey(p => p.CourseId);


            builder.Entity<Course>()
           .HasOne<Teacher>(p => p.FirstTeacher)
           .WithMany(p => p.FirstCourses)
           .HasForeignKey(p => p.FirstTeacherId)
           .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Course>()
           .HasOne<Teacher>(p => p.SecondTeacher)
           .WithMany(p => p.SecondCourses)
           .HasForeignKey(p => p.SecondTeacherId)
           .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(builder);
        }
    }
}

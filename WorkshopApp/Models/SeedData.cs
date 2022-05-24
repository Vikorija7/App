 using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkshopApp.Areas.Identity.Data;
using WorkshopApp.Data;

namespace WorkshopApp.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<WorkshopAppUser>>();
            IdentityResult roleResult;
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }
            roleCheck = await RoleManager.RoleExistsAsync("Student");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Student"));
            }
            roleCheck = await RoleManager.RoleExistsAsync("Teacher");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Teacher"));
            }

            WorkshopAppUser user = await UserManager.FindByEmailAsync("admin@workshopapp.com");
            if (user == null)
            {
                var User = new WorkshopAppUser();
                User.Email = "admin@workshopapp.com";
                User.UserName = "admin@workshopapp.com";
                User.Role = "Admin";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin      
                if (chkUser.Succeeded)
                {
                    var result1 = await UserManager.AddToRoleAsync(User, "Admin");
                }
            }
        }

            public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new WorkshopAppContext(
            serviceProvider.GetRequiredService<
            DbContextOptions<WorkshopAppContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();
                // Look for any movies.
                if (context.Teacher.Any() || context.Student.Any() || context.Course.Any())
                {
                    return; // DB has been seeded
                }
                context.Student.AddRange(
            new Student { StudentId = "170/2018", FirstName = "Димитар", LastName = "Наумоски", EnrollmentDate = DateTime.Parse("2018-09-01"), AcquiredCredits = 144, CurrentSemestar = 6, EducationLevel = "Додипломски" },
            new Student { StudentId = "321/2018", FirstName = "Петар", LastName = "Петровски", EnrollmentDate = DateTime.Parse("2018-09-01"), AcquiredCredits = 144, CurrentSemestar = 6, EducationLevel = "Додипломски" },
            new Student { StudentId = "328/2018", FirstName = "Стефан", LastName = "Кузманоски", EnrollmentDate = DateTime.Parse("2018-09-01"), AcquiredCredits = 126, CurrentSemestar = 6, EducationLevel = "Додипломски" },
            new Student { StudentId = "369/2018", FirstName = "Ангела", LastName = "Иванова", EnrollmentDate = DateTime.Parse("2018-09-01"), AcquiredCredits = 144, CurrentSemestar = 6, EducationLevel = "Додипломски" },
            new Student { StudentId = "322/2018", FirstName = "Анастасија", LastName = "Димитриевска", EnrollmentDate = DateTime.Parse("2018-09-01"), AcquiredCredits = 132, CurrentSemestar = 6, EducationLevel = "Додипломски" },
            new Student { StudentId = "400/2018", FirstName = "Михаил", LastName = "Цветковски", EnrollmentDate = DateTime.Parse("2018-09-01"), AcquiredCredits = 120, CurrentSemestar = 6, EducationLevel = "Додипломски" }



                );
                context.SaveChanges();
                context.Teacher.AddRange(
             
             new Teacher { FirstName = "Перо", LastName = "Латкоски", Degree = "Доктор на науки", AcademicRank = "Редовен професор", OfficeNumber = "ТК", HireDate = DateTime.Parse("2006-01-01") },
             new Teacher { FirstName = "Валентин", LastName = "Ракович", Degree = "Доктор на науки", AcademicRank = "Доцент", OfficeNumber = "ТК", HireDate = DateTime.Parse("2017-01-01") },
             new Teacher { FirstName = "Владимир", LastName = "Атанасовски", Degree = "Доктор на науки", AcademicRank = "Доцент", OfficeNumber = "ТК", HireDate = DateTime.Parse("2015-01-01") },
             new Teacher { FirstName = "Борислав", LastName = "Попоски", Degree = "Доктор на науки", AcademicRank = "Редовен професор", OfficeNumber = "ТК", HireDate = DateTime.Parse("2010-01-01") },
             new Teacher { FirstName = "Даниел", LastName = "Денковски", Degree = "Доктор на науки", AcademicRank = "Доцент", OfficeNumber = "121b", HireDate = DateTime.Parse("2017-01-01") }

                );
                context.SaveChanges();
                context.Course.AddRange(
                new Course
                {
                    Title = "Развој на серверски WEB апликации",
                    Credits = 6,
                    Semester = 6,
                    Programme = "КТИ,ТКИИ",
                    EducationLevel = "Додипломски",
                    FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Даниел" && d.LastName == "Денковски").Id,
                    SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Перо" && d.LastName == "Латкоски").Id
                },
            new Course
            {
                Title = "Основи на WEB програмирање",
                Credits = 6,
                Semester = 5,
                Programme = "КТИ,ТКИИ",
                EducationLevel = "Додипломски",
                FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Владимир" && d.LastName == "Атанасовски").Id,
                SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Валентин" && d.LastName == "Ракович").Id
            },
            new Course
            {
                Title = "Комуникациски технологии",
                Credits = 6,
                Semester = 5,
                Programme = "КТИ",
                EducationLevel = "Додипломски",
                FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Перо" && d.LastName == "Латкоски").Id,
                SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Борислав" && d.LastName == "Попоски").Id
            }

                );
                context.SaveChanges();
                context.Enrollment.AddRange
                (
            new Enrollment { StudentId = 1, CourseId = 1, Semester = "6", Year = 2021, Grade = 10, SeminalUrl = "", ProjectUrl = "", ExamPoints = 100, SeminalPoints = 0, ProjectPoints = 100, AdditionalPoints = 0, FinishDate = DateTime.Parse("2021-06-05") },
            new Enrollment { StudentId = 1, CourseId = 2, Semester = "6", Year = 2021, Grade = 8, SeminalUrl = "", ProjectUrl = "", ExamPoints = 78, SeminalPoints = 0, ProjectPoints = 100, AdditionalPoints = 0, FinishDate = DateTime.Parse("2021-06-05") },
            new Enrollment { StudentId = 1, CourseId = 3, Semester = "6", Year = 2021, Grade = 10, SeminalUrl = "", ProjectUrl = "", ExamPoints = 100, SeminalPoints = 90, ProjectPoints = 100, AdditionalPoints = 0, FinishDate = DateTime.Parse("2021-06-05") },
            new Enrollment { StudentId = 2, CourseId = 1, Semester = "6", Year = 2021, Grade = 10, SeminalUrl = "", ProjectUrl = "", ExamPoints = 100, SeminalPoints = 0, ProjectPoints = 100, AdditionalPoints = 0, FinishDate = DateTime.Parse("2021-05-05") },
            new Enrollment { StudentId = 2, CourseId = 2, Semester = "6", Year = 2021, Grade = 6, SeminalUrl = "", ProjectUrl = "", ExamPoints = 53, SeminalPoints = 0, ProjectPoints = 0, AdditionalPoints = 0, FinishDate = DateTime.Parse("2021-06-05") },
            new Enrollment { StudentId = 3, CourseId = 3, Semester = "6", Year = 2021, Grade = 10, SeminalUrl = "", ProjectUrl = "", ExamPoints = 100, SeminalPoints = 64, ProjectPoints = 100, AdditionalPoints = 0, FinishDate = DateTime.Parse("2021-10-05") },
            new Enrollment { StudentId = 3, CourseId = 1, Semester = "6", Year = 2021, Grade = 9, SeminalUrl = "", ProjectUrl = "", ExamPoints = 70, SeminalPoints = 100, ProjectPoints = 100, AdditionalPoints = 0, FinishDate = DateTime.Parse("2021-06-05") },
            new Enrollment { StudentId = 3, CourseId = 2, Semester = "6", Year = 2021, Grade = 10, SeminalUrl = "", ProjectUrl = "", ExamPoints = 100, SeminalPoints = 0, ProjectPoints = 100, AdditionalPoints = 5, FinishDate = DateTime.Parse("2021-06-05") },
            new Enrollment { StudentId = 4, CourseId = 1, Semester = "6", Year = 2021, Grade = 10, SeminalUrl = "", ProjectUrl = "", ExamPoints = 100, SeminalPoints = 0, ProjectPoints = 100, AdditionalPoints = 0, FinishDate = DateTime.Parse("2021-06-05") },
            new Enrollment { StudentId = 5, CourseId = 2, Semester = "6", Year = 2021, Grade = 10, SeminalUrl = "", ProjectUrl = "", ExamPoints = 100, SeminalPoints = 0, ProjectPoints = 50, AdditionalPoints = 0, FinishDate = DateTime.Parse("2021-06-05") },
            new Enrollment { StudentId = 6, CourseId = 1, Semester = "6", Year = 2021, Grade = 7, SeminalUrl = "", ProjectUrl = "", ExamPoints = 68, SeminalPoints = 0, ProjectPoints = 100, AdditionalPoints = 0, FinishDate = DateTime.Parse("2021-06-05") },
           new Enrollment { StudentId = 6, CourseId = 2, Semester = "6", Year = 2021, Grade = 10, SeminalUrl = "", ProjectUrl = "", ExamPoints = 100, SeminalPoints = 0, ProjectPoints = 100, AdditionalPoints = 0, FinishDate = DateTime.Parse("2021-06-05") }

                );
                context.SaveChanges();

            }


        }
    }
}

    


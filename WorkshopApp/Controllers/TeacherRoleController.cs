using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkshopApp.Areas.Identity.Data;
using WorkshopApp.Data;
using WorkshopApp.Models;
using WorkshopApp.ViewModels;

namespace WorkshopApp.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherRoleController : Controller
    {
        private readonly WorkshopAppContext _context;
        private readonly UserManager<WorkshopAppUser> userManager;

        public TeacherRoleController(WorkshopAppContext context, UserManager<WorkshopAppUser> userMgr)
        {
            _context = context;
            userManager = userMgr;
        }

        public async Task<IActionResult> Teachercourses(int? id, string SearchTitle, string CourseSemester, string CourseProgram)
        {
            IQueryable<Course> courses = _context.Course.AsQueryable();
            IQueryable<int> semesterQuery = _context.Course.OrderBy(c => c.Semester).Select(m => m.Semester).Distinct();
            IQueryable<string> programQuery = _context.Course.OrderBy(c => c.Programme).Select(g => g.Programme).Distinct();

            if (!string.IsNullOrEmpty(SearchTitle))
            {
                courses = courses.Where(t => t.Title.Contains(SearchTitle));
            }

            if (!string.IsNullOrEmpty(CourseProgram))
            {
                courses = courses.Where(s => s.Programme == CourseProgram);
            }

            if (!string.IsNullOrEmpty(CourseSemester))
            {
                courses = courses.Where(s => s.Semester.ToString().Equals(CourseSemester));
            }

            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .Include(c => c.FirstCourses)
                  .Include(d => d.SecondCourses)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            WorkshopAppUser user = await userManager.GetUserAsync(User);
            if (id != user.TeacherId)
            {
                return RedirectToAction("AccessDenied", "Account", null);
            }

            CourseFilterViewModel viewmodel = new CourseFilterViewModel
            {
                Teacher = teacher,
                Courses = await courses.ToListAsync(),
                ProgramList = new SelectList(await programQuery.ToListAsync()),
                SemesterList = new SelectList(await semesterQuery.ToListAsync())
            };
            //return View(teacher);
            return View(viewmodel);
        }


        public async Task<IActionResult> EnrollmentsCourse(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher)
                .Include(c => c.Students)
                .ThenInclude(e => e.Student)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
            {
                return NotFound();
            }
            WorkshopAppUser user = await userManager.GetUserAsync(User);
            if (id != user.TeacherId)
            {
                return RedirectToAction("AccessDenied", "Account", null);
            }

            return View(course);
        }

        public async Task<IActionResult> TeacherEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["courseID"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["studentID"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }

        // POST: Enrollments/TeacherEdit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public async Task<IActionResult> TeacherEdit(int id, [Bind("Id,CourseId,StudentId,Semester,Year,Grade,SeminalUrl,ProjectUrl,ExamPoints,SeminalPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Teachers");
            }
            ViewData["courseID"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["studentID"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            WorkshopAppUser user = await userManager.GetUserAsync(User);
            if (id != user.TeacherId)
            {
                return RedirectToAction("AccessDenied", "Account", null);
            }
            return View(enrollment);
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollment.Any(e => e.Id == id);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkshopApp.Data;
using WorkshopApp.Models;
using WorkshopApp.ViewModels;

namespace WorkshopApp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CoursesController : Controller
    {
        private readonly WorkshopAppContext _context;

        public CoursesController(WorkshopAppContext context)
        {
            _context = context;
        }

      
        // GET: Courses
        public async Task<IActionResult> Index(string SearchTitle, string CourseSemester, string CourseProgram, int? id)
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




            courses = courses.Include(c => c.FirstTeacher).Include(c => c.SecondTeacher).Include(c => c.Students).ThenInclude(c => c.Student);



            var viewmodel = new CourseFilterViewModel
            {
                Courses = await courses.ToListAsync(),
                ProgramList = new SelectList(await programQuery.ToListAsync()),
                SemesterList = new SelectList(await semesterQuery.ToListAsync())


            };

            return View(viewmodel);
        }

        // GET: Courses/Details/5
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher)
                .Include(c => c.Students).ThenInclude(c => c.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
      
        public IActionResult Create()
        {
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "Id");
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "Id");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

 
        public async Task<IActionResult> Create([Bind("Id,Title,Credits,Semester,Programme,EducationLevel,FirstTeacherId,SecondTeacherId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "Id", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "Id", course.SecondTeacherId);
            return View(course);
        }

        // GET: Courses/Edit/5
       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _context.Course.Where(c => c.Id == id).Include(c => c.Students).First();
            if (course == null)
            {
                return NotFound();
            }
            IEnumerable<Student> students = _context.Student.AsEnumerable();


            CourseStudentsViewModel viewmodel = new CourseStudentsViewModel
            {
                Course = course,
                StudentList = new MultiSelectList(students, "Id", "FullName"),
                SelectedStudents = course.Students.Select(s => s.StudentId)

            };
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.SecondTeacherId);
            return View(viewmodel);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public async Task<IActionResult> Edit(int id, CourseStudentsViewModel viewmodel)
        {
            if (id != viewmodel.Course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.Course);
                    await _context.SaveChangesAsync();
                    IEnumerable<int> listStudents = viewmodel.SelectedStudents;
                    IQueryable<Enrollment> toBeRemoved = _context.Enrollment.Where(s => !listStudents.Contains(s.StudentId) && s.CourseId == id);
                    _context.Enrollment.RemoveRange(toBeRemoved);

                    IEnumerable<int> existStudents = _context.Enrollment.Where(s => listStudents.Contains(s.StudentId) && s.CourseId == id).Select(s => s.StudentId);
                    IEnumerable<int> newStudents = listStudents.Where(s => !listStudents.Contains(s));
                    foreach (int studentid in newStudents)

                        _context.Enrollment.Add(new Enrollment { StudentId = studentid, CourseId = id });


                    await _context.SaveChangesAsync();


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(viewmodel.Course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "Id", viewmodel.Course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "Id", viewmodel.Course.SecondTeacherId);
            return View(viewmodel);
        }

        // GET: Courses/Delete/5
  
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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

            return View(course);
        }
        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }

    }
}



using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    [Authorize(Roles = "Admin")]
    public class TeachersController : Controller
    {
        private readonly WorkshopAppContext _context;
        private readonly IHostingEnvironment webHostEnvironment;
        private readonly UserManager<WorkshopAppUser> userManager;

        public TeachersController(WorkshopAppContext context, IHostingEnvironment hostingEnviroment, UserManager<WorkshopAppUser> userMgr)
        {
            _context = context;
            webHostEnvironment = hostingEnviroment;
            userManager = userMgr;
        }

        // GET: Teachers
       
        public async Task<IActionResult> Index(string SearchFullName, string SearchRank, string SearchDegree)
        {
            IEnumerable<Teacher> teachers = _context.Teacher.AsEnumerable();
            IQueryable<string> rankQuery = _context.Teacher.OrderBy(t => t.AcademicRank).Select(t => t.AcademicRank).Distinct();
            IQueryable<string> degreeQuery = _context.Teacher.OrderBy(c => c.Degree).Select(g => g.Degree).Distinct();

            if (!string.IsNullOrEmpty(SearchFullName))
            {
                teachers = teachers.Where(t => t.FullName.Contains(SearchFullName));
            }

            if (!string.IsNullOrEmpty(SearchRank))
            {
                teachers = teachers.Where(s => s.AcademicRank == SearchRank);
            }

            if (!string.IsNullOrEmpty(SearchDegree))
            {
                teachers = teachers.Where(s => s.Degree.Contains(SearchDegree));
            }

            var viewmodel = new TeacherFilterViewModel
            {
                Teachers = teachers.ToList(),
                Ranks = new SelectList(await rankQuery.ToListAsync()),
                Degrees = new SelectList(await degreeQuery.ToListAsync())


            };

            return View(viewmodel);
        }


        // GET: Teachers/Details/5
      
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
     
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
   
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Edit(int id, IFormFile imageUrl, [Bind("Id,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate,profilePicture")] Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

            TeachersController uploadImage = new TeachersController(_context, webHostEnvironment,userManager);
            teacher.profilePicture = uploadImage.UploadedFile(imageUrl);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = teacher.Id });
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
      
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teacher.FindAsync(id);
            _context.Teacher.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       
        public string UploadedFile(IFormFile file)
        {
            string uniqueFileName = null;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "teacherimages");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
       
        private bool TeacherExists(int id)
        {
            return _context.Teacher.Any(e => e.Id == id);
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


    }

}

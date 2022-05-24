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
    public class StudentsController : Controller
    {
        private readonly WorkshopAppContext _context;
        private readonly IHostingEnvironment webHostingEnvironment;
        private readonly UserManager<WorkshopAppUser> userManager;

        public StudentsController(WorkshopAppContext context, IHostingEnvironment hostingEnvironment, UserManager<WorkshopAppUser> userMgr)
        {
            _context = context;
            webHostingEnvironment = hostingEnvironment;
            userManager = userMgr;
        }

        // GET: Students
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string SearchIndex, string SearchFullName)
        {
            IEnumerable<Student> students = _context.Student.AsEnumerable();
            if(!string.IsNullOrEmpty(SearchIndex))
            {
                students = students.Where(s => s.StudentId.Contains(SearchIndex));
            }
            if (!string.IsNullOrEmpty(SearchFullName))
            {
                students = students.Where(s => s.FullName.Contains(SearchFullName));
            }
           


            var viewmodel = new StudentFilterViewModel
            {
                Students = students.ToList()

            };
            return View(viewmodel);
        }

        // GET: Students/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,StudentId,FirstName,LastName,EnrollmentDate,AcquiredCredits,CurrentSemestar,EducationLevel")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        [Authorize(Roles = "Admin")]
        private string UploadedFile(StudentPicEdit viewmodel)
        {
            string uniqueFileName = null;

            if (viewmodel.Pic != null)
            {
                string uploadsFolder = Path.Combine(webHostingEnvironment.WebRootPath, "StudentImages");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(viewmodel.Pic.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    viewmodel.Pic.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        [Authorize(Roles = "Admin")]
        public string UploadedFile(IFormFile file)
        {
            string uniqueFileName = null;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(webHostingEnvironment.WebRootPath, "StudentImages");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, IFormFile pictureUrl, [Bind("profilePicture,Id,StudentId,FirstName,LastName,EnrollmentDate,AcquiredCredits,CurrentSemestar,EducationLevel")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            StudentsController uploadImage = new StudentsController(_context, webHostingEnvironment,userManager);
            student.profilePicture = uploadImage.UploadedFile(pictureUrl);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = student.Id });
            }
            return View(student);
        }

        // GET: Students/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> CoursesOfStudent(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
            .Include(s => s.Courses)
            .ThenInclude(e => e.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }
            WorkshopAppUser user = await userManager.GetUserAsync(User);
            if (id != user.StudentId)
            {
                return RedirectToAction("AccessDenied", "Account", null);
            }

            return View(student);
        }
        [Authorize(Roles = "Admin")]
        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
    }
}

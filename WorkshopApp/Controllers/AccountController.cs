using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkshopApp.Areas.Identity.Data;
using WorkshopApp.Data;
using WorkshopApp.Models;
using WorkshopApp.ViewModels;

namespace WorkshopApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<WorkshopAppUser> userManager;
        private SignInManager<WorkshopAppUser> signInManager;
        private IPasswordHasher<WorkshopAppUser> passwordHasher;
        private IPasswordValidator<WorkshopAppUser> passwordValidator;
        private IUserValidator<WorkshopAppUser> userValidator;
        private readonly WorkshopAppContext _context;

        public AccountController(UserManager<WorkshopAppUser> userMgr, SignInManager<WorkshopAppUser> signinMgr, IPasswordHasher<WorkshopAppUser> passwordHash, IPasswordValidator<WorkshopAppUser> passwordVal, IUserValidator<WorkshopAppUser>
          userValid, WorkshopAppContext context)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            passwordHasher = passwordHash;
            passwordValidator = passwordVal;
            userValidator = userValid;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            Login login = new Login();
            login.ReturnUrl = returnUrl;
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                WorkshopAppUser user = await userManager.FindByEmailAsync(login.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, login.Password, false, false);
                    if (result.Succeeded)
                    {
                        if ((await userManager.IsInRoleAsync(user, "Admin")))
                        {
                           
                            return RedirectToAction("Index", "Courses", null);
                        }
                        if ((await userManager.IsInRoleAsync(user, "Teacher")))
                        {
                            return RedirectToAction("Teachercourses", "TeacherRole", new { id = user.TeacherId });
                        }
                        if ((await userManager.IsInRoleAsync(user, "Student")))
                        {
                            return RedirectToAction("CoursesOfStudent", "Students", new { id = user.StudentId });
                        }
                    }
                }
              
            }
            return View(login);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", null);
        }

        [Authorize]
        public async Task<IActionResult> UserDetails()
        {
            WorkshopAppUser user = await userManager.GetUserAsync(User);
            string userDetails = user.UserName;
            string role = "Администратор";
            if (user.TeacherId != null)
            {
                Teacher teacher = await _context.Teacher.FindAsync(user.TeacherId);
                userDetails = teacher.FullName + ", " + teacher.AcademicRank;
                role = "Професор";
            }
            else if (user.StudentId != null)
            {
                Student student = await _context.Student.FindAsync(user.StudentId);
                userDetails = student.FullNamePlusIndex;
                role = "Студент";
            }
            RegisteredUserDetails userInfos = new RegisteredUserDetails
            {
                UserDetails = userDetails,
                Role = role,
                Id = user.Id,
                PasswordHash = user.PasswordHash,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
            return View(userInfos);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserInfo(RegisteredUserDetails entry)
        {
            WorkshopAppUser user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(entry.NewPassword))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, user, entry.NewPassword);
                    if (validPass.Succeeded)
                        user.PasswordHash = passwordHasher.HashPassword(user, entry.NewPassword);
                    else
                        Errors(validPass);
                }
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                if (!string.IsNullOrEmpty(entry.NewPassword) && validPass.Succeeded)
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(UserInfo));
                    else
                        Errors(result);
                }
            }
            return View(user);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
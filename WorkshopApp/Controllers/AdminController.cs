using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkshopApp.Areas.Identity.Data;
using WorkshopApp.Data;
using WorkshopApp.Models;

namespace WorkshopApp.Controllers
{
   
        [Authorize(Roles = "Admin")]
        public class AdminController : Controller
        {
            private UserManager<WorkshopAppUser> userManager;
            private IPasswordHasher<WorkshopAppUser> passwordHasher;
            private IPasswordValidator<WorkshopAppUser> passwordValidator;
            private IUserValidator<WorkshopAppUser> userValidator;
            private readonly WorkshopAppContext _context;

            public AdminController(UserManager<WorkshopAppUser> usrMgr, IPasswordHasher<WorkshopAppUser> passwordHash, IPasswordValidator<WorkshopAppUser> passwordVal, IUserValidator<WorkshopAppUser>
    userValid, WorkshopAppContext context)
            {
                userManager = usrMgr;
                passwordHasher = passwordHash;
                passwordValidator = passwordVal;
                userValidator = userValid;
                _context = context;
            }

            public IActionResult Index()
            {
                IEnumerable<WorkshopAppUser> users = userManager.Users.OrderBy(u => u.Email);
                return View(users);
            }

            public IActionResult TeacherView(int id)
            {
                WorkshopAppUser user = userManager.Users.FirstOrDefault(u => u.TeacherId == id);
                Teacher teacher = _context.Teacher.Where(s => s.Id == id).FirstOrDefault();
                if (teacher != null)
                {
                    ViewData["FullName"] = teacher.FullName;
                    ViewData["TeacherId"] = teacher.Id;
                }
                if (user != null)
                    return View(user);
                else
                    return View(null);
            }

            [HttpPost]
            public async Task<IActionResult> TeacherView(int id, string email, string password, string phoneNumber)
            {
               
                WorkshopAppUser user = userManager.Users.FirstOrDefault(u => u.TeacherId == id);
                if (user != null)
                {
                    IdentityResult validUser = null;
                    IdentityResult validPass = null;

                    user.Email = email;
                     user.PhoneNumber = phoneNumber;
                      user.UserName = email;
                    

                    if (string.IsNullOrEmpty(email))
                        ModelState.AddModelError("", "Внесете Email");

                    validUser = await userValidator.ValidateAsync(userManager, user);
                    if (!validUser.Succeeded)
                        Errors(validUser);

                    if (!string.IsNullOrEmpty(password))
                    {
                        validPass = await passwordValidator.ValidateAsync(userManager, user, password);
                        if (validPass.Succeeded)
                            user.PasswordHash = passwordHasher.HashPassword(user, password);
                        else
                            Errors(validPass);
                    }

                    if (validUser != null && validUser.Succeeded && (string.IsNullOrEmpty(password) || validPass.Succeeded))
                    {
                        IdentityResult result = await userManager.UpdateAsync(user);
                        if (result.Succeeded)
                            return RedirectToAction(nameof(TeacherView), new { id });
                        else
                            Errors(result);
                    }
                }
                else
                {
                    WorkshopAppUser newuser = new WorkshopAppUser();
                    IdentityResult validUser = null;
                    IdentityResult validPass = null;

                    newuser.Email = email;
                    newuser.UserName = email;
                    newuser.PhoneNumber = phoneNumber;
                    newuser.TeacherId = id;
                    newuser.Role = "Teacher";

                    if (string.IsNullOrEmpty(email))
                        ModelState.AddModelError("", "Внесете Email");

                    validUser = await userValidator.ValidateAsync(userManager, newuser);
                    if (!validUser.Succeeded)
                        Errors(validUser);

                    if (!string.IsNullOrEmpty(password))
                    {
                        validPass = await passwordValidator.ValidateAsync(userManager, newuser, password);
                        if (validPass.Succeeded)
                            newuser.PasswordHash = passwordHasher.HashPassword(newuser, password);
                        else
                            Errors(validPass);
                    }
                    else
                        ModelState.AddModelError("", "Внесете лозинка");

                    if (validUser != null && validUser.Succeeded && validPass != null && validPass.Succeeded)
                    {
                        IdentityResult result = await userManager.CreateAsync(newuser, password);
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(newuser, "Teacher");
                            return RedirectToAction(nameof(TeacherView), new { id });
                        }
                        else
                            Errors(result);
                    }
                    user = newuser;
                }
                Teacher teacher = _context.Teacher.Where(s => s.Id == id).FirstOrDefault();
                if (teacher != null)
                {
                    ViewData["FullName"] = teacher.FullName;
                    ViewData["TeacherId"] = teacher.Id;
                }
                return View(user);
            }

            public IActionResult StudentView(int id)
            {
                
                WorkshopAppUser user = userManager.Users.FirstOrDefault(u => u.StudentId == id);
                Student student = _context.Student.Where(s => s.Id == id).FirstOrDefault();
                if (student != null)
                {
                    ViewData["FullName"] = student.FullName;
                    ViewData["StudentId"] = student.Id;
                }
                if (user != null)
                    return View(user);
                else
                    return View(null);
            }

            [HttpPost]
            public async Task<IActionResult> StudentView(int id, string email, string password, string phoneNumber)
            {
               
                WorkshopAppUser user = userManager.Users.FirstOrDefault(u => u.StudentId == id);
                if (user != null)
                {
                    IdentityResult validUser = null;
                    IdentityResult validPass = null;

                    user.Email = email;
                    user.UserName = email;
                    user.PhoneNumber = phoneNumber;

                    if (string.IsNullOrEmpty(email))
                        ModelState.AddModelError("", "Внесете Email");

                    validUser = await userValidator.ValidateAsync(userManager, user);
                    if (!validUser.Succeeded)
                        Errors(validUser);

                    if (!string.IsNullOrEmpty(password))
                    {
                        validPass = await passwordValidator.ValidateAsync(userManager, user, password);
                        if (validPass.Succeeded)
                            user.PasswordHash = passwordHasher.HashPassword(user, password);
                        else
                            Errors(validPass);
                    }

                    if (validUser != null && validUser.Succeeded && (string.IsNullOrEmpty(password) || validPass.Succeeded))
                    {
                        IdentityResult result = await userManager.UpdateAsync(user);
                        if (result.Succeeded)
                            return RedirectToAction(nameof(StudentView), new { id });
                        else
                            Errors(result);
                    }
                }
                else
                {
                    WorkshopAppUser newuser = new WorkshopAppUser();
                    IdentityResult validUser = null;
                    IdentityResult validPass = null;

                    newuser.Email = email;
                    newuser.UserName = email;
                    newuser.PhoneNumber = phoneNumber;
                    newuser.StudentId = id;
                    newuser.Role = "Student";

                    if (string.IsNullOrEmpty(email))
                        ModelState.AddModelError("", "Внесете Email");

                    validUser = await userValidator.ValidateAsync(userManager, newuser);
                    if (!validUser.Succeeded)
                        Errors(validUser);

                    if (!string.IsNullOrEmpty(password))
                    {
                        validPass = await passwordValidator.ValidateAsync(userManager, newuser, password);
                        if (validPass.Succeeded)
                            newuser.PasswordHash = passwordHasher.HashPassword(newuser, password);
                        else
                            Errors(validPass);
                    }
                    else
                        ModelState.AddModelError("", "Внесете лозинка");

                    if (validUser != null && validUser.Succeeded && validPass != null && validPass.Succeeded)
                    {
                        IdentityResult result = await userManager.CreateAsync(newuser, password);
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(newuser, "Student");
                            return RedirectToAction(nameof(StudentView), new { id });
                        }
                        else
                            Errors(result);
                    }
                    user = newuser;
                }
                Student student = _context.Student.Where(s => s.Id == id).FirstOrDefault();
                if (student != null)
                {
                    ViewData["FullName"] = student.FullName;
                    ViewData["StudentId"] = student.Id;
                }
                return View(user);
            }

            [HttpPost]
            public async Task<IActionResult> Delete(string id)
            {
                WorkshopAppUser user = await userManager.FindByIdAsync(id);
                if (user != null)
                {
                    IdentityResult result = await userManager.DeleteAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }
                else
                    ModelState.AddModelError("", "User Not Found");
                return View("Index", userManager.Users);
            }

            private void Errors(IdentityResult result)
            {
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
        }
    }



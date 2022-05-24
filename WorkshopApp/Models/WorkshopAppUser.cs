using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WorkshopApp.Models;

namespace WorkshopApp.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the WorkshopAppUser class
    public class WorkshopAppUser : IdentityUser
    {
        [Display(Name = "Улога")]
        public string Role { get; set; }

        public int? StudentId { get; set; }
        [Display(Name = "Студент")]
        [ForeignKey("StudentId")]
        public Student Student { get; set; }
        public int? TeacherId { get; set; }
        [Display(Name = "Професор")]
        [ForeignKey("TeacherId")]
        public Teacher Teacher { get; set; }
    }
}
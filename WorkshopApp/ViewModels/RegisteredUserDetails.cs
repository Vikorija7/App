using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkshopApp.ViewModels
{
    public class RegisteredUserDetails
    {
        [Display(Name = "лозинка")]
        public string PasswordHash { get; set; }
        [Display(Name = "Телефонски број")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email адреса")]
        public string Email { get; set; }
        [Display(Name = "Корисник")]
        public string UserDetails { get; set; }
        [Display(Name = "Улога")]
        public string Role { get; set; }
        [Display(Name = "Id")]
        public string Id { get; set; }
       
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Нова лозинка")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Повтори лозинка")]
        [Compare("NewPassword", ErrorMessage = "Обиди се повторно")]
        public string ConfirmPassword { get; set; }
    }
}


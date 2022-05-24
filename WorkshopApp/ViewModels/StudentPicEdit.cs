using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WorkshopApp.Models;

namespace WorkshopApp.ViewModels
{
    public class StudentPicEdit
    {
       
        public Int64 ID { get; set; }

      
       
        [Display(Name = "Индекс")]
        public string StudentId { get; set; }

        
        [Display(Name = "Име")]
        public string FirstName { get; set; }

        
        [Display(Name = "Презиме")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата на запишување")]
        public DateTime? ЕnrollmentDate { get; set; }

        [Display(Name = "Освоени кредити")]
        public int? AcquiredCredits { get; set; }

        [Display(Name = "Семестар")]
        public int? CurrentSemestar { get; set; }

        
        [Display(Name = "Степен")]
        public string? EducationLevel { get; set; }

        [Display(Name = "Слика")]
        public IFormFile? Pic { get; set; }

        [Display(Name = "Име и презиме")]
        public string FullName
        {
            get { 
                return FirstName + " " + LastName;
                }
        }

        public ICollection<Enrollment> Courses { get; set; }
    }

}


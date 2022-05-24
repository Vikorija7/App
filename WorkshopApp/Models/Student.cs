using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkshopApp.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Display(Name = "Индекс")]
        public string StudentId { get; set; }

        [Display(Name = "Име")]
        public string FirstName { get; set; }

        [Display(Name = "Презиме")]
        public string LastName { get; set; }

        [Display(Name = "Дата на запишување")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Освоени кредити")]
        public int AcquiredCredits { get; set; }

        [Display(Name = "Семестар")]
        public int CurrentSemestar { get; set; }


        [Display(Name = "Степен")]
        public string EducationLevel { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;


            }
        }

        public string FullNamePlusIndex
        {
            get
            {
                return FirstName + " " + LastName + " " + StudentId;
            }
        }

        public string? profilePicture { get; set; }

        public ICollection<Enrollment> Courses { get; set; }


    }
}

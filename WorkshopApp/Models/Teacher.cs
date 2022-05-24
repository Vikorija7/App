using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WorkshopApp.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Display(Name = "Име")]
        public string FirstName { get; set; }

        [Display(Name = "Презиме")]
        public string LastName { get; set; }

        [Display(Name = "Степен")]
        public string Degree { get; set; }

        [Display(Name = "Звање")]
        public string AcademicRank { get; set; }

        [Display(Name = "Канцеларија")]
        public string OfficeNumber { get; set; }

        [Display(Name = "Дата на вработување")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        [Display(Name = "Слика на профил")]
        public string? profilePicture { get; set; }

        [Display(Name = "Име и Презиме")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
               

            }
        }

        [InverseProperty("FirstTeacher")]
        [Display(Name ="Предмети")]
        public ICollection<Course> FirstCourses { get; set; }


        [InverseProperty("SecondTeacher")]

        public ICollection<Course> SecondCourses { get; set; }





    }
}

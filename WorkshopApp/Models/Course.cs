using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WorkshopApp.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Display(Name = "Наслов")]
        public string Title { get; set; }


        [Display(Name = "Кредити")]
        public int Credits { get; set; }


        [Display(Name = "Семестар")]

        public int Semester { get; set; }

        [Display(Name = "Програма")]

        public string Programme { get; set; }

        [Display(Name = "Ниво")]
        public string EducationLevel { get; set; }

        [Display(Name ="Студенти")]

        public ICollection<Enrollment> Students { get; set; }


        [Display(Name ="Професор 1")]
        [ForeignKey("FirstTeacherId")]
        public int FirstTeacherId { get; set; }

        [Display(Name = "Професор 1")]

        public  Teacher FirstTeacher { get; set; }

        [Display(Name = "Професор 2")]

        [ForeignKey("SecondTeacherId")]
        public int SecondTeacherId { get; set; }

        [Display(Name="Професор 2")]

        public Teacher SecondTeacher { get; set; }


    }
}

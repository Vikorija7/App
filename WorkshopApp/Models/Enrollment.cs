using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkshopApp.Models
{
    public class Enrollment
    {

        public int Id { get; set; }

        [Display(Name ="Студенти")]

        public int StudentId { get; set; }
        [Display(Name = "Студенти")]
        public Student Student { get; set; }

        [Display(Name = "Предмети")]

        public int CourseId { get; set; }
        [Display(Name = "Предмети")]


        public Course Course { get; set; }


        [Display(Name = "Семестар")]

        public String Semester { get; set; }

        [Display(Name = "Година")]
        public int? Year { get; set; }


        [Display(Name = "Оценка")]
        public int Grade { get; set; }

        [Display(Name ="Семинарска")]

        public string SeminalUrl { get; set; }

        [Display(Name ="Проект")]

        public String ProjectUrl { get; set; }

        [Display(Name = "Поени од испит")]

        public int ExamPoints { get; set; }

        [Display(Name = "Поени од семинарски")]

        public int SeminalPoints { get; set; }

        [Display(Name = "Поени од проекти")]

        public int ProjectPoints { get; set; }

        [Display(Name = "Дополнителни поени")]

        public int AdditionalPoints { get; set; }

        [Display(Name = "Дата на положување")]
        [DataType(DataType.Date)]
        public DateTime FinishDate { get; set; }



    }
}

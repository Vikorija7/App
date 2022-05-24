using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WorkshopApp.Models;

namespace WorkshopApp.ViewModels
{
    public class UnEnrollStudents
    {
        public int CourseId { get; set; }

        public SelectList CoursesList { get; set; }

        [Display(Name="Студенти")]
        public IList<int> SelectedEnrollments { get; set; }
        public SelectList Enrollments { get; set; }

        [Display(Name = "Дата на завршување")]
        [DataType(DataType.Date)]
        public DateTime FinishDate { get; set; }
        public int Year { get; set; }

        public string Semester { get; set; }
        public SelectList Semesters { get; set; }
        public SelectList Years { get; set; }



    }
}

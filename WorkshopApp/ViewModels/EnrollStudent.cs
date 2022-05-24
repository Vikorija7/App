using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WorkshopApp.Models;

namespace WorkshopApp.ViewModels
{
    public class EnrollStudent
    {
        public Enrollment Enrollments { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        [Display(Name="Студенти")]
        public IEnumerable<int> SelectedStudents { get; set; }
        public int Year { get; set; }
        public string Semester { get; set; }
        public SelectList Courses { get; set; }
        public SelectList StudentsList { get; set; }
      
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkshopApp.Models;

namespace WorkshopApp.ViewModels
{
    public class EnrollmentFilterViewModel
    {

        public IList<Course> Courses { get; set; }

        public IList<Student> Students { get; set; }

        public IList<Enrollment> Enrollments { get; set; }

        public int SearchSemester { get; set; }

        public SelectList Semesters { get; set; }

        public int SearchYear { get; set; }

        public SelectList Years { get; set; }

        public string SearchIndex { get; set; }

        public string SearchTitle { get; set; }



    }
}

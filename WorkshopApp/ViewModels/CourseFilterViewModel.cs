using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkshopApp.Models;

namespace WorkshopApp.ViewModels
{
    public class CourseFilterViewModel
    {
        public IList<Course> Courses { get; set; }

        public int CourseSemester { get; set; }

        public SelectList SemesterList { get; set; }

        public string CourseProgram { get; set; }
        public SelectList ProgramList { get; set; }

        public string SearchTitle { get; set; }

        public Teacher Teacher { get; set; }

    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkshopApp.Models;

namespace WorkshopApp.ViewModels
{
    public class TeacherCourses
    {

        public Teacher Teacher { get; set; }
        public Course Course { get; set; }
        public IList<Enrollment> Enrollments { get; set; }
        public string SInd { get; set; }
        public SelectList Semesters { get; set; }
        public string SSem { get; set; }
        public SelectList Years { get; set; }
        public int SYear { get; set; }
    }
}

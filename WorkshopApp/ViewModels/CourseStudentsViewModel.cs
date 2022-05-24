using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkshopApp.Models;

namespace WorkshopApp.ViewModels
{
    public class CourseStudentsViewModel
    {

        public Course Course { get; set; }

        public IEnumerable<int> SelectedStudents { get; set; }

        public IEnumerable<SelectListItem> StudentList { get; set; }

    }
}

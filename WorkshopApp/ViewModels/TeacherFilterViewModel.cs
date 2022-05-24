using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkshopApp.Models;

namespace WorkshopApp.ViewModels
{
    public class TeacherFilterViewModel
    {
        public IList<Teacher> Teachers { get; set; }

        public string SearchRank { get; set; }

        public SelectList Ranks { get; set; }

        public string SearchDegree { get; set; }
        public SelectList Degrees { get; set; }

        public string SearchFullName { get; set; }
        public Teacher Teacher { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkshopApp.Models;

namespace WorkshopApp.ViewModels
{
    public class Relation
    {
         public IEnumerable<Course> Courses { get; set; }

        public IEnumerable<Teacher> Teachers { get; set; }
    }
}

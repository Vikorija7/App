using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkshopApp.Models;

namespace WorkshopApp.ViewModels
{
    public class StudentFilterViewModel
    {
        public IList<Student> Students { get; set; }

        public string SearchIndex { get; set; }

        public string SearchFirstName { get; set; }

        public string SearchLastName { get; set; }

        public string SearchFullName { get; set; }

    }
}

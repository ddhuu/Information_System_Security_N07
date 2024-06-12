using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATBM_Project.Models
{
    public class Assignment
    {
        public string LecturerID { get; set; }
        public string CourseID { get; set; }
        public int Semester { get; set; }
        public int Year { get; set; }
        public string Program { get; set; }
    }
}

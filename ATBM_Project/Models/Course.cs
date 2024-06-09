using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATBM_Project.Models
{
    public class Course
    {
        public string CourseID { get; set; }
        public string CourseName { get; set; }
        public int TotalCredit { get; set; }
        public int TheoryCredit { get; set; }
        public int LabCredit { get; set; }
        public string Unit { get; set; }
        public int MaxNumParticipatient { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATBM_Project.Models
{
    internal class Registration
    {
        public string studentID { get; set; }
        public string lecturerID { get; set; }
        public string courseID { get; set; }
        public int Semester { get; set; }
        public int Year { get; set; }
        public string Program { get; set; }
        public double labGrade { get; set; }
        public double progressGrade { get; set; }
        public double finalGrade { get; set; }
        public double totalGrade { get; set; }
    }
}

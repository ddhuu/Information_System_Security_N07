using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATBM_Project.Models
{
    public class CourseRegistration
    {
        public String StudentId { get; set; }
        public String LecturerId {get; set; }
        public String CourseId {get; set; }
        public int Semester { get; set; }
        public int Year { get; set; }
        public string Program { get; set; }
        public double? LabGrade { get; set; }
        public double? ProcessGrade { get; set; }
        public double? FinalExamGrade { get; set; }  
        public double? FinalGrade { get; set; }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATBM_Project.Models
{
    public class Student
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Program { get; set; }
        public string Major { get; set; }
        public double AvgGrade { get; set; }
        public int CummulativeCredits { get; set; }
        public string Group { get; set; }
    }
}

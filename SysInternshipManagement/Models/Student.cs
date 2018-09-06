using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SysInternshipManagement.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailSchool { get; set; }
        public string emailPersonal { get; set; }
        public string phone { get; set; }
        public string DaNumber { get; set; }
        public string permanentCode { get; set; }
        public Preference preference { get; set; }
        public string role { get; set; }

    }
}
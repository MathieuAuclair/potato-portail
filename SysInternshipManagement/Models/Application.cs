using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SysInternshipManagement.Models
{
    public class Application
    {
        public int Id { get; set; }
        public DateTime timestamp { get; set; }
        public Student student { get; set; }
        public Internship internship { get; set; }
    }
}
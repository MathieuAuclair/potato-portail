using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SysInternshipManagement.Models
{
    public class Internship
    {
        public int Id { get; set; }
        public string description { get; set; }
        public string address { get; set; }
        public Location location { get; set; }
        public Post post { get; set; }
        public Status status { get; set; }
        public Contact contact { get; set; }
        public int salary { get; set; }
    }
}
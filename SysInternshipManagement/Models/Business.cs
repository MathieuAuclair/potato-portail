using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SysInternshipManagement.Models
{
    public class Business
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public Contact contact { get; set; }
        public Location location { get; set; }
    }
}
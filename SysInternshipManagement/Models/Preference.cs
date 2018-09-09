using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SysInternshipManagement.Models
{
    public class Preference
    {
        public int Id { get; set; }
        public Location location { get; set; }
        public Business business { get; set; }
        public int salary { get; set; }
        public Post post { get; set; }
    }
}
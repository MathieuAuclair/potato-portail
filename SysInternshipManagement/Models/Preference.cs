using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysInternshipManagement.Models
{
    public class Preference
    {
        [Key]
        public int IdPreference { get; set; }
        public float Salary { get; set; }

        public virtual ICollection<Location> Location { get; set; }

        public virtual ICollection<Business> Business { get; set; }

        public virtual ICollection<Post> Post { get; set; }
    }
}
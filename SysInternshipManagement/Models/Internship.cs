using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysInternshipManagement.Models
{
    public class Internship
    {
        [Key]
        public int IdInternship { get; set; }

        public string Description { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string PostalCode { get; set; }

        public float Salary { get; set; }

        public string DocumentName { get; set; }

        public virtual Location Location { get; set; }

        public virtual Post Post { get; set; }

        public virtual Status Status { get; set; }

        public virtual Contact Contact { get; set; }
    }
}

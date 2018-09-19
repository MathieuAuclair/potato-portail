using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysInternshipManagement.Models
{
    public class Internship
    {
        [Key]
        public int idInternship { get; set; }

        public string description { get; set; }

        [Required]
        public string address { get; set; }

        [Required]
        public string postalCode { get; set; }

        public float salary { get; set; }

        [Required]
        [ForeignKey("location")]
        public virtual ICollection<int> idLocation { get; set; }

        [Required]
        [ForeignKey("post")]
        public virtual ICollection<int> idPost { get; set; }

        [Required]
        [ForeignKey("status")]
        public virtual ICollection<int> idStatus { get; set; }

        [Required]
        [ForeignKey("contact")]
        public virtual ICollection<int> idContact { get; set; }
    }
}

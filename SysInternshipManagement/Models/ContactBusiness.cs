using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysInternshipManagement.Models
{
    public class ContactBusiness
    {
        [Key]
        public int IdContactBusiness { get; set;}

        [Required]
        [ForeignKey("business")]
        public virtual ICollection<int> IdBusiness { get; set; }

        [Required]
        [ForeignKey("contact")]
        public virtual ICollection<int> IdContact { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysInternshipManagement.Models
{
    public class ContactBusiness
    {
        [Key]
        public int idContactBusiness { get; set;}

        [Required]
        [ForeignKey("business")]
        public virtual ICollection<int> idBusiness { get; set; }

        [Required]
        [ForeignKey("contact")]
        public virtual ICollection<int> idContact { get; set; }
    }
}
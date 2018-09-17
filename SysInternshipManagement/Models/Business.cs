using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysInternshipManagement.Models
{
    public class Business
    {
        [Key]
        public int idBusiness { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string address { get; set; }

        [Required]
        [ForeignKey("contactbusiness")]
        public virtual ICollection<int> idContactBusiness { get; set; }

        [Required]
        [ForeignKey("location")]
        public virtual ICollection<int> idLocation { get; set; }
    }
}
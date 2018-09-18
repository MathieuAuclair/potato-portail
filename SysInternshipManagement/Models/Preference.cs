using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysInternshipManagement.Models
{
    public class Preference
    {
        [Key]
        public int idPreference { get; set; }
        public float salary { get; set; }

        [ForeignKey("location")]
        public virtual ICollection<int> idLocation { get; set; }

        [ForeignKey("business")]
        public virtual ICollection<int> idBusiness { get; set; }

        [ForeignKey("post")]
        public virtual ICollection<int> idPost { get; set; }
    }
}
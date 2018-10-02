using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysInternshipManagement.Models
{
    public class Preference
    {
        [Key]
        public int IdPreference { get; set; }
        public float Salaire { get; set; }

        public virtual ICollection<Location> Location { get; set; }

        public virtual ICollection<Entreprise> Entreprise { get; set; }

        public virtual ICollection<Poste> Poste { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysInternshipManagement.Models
{
    public class Stage
    {
        [Key] public int IdStage { get; set; }

        public string Description { get; set; }

        [Required] public string CodePostal { get; set; }

        [Required] public int CivicNumber { get; set; }

        [Required] public string NomRue { get; set; }

        [Required] public string Ville { get; set; }

        [Required] public string Province { get; set; }

        [Required] public string Pays { get; set; }

        public float Salaire { get; set; }

        public string NomDocument { get; set; }

        public virtual Location Location { get; set; }

        public virtual Poste Poste { get; set; }

        public virtual Status Status { get; set; }

        public virtual Contact Contact { get; set; }
    }
}
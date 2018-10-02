using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysInternshipManagement.Models
{
    public class Entreprise
    {
        [Key]
        public int IdEntreprise { get; set; }

        [Required]
        public string Nom { get; set; }
        
        [Required]
        public int NumeroCivique { get; set; }

        [Required]
        public string Rue { get; set; }
        
        [Required]
        public string Ville { get; set; }
        
        [Required]
        public string Province { get; set; }

        [Required]
        public string Pays { get; set; }

        [Required]
        public string CodePostal { get; set; }
    }
}
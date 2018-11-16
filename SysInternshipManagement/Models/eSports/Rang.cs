using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApplicationPlanCadre.Models.eSports
{
    [Table("Rangs")]
    public class Rang
    {
        public Rang()
        {

        }

        public int id { get; set; }

        [Required][Display(Name = "Rang")]
        public string nomRang { get; set; }

        [Required][Display(Name = "Abréviation")]
        [StringLength(6, MinimumLength = 2, ErrorMessage = "L'abréviation doit avoir entre 2 et 6 caractères.")]
        public string abreviation { get; set; }

        [Required][Display(Name = "Hiérarchie")]
        public int hierarchie { get; set; }

        [Display(Name = "Jeu")]
        public int JeuId { get; set; }

        public virtual Jeu Jeu { get; set; }

        public virtual ICollection<HistoriqueRang> HistoriqueRang { get; set; }
    }
}
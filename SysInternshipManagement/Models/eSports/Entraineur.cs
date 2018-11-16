using ApplicationPlanCadre.Models.eSports;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApplicationPlanCadre.Models
{
    [Table("Entraineurs")]
    public class Entraineur
    {
        public Entraineur()
        {
            Equipe = new HashSet<Equipe>();
        }

        public int id { get; set; }

        [Required]
        [Display(Name = "Nom")]
        public string nomEntraineur { get; set; }

        [Required]
        [Display(Name = "Prénom")]
        public string prenomEntraineur { get; set; }

        [Required]
        [Display(Name = "Pseudo")]
        public string pseudoEntraineur { get; set; }

        [Required]
        [Display(Name = "#Téléphone")]
        public string numTel { get; set; }

        [Required]
        [Display(Name = "Courriel")]
        public string adresseCourriel { get; set; }

        public virtual ICollection<Equipe> Equipe { get; set; }
    }
}
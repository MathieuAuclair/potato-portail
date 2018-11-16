using ApplicationPlanCadre.Models.eSports;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApplicationPlanCadre.Models.eSports
{
    [Table("Caracteristiques")]
    public class Caracteristique
    {
        public Caracteristique()
        {

        }

        public int id { get; set; }

        [Required][Display(Name = "Caractéristique")]
        public string nomCaracteristique { get; set; }

        [Display(Name = "Jeu")]
        public int JeuId { get; set; }

        public virtual Jeu Jeu { get; set; }

        public virtual ICollection<Item> Item { get; set; }
    }
}
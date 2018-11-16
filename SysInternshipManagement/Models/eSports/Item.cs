using ApplicationPlanCadre.Models.eSports;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SysInternshipManagement.Models.eSports;

namespace ApplicationPlanCadre.Models.eSports
{
    [Table("Items")]
    public class Item
    {
        public Item()
        {
            Joueur = new HashSet<Joueur>();
        }

        public int id { get; set; }

        [Required][Display(Name = "Nom de l'item")]
        public string nomItem { get; set; }

        [Display(Name = "Caractéristique")]
        public int CaracteristiqueId { get; set; }

        public virtual Caracteristique Caracteristique { get; set; }

        public virtual ICollection <Joueur> Joueur { get; set; }


    }
}
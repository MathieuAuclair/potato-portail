using ApplicationPlanCadre.Models.eSports;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApplicationPlanCadre.Models.eSports
{
    [Table("Statuts")]
    public class Statut
    {
        public Statut()
        {

        }

        public int id { get; set; }

        [Required]
        public string nomStatut { get; set; }

        public ICollection<Jeu> Jeu { get; set; }
    }
}
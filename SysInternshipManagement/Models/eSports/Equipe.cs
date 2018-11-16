using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SysInternshipManagement.Models.eSports;

namespace ApplicationPlanCadre.Models.eSports
{
    [Table("Equipes")]
    public class Equipe
    {
        public Equipe ()
        {
            Entraineur = new HashSet<Entraineur>();
            Joueur = new HashSet<Joueur>();
        }

        public int id { get; set; }

        [Required][Display(Name = "Nom de l'équipe")]
        public string nomEquipe { get; set; }

        [Required]
        public bool estMonojoueur { get; set; }

        [Display(Name = "Jeu associé à l'équipe")]
        public int JeuId { get; set; }
 
        public virtual Jeu Jeu { get; set; }

        public virtual ICollection<Entraineur> Entraineur { get; set; }

        public virtual ICollection<Joueur> Joueur { get; set; }
    }
}
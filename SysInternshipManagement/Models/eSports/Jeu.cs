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
    [Table("Jeux")]
    public class Jeu
    {
        public Jeu()
        {
            Equipe = new HashSet<Equipe>();
        }
        
        public int id { get; set; }

        [Required] [Display(Name = "Nom du jeu")]
        public string nomJeu { get; set; }

        [Display(Name = "Description du jeu")]
        public string description { get; set; }

        [Display(Name = "Adresse du site officiel")]
        public string urlReference { get; set; }

        [Required] [Display(Name = "Abréviation")]
        [StringLength(6, MinimumLength = 2, ErrorMessage = "L'abréviation doit avoir entre 2 et 6 caractères.")]
        public string abreviation { get; set; }

        [Display(Name = "Statut du jeu")]
        public int StatutId { get; set; }

        public virtual ICollection<Caracteristique> Caracteristique { get; set; }

        //[InverseProperty("Jeu")]
        public virtual ICollection<Profil> Profil { get; set; }

        //[InverseProperty("JeuSecondaire")]
        //public virtual ICollection<Profil> ProfilSecondaire { get; set; }

        public virtual ICollection<Rang> Rang { get; set; }

        public virtual ICollection<Equipe> Equipe { get; set; }        

        public virtual Statut Statut { get; set; }
    }
}
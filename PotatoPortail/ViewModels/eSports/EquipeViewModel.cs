using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApplicationPlanCadre.ViewModels.eSportsVM
{
    public class CreationEquipeViewModel
    {

        [Required]
        public int EquipeId { get; set; }

        [Required]
        [Display(Name = "Nom de l'équipe")]
        public string NomEquipe { get; set; }

        [Required]
        public bool EstMonoJoueur { get; set; }

        [Required]
        [Display(Name = "Jeu de l'équipe")]
        public int JeuID { get; set; }

        [Display(Name = "Entraineurs")]
        public List<string> Entraineurs { get; set; }

    }
    public class ModifierEquipeViewModel
    {

        [Required]
        public int EquipeId { get; set; }

        [Required]
        [Display(Name = "Nom de l'équipe")]
        public string NomEquipe { get; set; }

        [Required]
        public bool EstMonoJoueur { get; set; }

        [Required]
        [Display(Name = "Jeu de l'équipe")]
        public int JeuID { get; set; }

        public ApplicationPlanCadre.Models.eSports.Jeu Jeu { get; set; }

        [Display(Name = "Entraineurs")]
        public List<ApplicationPlanCadre.Models.Entraineur> Entraineurs { get; set; }

        [Display(Name = "Joueurs")]
        public List<ApplicationPlanCadre.Models.eSports.Joueur> Joueurs { get; set; }

    }
}
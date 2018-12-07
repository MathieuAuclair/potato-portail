using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PotatoPortail.Models;

namespace PotatoPortail.ViewModels.eSports
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
        public int IdJeu { get; set; }

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
        public int IdJeu { get; set; }

        public Jeu Jeu { get; set; } 

        [Display(Name = "Entraineurs")]
        public List<Entraineur> Entraineurs { get; set; }

        [Display(Name = "Joueurs")]
        public List<Joueur> Joueurs { get; set; }

    }
}
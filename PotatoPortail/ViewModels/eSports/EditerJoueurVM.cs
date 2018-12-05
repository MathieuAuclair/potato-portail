using System.ComponentModel.DataAnnotations;
using ApplicationPlanCadre.Models;
using ApplicationPlanCadre.Models.eSports;

namespace ApplicationPlanCadre.ViewModels.eSportsVM
{
    public class EditerJoueurVm
    {
        public int JoueurId { get; set; }

        [Required]
        [Display(Name = "Pseudonyme")]
        public string pseudo { get; set; }

        [Required]
        [Display(Name = "Adresse courriel")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Veuillez entrer une adresse courriel valide.")]
        public string courriel { get; set; }

        public virtual MembreESports MembreESports { get; set; }

        public virtual Jeu Jeu { get; set; }
    }
}
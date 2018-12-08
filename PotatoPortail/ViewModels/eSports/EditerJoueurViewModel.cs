using System.ComponentModel.DataAnnotations;
using PotatoPortail.Models;
using PotatoPortail.Models.eSports;

namespace PotatoPortail.ViewModels.eSports
{
    public class EditerJoueurViewModel
    {
        public int JoueurId { get; set; }

        [Required]
        [Display(Name = "Pseudonyme")]
        public string Pseudo { get; set; }

        [Required]
        [Display(Name = "Adresse courriel")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Veuillez entrer une adresse courriel valide.")]
        public string Courriel { get; set; }

        public virtual MembreESports MembreESports { get; set; }

        public virtual Jeu Jeu { get; set; }
    }
}
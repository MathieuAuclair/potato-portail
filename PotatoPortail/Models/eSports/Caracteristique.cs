using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PotatoPortail.Models.eSports
{
    public partial class Caracteristique
    {
        public Caracteristique()
        {

        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Caractéristique")]
        public string NomCaracteristique { get; set; }

        [Display(Name = "Jeu")]
        public int IdJeu { get; set; }

        public virtual Jeu Jeu { get; set; }

        public virtual ICollection<Item> Item { get; set; }
    }
}

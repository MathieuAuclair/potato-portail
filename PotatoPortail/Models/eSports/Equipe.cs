using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PotatoPortail.Models.eSports
{
    public partial class Equipe
    {
        public Equipe()
        {
            Entraineur = new HashSet<Entraineur>();
            Joueur = new HashSet<Joueur>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Nom de l'équipe")]
        public string NomEquipe { get; set; }

        [Required]
        public bool EstMonoJoueur { get; set; }

        [Display(Name = "Jeu associé à l'équipe")]
        public int IdJeu { get; set; }

        public virtual Jeu Jeu { get; set; }

        public virtual ICollection<Entraineur> Entraineur { get; set; }

        public virtual ICollection<Joueur> Joueur { get; set; }
    }
}

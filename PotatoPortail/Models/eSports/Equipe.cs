using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PotatoPortail.Models.eSports
{
    public partial class Equipe
    {
        public Equipe()
        {
            Entraineurs = new HashSet<Entraineur>();
            Joueurs = new HashSet<Joueur>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nom de l'équipe")]
        public string NomEquipe { get; set; }

        [Required]
        public bool EstMonoJoueur { get; set; }

        [Display(Name = "Jeu associé à l'équipe")]
        public int IdJeu { get; set; }

        public virtual Jeu Jeu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Entraineur> Entraineurs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Joueur> Joueurs { get; set; }
    }
}

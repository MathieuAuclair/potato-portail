using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PotatoPortail.Migrations;

namespace PotatoPortail.Models.eSports
{
    public partial class Profil
    {
        private readonly BdPortail _db = new BdPortail();

        public Profil()
        {

        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Pseudonyme")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Votre pseudonyme doit comprendre au minimum 2 caractères et 25 au maximum.")]
        public string Pseudo { get; set; }

        [Required]
        [Display(Name = "Adresse courriel")]
        public string Courriel { get; set; }

        [Display(Name = "Informations supplémentaires (facultatives)")]
        public string Note { get; set; }

        public bool EstArchive { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Étudiant")]
        public string IdMembreESports { get; set; }

        [Display(Name = "Jeu")]
        public int IdJeu { get; set; }

        public int? IdJeuSecondaire { get; set; }

        public virtual MembreESports MembreESports { get; set; }

        public virtual Jeu Jeu { get; set; }

        public virtual ICollection<Joueur> Joueur { get; set; }

        public Jeu JeuSecondaire => _db.Jeu.Find(IdJeuSecondaire);
    }
}

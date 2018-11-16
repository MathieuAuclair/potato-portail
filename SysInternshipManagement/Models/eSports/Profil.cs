using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationPlanCadre.Models.eSports;
using SysInternshipManagement.Migrations;

namespace SysInternshipManagement.Models.eSports
{
    [Table("Profils")]
    public class Profil
    {
        private readonly DatabaseContext _db = new DatabaseContext();

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

        [Display(Name = "Étudiant")]
        public int EtudiantId { get; set; }

        [Display(Name = "Jeu")]
        public int JeuId { get; set; }

        [Display(Name = "Jeu secondaire (optionnel)")]
        public int? JeuSecondaireId { get; set; }

        public virtual Etudiant Etudiant { get; set; }

        public virtual Jeu Jeu { get; set; }

        public virtual Joueur Joueur { get; set; }

        public Jeu JeuSecondaire => _db.Jeux.Find(JeuSecondaireId);
    }
}
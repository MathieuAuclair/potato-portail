using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysInternshipManagement.Models.eSports
{
    [Table("Etudiants")]
    public class MembreEsport
    {
        [Key] public int IdMembreEsport { get; set; }
        public virtual ICollection<Joueur> Joueur { get; set; }
        public virtual ICollection<Profil> Profil { get; set; }

        [Required] public virtual Etudiant Etudiant { get; set; }
    }
}
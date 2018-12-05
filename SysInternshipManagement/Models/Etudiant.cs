using System.ComponentModel.DataAnnotations;

namespace SysInternshipManagement.Models
{
    public class Etudiant
    {
        [Key] public int IdEtudiant { get; set; }

        [Required] public string Prenom { get; set; }

        [Required] public string NomDeFamille { get; set; }

        [Required] public string CourrielEcole { get; set; }

        [Required] public string CourrielPersonnel { get; set; }

        [Required] public string Telephone { get; set; }

        [Required] public string NumeroDa { get; set; }

        [Required] public string CodePermanent { get; set; }

        [Required] public string Role { get; set; }

        public virtual Preference Preference { get; set; }

        public string NomComplet
        {
            get
            {
                return Prenom + " " + NomDeFamille;
            }
        }
    }
}
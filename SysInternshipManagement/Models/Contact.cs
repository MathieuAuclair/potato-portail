using System.ComponentModel.DataAnnotations;

namespace SysInternshipManagement.Models
{
    public class Contact
    {
        [Key] public int IdContact { get; set; }

        [Required] public string Nom { get; set; }

        [Required] public string Telephone { get; set; }

        [Required] public string Courriel { get; set; }

        [Required] public virtual Entreprise Entreprise { get; set; }
    }
}
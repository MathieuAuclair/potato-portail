using System.ComponentModel.DataAnnotations;

namespace SysInternshipManagement.Models
{
    public class Responsable

    {
    [Key] public int IdResponsable { get; set; }

    [Required] public string Prenom { get; set; }

    [Required] public string NomDeFamille { get; set; }

    [Required] public string Telephone { get; set; }

    [Required] public string Courriel { get; set; }

    [Required] public string Role { get; set; }
    }
}
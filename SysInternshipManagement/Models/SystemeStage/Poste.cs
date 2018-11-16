using System.ComponentModel.DataAnnotations;

namespace SysInternshipManagement.Models
{
    public class Poste
    {
        [Key]
        public int IdPoste { get; set; }

        [Required]
        public string Nom { get; set; }
    }
}
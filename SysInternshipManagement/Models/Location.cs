using System.ComponentModel.DataAnnotations;

namespace SysInternshipManagement.Models
{
    public class Location
    {
        [Key]
        public int IdLocation { get; set; }

        [Required]
        public string Nom { get; set; }
    }
}
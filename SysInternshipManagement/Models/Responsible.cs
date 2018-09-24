using System.ComponentModel.DataAnnotations;

namespace SysInternshipManagement.Models
{
    public class Responsible
    {
        [Key]
        public int IdResponsible { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
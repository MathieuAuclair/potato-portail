using System.ComponentModel.DataAnnotations;

namespace SysInternshipManagement.Models
{
    public class Contact
    {
        [Key]
        public int IdContact { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

    }
}
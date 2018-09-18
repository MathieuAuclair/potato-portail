using System.ComponentModel.DataAnnotations;

namespace SysInternshipManagement.Models
{
    public class Contact
    {
        [Key]
        public int idContact { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string phone { get; set; }

        [Required]
        public string email { get; set; }

    }
}
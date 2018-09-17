using System.ComponentModel.DataAnnotations;

namespace SysInternshipManagement.Models
{
    public class Responsible
    {
        [Key]
        public int idResponsible { get; set; }

        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }

        [Required]
        public string phone { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string role { get; set; }
    }
}
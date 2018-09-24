using System.ComponentModel.DataAnnotations;

namespace SysInternshipManagement.Models
{
    public class Status
    {
        [Key]
        public int IdStatus { get; set; }

        [Required]
        public string StatusInternship { get; set; }
    }
}

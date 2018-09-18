using System.ComponentModel.DataAnnotations;

namespace SysInternshipManagement.Models
{
    public class Status
    {
        [Key]
        public int idStatus { get; set; }

        [Required]
        public string status { get; set; }
    }
}

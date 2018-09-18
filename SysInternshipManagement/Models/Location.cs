using System.ComponentModel.DataAnnotations;

namespace SysInternshipManagement.Models
{
    public class Location
    {
        [Key]
        public int idLocation { get; set; }

        [Required]
        public string name { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace SysInternshipManagement.Models
{
    public class Post
    {
        [Key]
        public int IdPost { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
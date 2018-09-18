using System.ComponentModel.DataAnnotations;

namespace SysInternshipManagement.Models
{
    public class Post
    {
        [Key]
        public int idPost { get; set; }

        [Required]
        public string name { get; set; }
    }
}
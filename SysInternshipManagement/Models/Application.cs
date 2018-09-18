using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysInternshipManagement.Models
{
    public class Application
    {
        [Key]
        public int idApplication { get; set; }

        [Required]
        public DateTime timestamp { get; set; }

        [Required]
        [ForeignKey("intership")]
        public virtual ICollection<int> idInternship { get; set; }

        [Required]
        [ForeignKey("student")]
        public virtual ICollection<int> student { get; set; }
    }
}
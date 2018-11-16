using System;
using System.ComponentModel.DataAnnotations;
using SysInternshipManagement.Models.SystemeStage;

namespace SysInternshipManagement.Models
{
    public class Application
    {
        [Key]
        public int IdApplication { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public virtual Stage Stage { get; set; }

        [Required]
        public virtual Etudiant Etudiant { get; set; }
    }
}
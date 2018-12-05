using System;
using System.ComponentModel.DataAnnotations;
using SysInternshipManagement.Models.SystemeStage;

namespace SysInternshipManagement.Models
{
    public class Application
    {
        [Key]
        public int IdApplication { get; set; }

        public DateTime Timestamp { get; set; }

        public virtual Stage Stage { get; set; }

        public virtual Etudiant Etudiant { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysInternshipManagement.Models
{
    public class Application
    {
        [Key]
        public int IdApplication { get; set; }

        public DateTime Timestamp { get; set; }

        [Display(Name = "Stage voulu")]
        public int IdStage { get; set; }

        public virtual Stage Stage { get; set; }

        public virtual Etudiant Etudiant { get; set; }
    }
}
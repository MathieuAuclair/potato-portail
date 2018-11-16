using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SysInternshipManagement.Models;
using SysInternshipManagement.Models.eSports;

namespace ApplicationPlanCadre.Models.eSports
{
    public class EditerJoueurVM
    {
        public int JoueurId { get; set; }

        [Required]
        [Display(Name = "Pseudonyme")]
        public string pseudo { get; set; }

        [Required]
        [Display(Name = "Adresse courriel")]
        public string courriel { get; set; }

        public virtual Etudiant Etudiant { get; set; }

        public virtual Jeu Jeu { get; set; }
    }
}
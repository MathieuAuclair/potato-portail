using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApplicationPlanCadre.Models
{
    public class ConnexionViewModel
    {
        [Required]
        [Display(Name = "Courriel")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [Display(Name = "Mémoriser le mot de passe ?")]
        public bool RememberMe { get; set; }
    }

    public class EnregistrementViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Nom")]
        public string Nom { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }
		
        [Required]
        [EmailAddress]
        [Display(Name = "Courriel")]
        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<string> CodeProgrammes { get; set; }
    }

    public class ModifierUtilisateurViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nom")]
        public string Nom { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Courriel")]
        public string Email { get; set; }

        [Display(Name = "Rôles")]
        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<string> CodeProgrammes { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PotatoPortail.Models;

namespace PotatoPortail.ViewModels.PlanCadreVM
{
    public class ActiviteViewModel
    {
        [Display(Name = "Activité d'apprentissage")]
        public List<ActiviteApprentissage> ActiviteApprentissages { get; set; }
        [Display(Name = "Sous-activité d'apprentissage")]
        public List<SousActiviteApprentissage> SousActiviteApprentissages { get; set; }
    }

    public class ActiviteSousActivite
    {
        public string Activite { get; set; }
        public List<string> SousActivites { get; set; }
    }
}
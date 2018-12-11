using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PotatoPortail.Models;

namespace PotatoPortail.ViewModels.PlanCadre
{
    public class ActiviteViewModel
    {
        public int IdPlanCadre { get; set; }
        public int IdPlanCadreElement { get; set; }
        [Display(Name = "Activité d'apprentissage")]
        public List<ActiviteSousActivite> ActiviteSousActivites { get; set; }
    }

    public class ActiviteSousActivite
    {
        public ActiviteApprentissage Activite { get; set; }
        public List<SousActiviteApprentissage> SousActivites { get; set; }
    }
}
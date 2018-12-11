using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PotatoPortail.Models;

namespace PotatoPortail.ViewModels.PlanCadre
{
    public class EnvironnementPhysiqueViewModel
    {
        public int IdPlanCadre { get; set; }
        [Display(Name = "Environnement Physiques")]
        public List<EnvironnementSousEnvironnement> EnvironnementSousEnvironnements { get; set; }

    }
    public class EnvironnementSousEnvironnement
    {
        public EnvironnementPhysique EnvironnementPhysique { get; set; }
        public List<SousEnvironnementPhysique> SousEnvironnementPhysiques { get; set; }
    }
}
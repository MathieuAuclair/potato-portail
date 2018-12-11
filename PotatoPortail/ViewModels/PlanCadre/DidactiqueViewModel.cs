using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PotatoPortail.Models;

namespace PotatoPortail.ViewModels.PlanCadre
{
    public class DidactiqueViewModel
    {
        public int IdPlanCadre { get; set; }
        [Display(Name = "Ressource Didactiques")]
        public List<DidactiqueSousDidactique> DidactiqueSousDidactiques {get;set;}
        
    }
    public class DidactiqueSousDidactique
    {
        public RessourceDidactique ressourceDidactique { get; set; }
        public List<SousRessourceDidactique> sousRessourceDidactiques { get; set; }
    }
}
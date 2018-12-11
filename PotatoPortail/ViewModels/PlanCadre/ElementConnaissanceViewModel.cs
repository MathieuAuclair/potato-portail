using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PotatoPortail.Models;

namespace PotatoPortail.ViewModels.PlanCadre
{
    public class ElementConnaissanceViewModel
    {
        public int IdPlanCadre { get; set; }
        public int IdPlanCadreElement { get; set; }
        [Display(Name = "Activité d'apprentissage")]
        public List<ConnaissanceSousElement> ConnaissanceSousElements { get; set; }
    }

    public class ConnaissanceSousElement
    {
        public ElementConnaissance ElementConnaissance { get; set; }
        public List<SousElementConnaissance> SousElementConnaissances { get; set; }
    }
}
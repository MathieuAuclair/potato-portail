using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationPlanCadre.Models
{
    public class GrilleCoursViewModel
    {
        public GrilleCours Grille { get; set; }
        public List<EnonceCompetence> Enonces { get; set; }
    }
}
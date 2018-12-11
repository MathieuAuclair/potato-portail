using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PotatoPortail.Models;

namespace PotatoPortail.ViewModels.PlanCadre
{
    public class CompetenceViewModel
    {
        public virtual int idPlanCadre { get; set; }
        public virtual Models.PlanCadre PlanCadre { get; set; }
        public virtual IEnumerable<EnonceCompetence> EnonceCompetence { get; set; }
        public virtual IEnumerable<SelectListItem> EnonceCompetences { get; set; }
        public virtual IEnumerable<ElementCompetence> ElementCompetence { get; set; }
        public virtual IEnumerable<SelectListItem> ElementCompetences { get; set; }
        public string idEnonce { get; set; }
        public List<ElementCompetence> listeElement { get; set; }
    }

    public class PlanCadreCompetenceElement
    {
        public int IdEnonce { get; set; }
        public ICollection<int> IdElements { get; set; }
        public int Ponderation { get; set; }
    }
}
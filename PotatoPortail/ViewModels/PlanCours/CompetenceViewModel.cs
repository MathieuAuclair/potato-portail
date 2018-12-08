using System.Collections.Generic;
using System.Web.Mvc;
using PotatoPortail.Models;

namespace PotatoPortail.ViewModels.PlanCours
{
    public class CompetenceViewModel
    {
        public SelectList TypePlanCadre { get; set; }
        public virtual int IdType { get; set; }
        public virtual string NomType { get; set; }
        public int? NbHeureTheorie { get; set; }
        public int? NbHeurePratique { get; set; }
        public int? NbHeureDevoir { get; set; }
        public string NumeroCours { get; set; }
        public string TitreCours { get; set; }
        public string IndicationPedago { get; set; }
		public virtual int IdPlanCadre { get; set; }
        public virtual Models.PlanCadre PlanCadre { get; set; }
        public virtual IEnumerable<string> CodeCompetences { get; set; }
        public virtual IEnumerable<int> IdElements { get; set; }
        public virtual IEnumerable<string> Description { get; set; }
        public string IdCompetence { get; set; }
        public virtual string CodeCompetence { get; set; }
        public virtual IEnumerable<EnonceCompetence> EnonceCompetence { get; set; }
        public virtual IEnumerable<SelectListItem> EnonceCompetences { get; set; }
        public virtual IEnumerable<ElementCompetence> ElementCompetence { get; set; }
        public virtual IEnumerable<SelectListItem> ElementCompetences { get; set; }
        public virtual IEnumerable<string> DescriptionElementCompetence { get; set; }
        public string IdEnonce { get; set; }
        public ICollection<ElementCompetence> ListeElement { get; set; }
        public List<string> Element { get; set; }
    }
}
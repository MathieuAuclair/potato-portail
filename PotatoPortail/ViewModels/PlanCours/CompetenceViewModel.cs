using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationPlanCadre.Models;

namespace ApplicationPlanCadre.ViewModels
{
    public class CompetenceViewModel
    {
        public SelectList typePlanCadre { get; set; }
        public virtual int idType { get; set; }
        public virtual string nomType { get; set; }
        public int? nbHeureTheorie { get; set; }
        public int? nbHeurePratique { get; set; }
        public int? nbHeureDevoir { get; set; }
        public string numeroCours { get; set; }
        public string titreCours { get; set; }
        public string indicationPedago { get; set; }
		public virtual int idPlanCadre { get; set; }
        public virtual PlanCadre PlanCadre { get; set; }
        public virtual IEnumerable<string> codeCompetences { get; set; }
        public virtual IEnumerable<int> idElements { get; set; }
        public virtual IEnumerable<string> description { get; set; }
        public string idCompetence { get; set; }
        public virtual string codeCompetence { get; set; }
        public virtual IEnumerable<EnonceCompetence> EnonceCompetence { get; set; }
        public virtual IEnumerable<SelectListItem> EnonceCompetences { get; set; }
        public virtual IEnumerable<ElementCompetence> ElementCompetence { get; set; }
        public virtual IEnumerable<SelectListItem> ElementCompetences { get; set; }
        public virtual IEnumerable<string> DescriptionElementCompetence { get; set; }
        public string idEnonce { get; set; }
        public ICollection<ElementCompetence> listeElement { get; set; }
        public List<string> Element { get; set; }
    }
}
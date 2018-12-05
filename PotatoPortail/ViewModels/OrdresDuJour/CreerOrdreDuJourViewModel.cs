using PotatoPortail.Models;

namespace ApplicationPlanCadre.ViewModels.OrdresDuJourVM
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class CreerOrdreDuJourViewModel
    {
        public OrdreDuJour OrdreDuJour { get; set; }
        public List<SousPointSujet> SousPointSujet { get; set; }
        public List<SujetPointPrincipal> SujetPointPrincipal { get; set; }
        public IEnumerable<SelectListItem> ListLieux { get; set; }
        public List<string> ListeSousPoint { get; set; }
        public List<int> listeSousPointCache { get; set; }
    }
}
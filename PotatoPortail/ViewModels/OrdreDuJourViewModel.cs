using System.Collections.Generic;
using System.Web.Mvc;
using PotatoPortail.Models.Reunions;

namespace PotatoPortail.ViewModels
{
    public class OrdreDuJourViewModel
    {
        public OrdreDuJour OrdreDuJour { get; set; }
        public List<SousPointSujet> ListeSousPointSujet { get; set; }
        public List<SujetPointPrincipal> SujetPointPrincipal { get; set; }
        public IEnumerable<SelectListItem> ListLieux { get; set; }
        public List<string> ListeSousPoint { get; set; }
        public List<int> ListeIdSousPointCache { get; set; }
    }
}
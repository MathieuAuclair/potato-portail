namespace ApplicationPlanCadre.ViewModels.OrdresDuJourVM
{
    using ApplicationPlanCadre.Models.Reunions;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class OrdreDuJourViewModel
    {
        public OrdreDuJour ordreDuJour { get; set; }
        public List<SousPointSujet> listeSousPointSujet { get; set; }
        public List<SujetPointPrincipal> sujetPointPrincipal { get; set; }
        public IEnumerable<SelectListItem> listLieux { get; set; }
        public List<string> listeSousPoint { get; set; }
        public List<int> listeIdSousPointCache { get; set; }
    }
}
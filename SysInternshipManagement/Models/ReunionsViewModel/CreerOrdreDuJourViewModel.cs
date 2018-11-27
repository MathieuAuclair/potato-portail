using SysInternshipManagement.Models.Reunions;

namespace ApplicationPlanCadre.Models.ReunionsViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using ApplicationPlanCadre.Models.Reunions;

    public class CreerOrdreDuJourViewModel
    {
        public OrdreDuJour ordreDuJour { get; set; }
        public List<SousPointSujet> sousPointSujet { get; set; }
        public List<SujetPointPrincipal> sujetPointPrincipal { get; set; }
        public IEnumerable<SelectListItem> listLieux { get; set; }
        public List<string> listSP { get; set; }
        public List<int> listHSP { get; set; }
    }
}
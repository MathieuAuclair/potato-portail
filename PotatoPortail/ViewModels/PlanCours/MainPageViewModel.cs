using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ApplicationPlanCadre.Models;
using PotatoPortail.Models;

namespace ApplicationPlanCadre.ViewModels
{
    public class MainPageViewModel
    {
        public virtual List<ContenuSection> contenuSection { get; set; }
        public virtual List<NomSection> nomSection { get; set; }
        public virtual List<PlanCours> PlanCours { get; set; }
        public virtual List<PlanCadre> PlanCadre {  get; set; }

        public virtual List<List<NomSection>> NomSections { get; set; }
        public int PlanCadreId { get; set; }
        public List<string> texteContenu { get; set; }
        public string titreSection { get; set; }

    }
}
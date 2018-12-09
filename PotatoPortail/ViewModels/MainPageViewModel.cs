using System.Collections.Generic;
using PotatoPortail.Models;
using PotatoPortail.Models.Plan_Cours;

namespace PotatoPortail.ViewModels
{
    public class MainPageViewModel
    {
        public virtual List<ContenuSection> ContenuSection { get; set; }
        public virtual List<NomSection> NomSection { get; set; }

        public virtual List<Models.Plan_Cours.PlanCours> PlanCours { get; set; }
        public virtual List<Models.PlanCadre> PlanCadre {  get; set; }

        public virtual List<List<NomSection>> NomSections { get; set; }
        public int PlanCadreId { get; set; }

        public List<string> TexteContenu { get; set; }
        public string TitreSection { get; set; }
    }
}
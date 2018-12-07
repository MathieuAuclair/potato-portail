using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PotatoPortail.Models;

namespace PotatoPortail.ViewModels.PlanCours
{
    public class ServicesAdaptesViewModel:ViewModelBase
    {
        [Display(Name = "Section")]
        public virtual IEnumerable<NomSection> NomSections { get; set; }
        public virtual string TitreSection { get; set; }
        public virtual IEnumerable<ContenuSection> ContenuSection { get; set; }
        [Display(Name = "Contenu")]
        public virtual string TexteContenu { get; set; }
        public Models.PlanCours PlanCours { get; set; }
        public int IdPlanCours { get; set; }
        [Display(Name = "Discipline")]
        public virtual IEnumerable<Departement> Departements { get; set; }
        public virtual int Discipline { get; set; }
        public virtual string Nom { get; set; }
    }
}
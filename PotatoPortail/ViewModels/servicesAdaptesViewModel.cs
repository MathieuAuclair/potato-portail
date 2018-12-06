using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ApplicationPlanCadre.Models;
using PotatoPortail.Models;

namespace PotatoPortail.ViewModels
{
    public class servicesAdaptesViewModel:ViewModelBase
    {
        public virtual IEnumerable<NomSection> NomSections { get; set; }
        public virtual string TitreSection { get; set; }
        public virtual IEnumerable<ContenuSection> ContenuSection { get; set; }
        public virtual string TexteContenu { get; set; }
        public PlanCours PlanCours { get; set; }
        public int IdPlanCours { get; set; }
        public virtual IEnumerable<Departement> Departements { get; set; }
        public virtual int Discipline { get; set; }
        public virtual string Nom { get; set; }
    }
}
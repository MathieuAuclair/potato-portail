﻿using System.Collections.Generic;
using PotatoPortail.Models;
using PotatoPortail.Models.Plan_Cours;

namespace PotatoPortail.ViewModels
{
    public class ServicesAdaptesViewModel:ViewModelBase
    {
        public virtual IEnumerable<NomSection> NomSections { get; set; }
        public virtual string TitreSection { get; set; }
        public virtual IEnumerable<ContenuSection> ContenuSection { get; set; }
        public virtual string TexteContenu { get; set; }
        public Models.Plan_Cours.PlanCours PlanCours { get; set; }
        public int IdPlanCours { get; set; }
        public virtual IEnumerable<Departement> Departements { get; set; }
        public virtual int Discipline { get; set; }
        public virtual string Nom { get; set; }
    }
}
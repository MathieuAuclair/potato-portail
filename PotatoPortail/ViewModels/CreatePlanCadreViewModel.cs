﻿using System;
using System.Collections.Generic;
using PotatoPortail.Models;
using PotatoPortail.Models.Plan_Cours;

namespace PotatoPortail.ViewModels
{
    public class CreatePlanCadreViewModel: PotatoPortail.ViewModels.ViewModelBase
    {
        public ContenuSection ContenuSection { get; set; }
        public virtual List<NomSection> NomSection { get; set; }
        public virtual Models.Plan_Cours.PlanCours PlanCours { get; set; }
        public virtual IEnumerable<Models.PlanCadre> PlanCadre { get; set; }
        public int IdPlanCours { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime? DateValidation { get; set; }
        public bool StatusPlanCours { get; set; }
    }
}
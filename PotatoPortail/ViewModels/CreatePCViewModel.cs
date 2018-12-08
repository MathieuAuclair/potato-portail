using ApplicationPlanCadre.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;

using System.Web.Mvc;

namespace ApplicationPlanCadre.ViewModels
{
    public class CreatePCViewModel: ViewModelBase
    {
        public ContenuSection contenuSection { get; set; }
        public virtual List<NomSection> nomSection { get; set; }
        public virtual PlanCours PlanCours { get; set; }
        public virtual IEnumerable<PlanCadre> PlanCadre { get; set; }
        public int idPlanCours { get; set; }
        public DateTime dateCreation { get; set; }
        public DateTime? dateValidation { get; set; }
        public bool statusPlanCours { get; set; }
    }
}
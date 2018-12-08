using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PotatoPortail.Models.Plan_Cours
{
    public class TexteSection
    {
        [Key]
        [Column(Order = 1)]
        public int idPlanCours { get; set; }
        [Key]
        [Column(Order = 2)]
        public int idContenuSection { get; set; }

        public PlanCours PlanCours { get; set; }

        public ContenuSection ContenuSection { get; set; }
    }
}
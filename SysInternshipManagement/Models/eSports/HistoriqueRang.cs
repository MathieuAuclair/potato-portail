using ApplicationPlanCadre.Models.eSports;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SysInternshipManagement.Models.eSports;

namespace ApplicationPlanCadre.Models
{
    public class HistoriqueRang
    {
        public HistoriqueRang()
        {

        }

        [Key, Column(Order = 0)]
        public int JoueurId { get; set; }

        [Key, Column(Order = 1)]
        public int RangId { get; set; }

        public DateTime date { get; set; }

        public virtual Joueur Joueur { get; set; }

        public virtual Rang Rang { get; set; }
    }
}
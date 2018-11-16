using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApplicationPlanCadre.Models
{
    [Table("lieuDeLaReunion")]
    public class lieuDeLaReunion
    {
        [Key]
        public int idLieu { get; set; }

        public string emplacementReunion { get; set; }
    }
}
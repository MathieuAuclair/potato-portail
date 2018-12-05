namespace ApplicationPlanCadre.Models.Reunions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;


    [Table("LieuDeLaReunion")]
    public partial class LieuDeLaReunion
    {
        [Key]
        public int IdLieu { get; set; }

        public string EmplacementReunion { get; set; }
    }
}

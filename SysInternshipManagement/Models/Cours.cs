namespace ApplicationPlanCadre.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cours
    {
        [Key]
        public int idCours { get; set; }

        public int idPlanCadre { get; set; }

        public int idGrille { get; set; }

        public int idSession { get; set; }

        public virtual GrilleCours GrilleCours { get; set; }

        public virtual PlanCadre PlanCadre { get; set; }

        public virtual Session Session { get; set; }
    }
}

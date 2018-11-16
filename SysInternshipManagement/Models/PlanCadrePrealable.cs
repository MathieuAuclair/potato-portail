namespace ApplicationPlanCadre.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PlanCadrePrealable")]
    public partial class PlanCadrePrealable
    {
        [Key]
        public int idPlanCadrePrealable { get; set; }

        public int idPlanCadre { get; set; }

        public int idPrealable { get; set; }

        public int idStatus { get; set; }

        public virtual PlanCadre PlanCadre { get; set; }

        public virtual PlanCadre Prealable { get; set; }

        public virtual StatusPrealable StatusPrealable { get; set; }
    }
}

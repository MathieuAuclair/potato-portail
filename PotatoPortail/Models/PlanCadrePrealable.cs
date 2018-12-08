namespace PotatoPortail.Models
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
        public int IdPlanCadrePrealable { get; set; }

        public int IdPlanCadre { get; set; }

        public int IdPrealable { get; set; }

        public int IdStatut { get; set; }

        public virtual PlanCadre PlanCadre { get; set; }

        public virtual StatutPrealable StatutPrealable { get; set; }
    }
}

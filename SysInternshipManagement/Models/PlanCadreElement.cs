namespace ApplicationPlanCadre.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PlanCadreElement")]
    public partial class PlanCadreElement
    {
        [Key]
        [Column(Order = 0)]
        public int idPlanCadreElement { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idPlanCadre { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idElement { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idElementConnaissance { get; set; }

        public virtual ElementCompetence ElementCompetence { get; set; }

        public virtual ElementConnaissance ElementConnaissance { get; set; }

        public virtual PlanCadre PlanCadre { get; set; }
    }
}

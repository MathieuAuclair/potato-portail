namespace ApplicationPlanCadre.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CriterePerformance")]
    public partial class CriterePerformance
    {
        [Key]
        public int idCritere { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "Critère de performance")]
        public string description { get; set; }

        [Display(Name = "Numéro")]
        public int numero { get; set; }

        public int idElement { get; set; }

        public virtual ElementCompetence ElementCompetence { get; set; }
    }
}

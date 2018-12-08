namespace PotatoPortail.Models
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
        public int IdCritere { get; set; }

        [Required]
        [StringLength(300)]
        public string Description { get; set; }

        public int Numero { get; set; }

        public int IdElement { get; set; }

        public virtual ElementCompetence ElementCompetence { get; set; }
    }
}

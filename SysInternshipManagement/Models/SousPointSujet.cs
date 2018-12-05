namespace SysInternshipManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SousPointSujet")]
    public partial class SousPointSujet
    {
        [Key]
        public int IdSousPoint { get; set; }

        [Required]
        [StringLength(50)]
        public string SujetSousPoint { get; set; }

        public int IdSujetPointPrincipal { get; set; }

        public virtual SujetPointPrincipal SujetPointPrincipal { get; set; }
    }
}

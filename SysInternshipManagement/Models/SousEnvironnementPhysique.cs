namespace SysInternshipManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SousEnvironnementPhysique")]
    public partial class SousEnvironnementPhysique
    {
        [Key]
        public int IdSousEnvPhys { get; set; }

        [Column(TypeName = "text")]
        public string NomSousEnvPhys { get; set; }

        public int IdEnvPhysique { get; set; }

        public virtual EnvironnementPhysique EnvironnementPhysique { get; set; }
    }
}

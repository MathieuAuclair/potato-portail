namespace SysInternshipManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SousActiviteApprentissage")]
    public partial class SousActiviteApprentissage
    {
        [Key]
        public int IdSousActivite { get; set; }

        [Column(TypeName = "text")]
        public string NomSousActivite { get; set; }

        public int IdActivite { get; set; }

        public virtual ActiviteApprentissage ActiviteApprentissage { get; set; }
    }
}

namespace SysInternshipManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SousRessourceDIdactique")]
    public partial class SousRessourceDIdactique
    {
        [Key]
        public int IdSousRessource { get; set; }

        [Column(TypeName = "text")]
        public string NomSousRessource { get; set; }

        public int IdRessource { get; set; }

        public virtual RessourceDIdactique RessourceDIdactique { get; set; }
    }
}

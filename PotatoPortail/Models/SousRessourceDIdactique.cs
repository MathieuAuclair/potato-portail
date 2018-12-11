namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SousRessourceDidactique")]
    public partial class SousRessourceDidactique
    {
        [Key]
        public int IdSousRessource { get; set; }

        [Column(TypeName = "text")]
        public string NomSousRessource { get; set; }

        public int IdRessource { get; set; }

        public virtual RessourceDidactique RessourceDidactique { get; set; }
    }
}

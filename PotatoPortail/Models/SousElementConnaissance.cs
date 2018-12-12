namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SousElementConnaissance")]
    public partial class SousElementConnaissance
    {
        [Key]
        public int IdSousElement { get; set; }

        [Column(TypeName = "text")]
        public string DescSousElement { get; set; }

        public int? IdElementConnaissance { get; set; }

        public virtual ElementConnaissance ElementConnaissance { get; set; }
    }
}

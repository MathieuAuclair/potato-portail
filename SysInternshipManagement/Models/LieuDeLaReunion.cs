namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LieuDeLaReunion")]
    public partial class LieuDeLaReunion
    {
        [Key]
        public int IdLieu { get; set; }

        public string EmplacementReunion { get; set; }
    }
}

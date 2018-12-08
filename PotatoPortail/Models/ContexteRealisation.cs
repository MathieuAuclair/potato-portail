namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContexteRealisation")]
    public partial class ContexteRealisation
    {
        [Key]
        public int IdContexte { get; set; }

        [Required]
        [StringLength(300)]
        public string Description { get; set; }

        public int Numero { get; set; }

        public int IdCompetence { get; set; }

        public virtual EnonceCompetence EnonceCompetence { get; set; }
    }
}

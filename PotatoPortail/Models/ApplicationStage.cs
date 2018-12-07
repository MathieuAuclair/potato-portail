namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Application")]
    public partial class Application
    {
        [Key]
        public int IdApplication { get; set; }

        public DateTime Timestamp { get; set; }

        public virtual Etudiant Etudiant { get; set; }

        public virtual Stage Stage { get; set; }
    }
}

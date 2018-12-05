namespace SysInternshipManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Rangs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rangs()
        {
            HistoriqueRangs = new HashSet<HistoriqueRangs>();
        }

        public int Id { get; set; }

        [Required]
        public string NomRang { get; set; }

        [Required]
        [StringLength(6)]
        public string Abreviation { get; set; }

        public int Hierarchie { get; set; }

        public int IdJeu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoriqueRangs> HistoriqueRangs { get; set; }

        public virtual Jeux Jeux { get; set; }
    }
}

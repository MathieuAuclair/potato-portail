namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Rang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rang()
        {
            HistoriqueRangs = new HashSet<HistoriqueRang>();
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
        public virtual ICollection<HistoriqueRang> HistoriqueRangs { get; set; }

        public virtual Jeu Jeux { get; set; }
    }
}

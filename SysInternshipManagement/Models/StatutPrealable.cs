namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StatutPrealable")]
    public partial class StatutPrealable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StatutPrealable()
        {
            PlanCadrePrealable = new HashSet<PlanCadrePrealable>();
        }

        [Key]
        public int IdStatut { get; set; }

        [Required]
        [StringLength(20)]
        public string Statut { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCadrePrealable> PlanCadrePrealable { get; set; }
    }
}

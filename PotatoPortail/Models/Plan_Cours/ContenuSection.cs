namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContenuSection")]
    public partial class ContenuSection
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContenuSection()
        {
            PlanCours = new HashSet<PlanCours>();
        }

        [Key]
        public int idContenuSection { get; set; }

        [Required]
        [StringLength(1000)]
        public string texteContenu { get; set; }

        public int idNomSection { get; set; }

        public bool modifiable { get; set; }

        public virtual NomSection NomSection { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCours> PlanCours { get; set; }
    }
}

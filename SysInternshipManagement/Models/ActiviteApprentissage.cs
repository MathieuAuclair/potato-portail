namespace ApplicationPlanCadre.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ActiviteApprentissage")]
    public partial class ActiviteApprentissage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ActiviteApprentissage()
        {
            ElementConnaissance = new HashSet<ElementConnaissance>();
        }

        [Key]
        public int idActiviteApprentissage { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ElementConnaissance> ElementConnaissance { get; set; }
    }
}

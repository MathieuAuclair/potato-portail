namespace ApplicationPlanCadre.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ElementConnaissance")]
    public partial class ElementConnaissance
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ElementConnaissance()
        {
            PlanCadreElement = new HashSet<PlanCadreElement>();
        }

        [Key]
        public int idElementConnaissance { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string description { get; set; }

        public int idActiviteApprentissage { get; set; }

        public virtual ActiviteApprentissage ActiviteApprentissage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCadreElement> PlanCadreElement { get; set; }
    }
}

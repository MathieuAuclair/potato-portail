namespace PotatoPortail.Models
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
            SousElementConnaissance = new HashSet<SousElementConnaissance>();
        }

        [Key]
        public int IdElementConnaissance { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Description { get; set; }

        public int? IdPlanCadreElement { get; set; }

        public virtual PlanCadreElement PlanCadreElement { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SousElementConnaissance> SousElementConnaissance { get; set; }
    }
}

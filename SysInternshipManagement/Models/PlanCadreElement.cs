namespace SysInternshipManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PlanCadreElement")]
    public partial class PlanCadreElement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlanCadreElement()
        {
            ActiviteApprentissage = new HashSet<ActiviteApprentissage>();
            ElementConnaissance = new HashSet<ElementConnaissance>();
        }

        [Key]
        public int IdPlanCadreElement { get; set; }

        public int IdElement { get; set; }

        public int IdPlanCadreCompetence { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ActiviteApprentissage> ActiviteApprentissage { get; set; }

        public virtual ElementCompetence ElementCompetence { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ElementConnaissance> ElementConnaissance { get; set; }

        public virtual PlanCadreCompetence PlanCadreCompetence { get; set; }
    }
}

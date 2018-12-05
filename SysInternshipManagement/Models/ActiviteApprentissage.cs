namespace SysInternshipManagement.Models
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
            SousActiviteApprentissage = new HashSet<SousActiviteApprentissage>();
        }

        [Key]
        public int IdActivite { get; set; }

        [Column(TypeName = "text")]
        public string DescActivite { get; set; }

        public int IdPlanCadreElement { get; set; }

        public virtual PlanCadreElement PlanCadreElement { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SousActiviteApprentissage> SousActiviteApprentissage { get; set; }
    }
}

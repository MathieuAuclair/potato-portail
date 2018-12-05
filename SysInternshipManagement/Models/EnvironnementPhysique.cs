namespace SysInternshipManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EnvironnementPhysique")]
    public partial class EnvironnementPhysique
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EnvironnementPhysique()
        {
            SousEnvironnementPhysique = new HashSet<SousEnvironnementPhysique>();
        }

        [Key]
        public int IdEnvPhysique { get; set; }

        [Column(TypeName = "text")]
        public string NomEnvPhys { get; set; }

        public int IdPlanCadre { get; set; }

        public virtual PlanCadre PlanCadre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SousEnvironnementPhysique> SousEnvironnementPhysique { get; set; }
    }
}

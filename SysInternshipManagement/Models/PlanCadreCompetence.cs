namespace SysInternshipManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PlanCadreCompetence")]
    public partial class PlanCadreCompetence
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlanCadreCompetence()
        {
            PlanCadreElement = new HashSet<PlanCadreElement>();
        }

        [Key]
        public int IdPlanCadreCompetence { get; set; }

        public int PonderationEnHeure { get; set; }

        public int IdPlanCadre { get; set; }

        public int IdCompetence { get; set; }

        public virtual EnonceCompetence EnonceCompetence { get; set; }

        public virtual PlanCadre PlanCadre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCadreElement> PlanCadreElement { get; set; }
    }
}

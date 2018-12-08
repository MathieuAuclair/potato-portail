namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cours
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cours()
        {
            PlanCours = new HashSet<PlanCours>();
        }

        [Key]
        public int IdCours { get; set; }

        public int IdPlanCadre { get; set; }

        public int IdGrille { get; set; }

        public int IdSession { get; set; }

        public virtual GrilleCours GrilleCours { get; set; }

        public virtual PlanCadre PlanCadre { get; set; }

        public virtual Session Session { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCours> PlanCours { get; set; }
    }
}

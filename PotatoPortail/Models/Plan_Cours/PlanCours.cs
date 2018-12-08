namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PlanCours
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlanCours()
        {
            PlanCoursDepart = new HashSet<PlanCoursDepart>();
            PlanCoursUtilisateur = new HashSet<PlanCoursUtilisateur>();
            ContenuSection = new HashSet<ContenuSection>();
        }

        [Key]
        public int idPlanCours { get; set; }

        [Column(TypeName = "date")]
        public DateTime dateCreation { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dateValidation { get; set; }

        public bool StatutPlanCours { get; set; }

        public int idCours { get; set; }

        public virtual Cours Cours { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCoursDepart> PlanCoursDepart { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCoursUtilisateur> PlanCoursUtilisateur { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContenuSection> ContenuSection { get; set; }
    }
}

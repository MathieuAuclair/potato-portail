using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PotatoPortail.Models.Plan_Cours
{
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
        public int IdPlanCours { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateCreation { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateValidation { get; set; }

        public bool StatutPlanCours { get; set; }

        public int IdCours { get; set; }

        public virtual Cours Cours { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCoursDepart> PlanCoursDepart { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCoursUtilisateur> PlanCoursUtilisateur { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContenuSection> ContenuSection { get; set; }
    }
}

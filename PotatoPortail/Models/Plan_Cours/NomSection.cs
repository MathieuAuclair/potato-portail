using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PotatoPortail.Models.Plan_Cours
{
    [Table("NomSection")]
    public partial class NomSection
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NomSection()
        {
            ContenuSection = new HashSet<ContenuSection>();
            PlanCoursDepart = new HashSet<PlanCoursDepart>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdNomSection { get; set; }

        [Required]
        [StringLength(75)]
        public string TitreSection { get; set; }
        
        [StringLength(3)]
        public string Obligatoire { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContenuSection> ContenuSection { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCoursDepart> PlanCoursDepart { get; set; }
    }
}

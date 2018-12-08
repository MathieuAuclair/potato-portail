namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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
        public int idNomSection { get; set; }

        [Required]
        [StringLength(75)]
        public string titreSection { get; set; }

        [Required]
        [StringLength(3)]
        public string obligatoire { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContenuSection> ContenuSection { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCoursDepart> PlanCoursDepart { get; set; }
    }
}

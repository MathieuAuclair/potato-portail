namespace ApplicationPlanCadre.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ElementCompetence")]
    public partial class ElementCompetence
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ElementCompetence()
        {
            CriterePerformance = new HashSet<CriterePerformance>();
            PlanCadreElement = new HashSet<PlanCadreElement>();
        }

        [Key]
        public int idElement { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "Élément de compétence")]
        public string description { get; set; }

        [Display(Name = "Numéro")]
        public int numero { get; set; }

        public int idCompetence { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CriterePerformance> CriterePerformance { get; set; }

        public virtual EnonceCompetence EnonceCompetence { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCadreElement> PlanCadreElement { get; set; }
    }
}

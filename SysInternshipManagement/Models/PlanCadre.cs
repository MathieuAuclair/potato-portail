namespace SysInternshipManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PlanCadre")]
    public partial class PlanCadre
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlanCadre()
        {
            Cours = new HashSet<Cours>();
            EnvironnementPhysique = new HashSet<EnvironnementPhysique>();
            RessourceDIdactique = new HashSet<RessourceDIdactique>();
            PlanCadreCompetence = new HashSet<PlanCadreCompetence>();
            PlanCadrePrealable = new HashSet<PlanCadrePrealable>();
        }

        [Key]
        public int IdPlanCadre { get; set; }

        [StringLength(10)]
        public string NumeroCours { get; set; }

        [Required]
        [StringLength(150)]
        public string TitreCours { get; set; }

        [Column(TypeName = "text")]
        public string IndicationPedago { get; set; }

        public int? NbHeureTheorie { get; set; }

        public int? NbHeurePratique { get; set; }

        public int? NbHeureDevoir { get; set; }

        public int IdProgramme { get; set; }

        public int IdType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cours> Cours { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnvironnementPhysique> EnvironnementPhysique { get; set; }

        public virtual Programme Programme { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RessourceDIdactique> RessourceDIdactique { get; set; }

        public virtual TypePlanCadre TypePlanCadre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCadreCompetence> PlanCadreCompetence { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCadrePrealable> PlanCadrePrealable { get; set; }
    }
}

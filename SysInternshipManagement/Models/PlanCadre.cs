namespace ApplicationPlanCadre.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Web.Mvc;

    [Table("PlanCadre")]
    public partial class PlanCadre
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlanCadre()
        {
            Cours = new HashSet<Cours>();
            PlanCadrePrealable = new HashSet<PlanCadrePrealable>();
            PlanCadreElement = new HashSet<PlanCadreElement>();
            PlanCadreEnonce = new HashSet<PlanCadreEnonce>();
            Prealable = new HashSet<PlanCadrePrealable>();
        }

        [Key]
        public int idPlanCadre { get; set; }

        public string nom
        {
            get { return numeroCours + " " + titreCours; }
        }

        [StringLength(10)]
        [Display(Name = "Code de cours")]
        public string numeroCours { get; set; }

        [Required]
        [Display(Name = "Titre du cour")]
        [StringLength(150)]
        public string titreCours { get; set; }

        [AllowHtml]
        [Display(Name = "Indication pédagogique")]
        [Column(TypeName = "text")]
        public string indicationPedago { get; set; }

        [AllowHtml]
        [Display(Name = "Environement physique")]
        [Column(TypeName = "text")]
        public string environnementPhys { get; set; }

        [AllowHtml]
        [Display(Name = "Ressource/réfférences")]
        [Column(TypeName = "text")]
        public string ressource { get; set; }


        [Display(Name = "Heure de théorie")]
        public int? nbHeureTheorie { get; set; }

        [Display(Name = "Heure de pratique")]
        public int? nbHeurePratique { get; set; }

        [Display(Name = "Heure de devoir")]
        public int? nbHeureDevoir { get; set; }

        public int idProgramme { get; set; }

        public int idType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cours> Cours { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCadrePrealable> PlanCadrePrealable { get; set; }

        public IEnumerable<PlanCadre> PlanCadrePrealableA
        {
            get
            {
                List<PlanCadrePrealable> prealable = new List<Models.PlanCadrePrealable>();
                foreach(PlanCadre planCadre in Programme.PlanCadre)
                {
                    prealable.AddRange(planCadre.PlanCadrePrealable);
                }
                var shit = (from pc in Programme.PlanCadre
                       join pcp in prealable on idPlanCadre equals pcp.idPrealable
                       select pc).ToList();
                return shit;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCadreElement> PlanCadreElement { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCadreEnonce> PlanCadreEnonce { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCadrePrealable> Prealable { get; set; }

        public virtual Programme Programme { get; set; }

        public virtual TypePlanCadre TypePlanCadre { get; set; }
    }
}

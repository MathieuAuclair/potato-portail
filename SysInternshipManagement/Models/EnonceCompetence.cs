namespace ApplicationPlanCadre.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EnonceCompetence")]
    public partial class EnonceCompetence
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EnonceCompetence()
        {
            ContexteRealisation = new HashSet<ContexteRealisation>();
            ElementCompetence = new HashSet<ElementCompetence>();
            PlanCadreEnonce = new HashSet<PlanCadreEnonce>();
        }

        [Key]
        public int idCompetence { get; set; }

        public string nom
        {
            get { return codeCompetence + " • " + description; }
        }

        [Required]
        [StringLength(4)]
        [Display(Name = "Code de la compétence")]
        [RegularExpression("^[0-9][0-9][0-9][0-9|A-Za-z]$", ErrorMessage = "Le code est invalide, il doit être composé de 3 chiffres suivis d'une lettre ou d'un chiffre.")]
        public string codeCompetence { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "Énoncé de compétence")]
        public string description { get; set; }

        [Display(Name = "Obligatoire")]
        public bool obligatoire { get; set; }

        [Display(Name = "Actif")]
        public bool actif { get; set; }

        public bool specifique { get; set; }

        public int idDevis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContexteRealisation> ContexteRealisation { get; set; }

        public virtual DevisMinistere DevisMinistere { get; set; }




        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ElementCompetence> ElementCompetence { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCadreEnonce> PlanCadreEnonce { get; set; }
    }
}

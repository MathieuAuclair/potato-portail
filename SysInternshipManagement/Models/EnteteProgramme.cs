namespace ApplicationPlanCadre.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EnteteProgramme")]
    public partial class EnteteProgramme
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EnteteProgramme()
        {
            DevisMinistere = new HashSet<DevisMinistere>();
        }

        [Key]
        [StringLength(3)]
        [Display(Name = "Code")]
        public string codeProgramme { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Nom")]
        public string nom { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DevisMinistere> DevisMinistere { get; set; }
    }
}

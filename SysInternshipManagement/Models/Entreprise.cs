namespace SysInternshipManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Entreprise")]
    public partial class Entreprise
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Entreprise()
        {
            Contact = new HashSet<Contact>();
        }

        [Key]
        public int IdEntreprise { get; set; }

        [Required]
        public string Nom { get; set; }

        public int NumeroCivique { get; set; }

        [Required]
        public string Rue { get; set; }

        [Required]
        public string Ville { get; set; }

        [Required]
        public string Province { get; set; }

        [Required]
        public string Pays { get; set; }

        [Required]
        public string CodePostal { get; set; }

        public int? Preference_IdPreference { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contact> Contact { get; set; }
    }
}

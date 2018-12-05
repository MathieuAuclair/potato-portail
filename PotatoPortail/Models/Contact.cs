namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contact")]
    public partial class Contact
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contact()
        {
            Stage = new HashSet<Stage>();
        }

        [Key]
        public int IdContact { get; set; }

        [Required]
        public string Nom { get; set; }

        [Required]
        public string Telephone { get; set; }

        [Required]
        public string Courriel { get; set; }

        public int Entreprise_IdEntreprise { get; set; }

        public virtual Entreprise Entreprise { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stage> Stage { get; set; }
    }
}

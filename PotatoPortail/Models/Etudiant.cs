namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Etudiant")]
    public partial class Etudiant
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Etudiant()
        {
            Application = new HashSet<Application>();
        }

        [Key]
        public int IdEtudiant { get; set; }

        public string NomComplet => Prenom + NomComplet;

        [Required]
        public string Prenom { get; set; }

        [Required]
        public string NomDeFamille { get; set; }

        [Required]
        public string CourrielEcole { get; set; }

        [Required]
        public string CourrielPersonnel { get; set; }

        [Required]
        public string Telephone { get; set; }

        [Required]
        public string NumeroDa { get; set; }

        [Required]
        public string CodePermanent { get; set; }

        [Required]
        public string Role { get; set; }

        public int? Preference_IdPreference { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Application> Application { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual Preference Preference { get; set; }
    }
}

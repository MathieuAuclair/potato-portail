namespace SysInternshipManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Entraineurs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Entraineurs()
        {
            Equipes = new HashSet<Equipes>();
        }

        public int Id { get; set; }

        [Required]
        public string NomEntraineur { get; set; }

        [Required]
        public string PrenomEntraineur { get; set; }

        [Required]
        public string PseudoEntraineur { get; set; }

        [Required]
        public string NumeroTelephone { get; set; }

        [Required]
        public string AdresseCourriel { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Equipes> Equipes { get; set; }
    }
}

namespace SysInternshipManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Joueurs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Joueurs()
        {
            HistoriqueRangs = new HashSet<HistoriqueRangs>();
            Equipes = new HashSet<Equipes>();
            Items = new HashSet<Items>();
        }

        public int Id { get; set; }

        [Required]
        public string PseudoJoueur { get; set; }

        public int IdEtudiant { get; set; }

        public int IdProfil { get; set; }

        [Required]
        [StringLength(128)]
        public string IdMembreESports { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoriqueRangs> HistoriqueRangs { get; set; }

        public virtual MembreESports MembreESports { get; set; }

        public virtual Profils Profils { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Equipes> Equipes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Items> Items { get; set; }
    }
}

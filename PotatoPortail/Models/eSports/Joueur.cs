namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Joueur
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Joueur()
        {
            HistoriqueRangs = new HashSet<HistoriqueRang>();
            Equipes = new HashSet<Equipe>();
            Items = new HashSet<Item>();
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
        public virtual ICollection<HistoriqueRang> HistoriqueRangs { get; set; }

        public virtual MembreESports MembreESports { get; set; }

        public virtual Profil Profils { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Equipe> Equipes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item> Items { get; set; }
    }
}

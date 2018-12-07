namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Profil
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Profil()
        {
            Joueurs = new HashSet<Joueur>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string Pseudo { get; set; }

        [Required]
        [StringLength(128)]
        public string IdMembreESports { get; set; }

        public int IdJeu { get; set; }

        public string Courriel { get; set; }

        public string Note { get; set; }

        public bool EstArchive { get; set; }

        public int? IdJeuSecondaire { get; set; }

        public virtual Jeu Jeux { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Joueur> Joueurs { get; set; }

        public virtual MembreESports MembreESports { get; set; }
    }
}

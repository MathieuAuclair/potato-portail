namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;

    public partial class MembreESports
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MembreESports()
        {
            Joueurs = new HashSet<Joueur>();
            Profils = new HashSet<Profil>();
        }

        public string NomComplet => Prenom + Nom;

        public string Id { get; set; }

        public string Nom { get; set; }

        public string Prenom { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Joueur> Joueurs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Profil> Profils { get; set; }
    }
}

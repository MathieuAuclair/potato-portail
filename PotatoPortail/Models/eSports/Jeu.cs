namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Jeux")]
    public partial class Jeu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Jeu()
        {
            Caracteristiques = new HashSet<Caracteristique>();
            Equipes = new HashSet<Equipe>();
            Profils = new HashSet<Profils>();
            Rangs = new HashSet<Rangs>();
        }

        public int Id { get; set; }

        [Required]
        public string NomJeu { get; set; }

        public string Description { get; set; }

        public string UrlReference { get; set; }

        [Required]
        [StringLength(6)]
        public string Abreviation { get; set; }

        public int IdStatuts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Caracteristiques> Caracteristiques { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Equipes> Equipes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Profils> Profils { get; set; }

        public virtual Statuts Statuts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rangs> Rangs { get; set; }
    }
}

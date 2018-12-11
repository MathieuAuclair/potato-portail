namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DevisMinistere")]
    public partial class DevisMinistere
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DevisMinistere()
        {
            EnonceCompetence = new HashSet<EnonceCompetence>();
            Programme = new HashSet<Programme>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDevis { get; set; }

        [Required]
        [StringLength(4)]
        public string Annee { get; set; }

        [Required]
        [StringLength(3)]
        public string CodeSpecialisation { get; set; }

        [StringLength(100)]
        public string Specialisation { get; set; }

        [StringLength(6)]
        public string NbUnite { get; set; }

        public int? NbHeureFrmGenerale { get; set; }

        public int? NbHeureFrmSpecifique { get; set; }

        [StringLength(300)]
        public string Condition { get; set; }

        [StringLength(150)]
        public string Sanction { get; set; }

        [StringLength(250)]
        public string DocMinistere { get; set; }

        [Required]
        [StringLength(3)]
        public string Discipline { get; set; }

        public virtual Departement Departement { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnonceCompetence> EnonceCompetence { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Programme> Programme { get; set; }
        public string Nom
        {
            get
            {
                if (Specialisation != "N/A")
                    return CodeDevis + " • " + Specialisation;
                return CodeDevis;
            }
        }

        public string CodeDevis => Discipline + "-" + Annee + "-" + CodeSpecialisation;
    }
}

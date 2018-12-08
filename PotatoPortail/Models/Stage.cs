namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Stage")]
    public partial class Stage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Stage()
        {
            Application = new HashSet<Application>();
        }

        [Key]
        public int IdStage { get; set; }

        public string Description { get; set; }

        [Required]
        public string CodePostal { get; set; }

        public int NumeroCivique { get; set; }

        [Required]
        public string NomRue { get; set; }

        [Required]
        public string Ville { get; set; }

        [Required]
        public string Province { get; set; }

        [Required]
        public string Pays { get; set; }

        public float Salaire { get; set; }

        public string NomDocument { get; set; }

        public int? Contact_IdContact { get; set; }

        public int? Location_IdLocation { get; set; }

        public int? Poste_IdPoste { get; set; }

        public int? StatutStage_IdStatutStage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Application> Application { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual Location Location { get; set; }

        public virtual Poste Poste { get; set; }

        public virtual StatutStage StatutStage { get; set; }
    }
}

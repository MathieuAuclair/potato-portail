namespace ApplicationPlanCadre.Models.Reunions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("OrdreDuJour")]
    public partial class OrdreDuJour
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrdreDuJour()
        {
            SujetPointPrincipal = new HashSet<SujetPointPrincipal>();
        }

        [Key]
        public int idOdJ { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Titre")]
        public string titreOdJ { get; set; }

        [DisplayName("Date de la réunion")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "yyyy-mm-dd", ApplyFormatInEditMode = true)]
        public DateTime dateOdJ { get; set; }

        [Required]
        [DisplayName("Heure de début")]
        [StringLength(10)]
        public string heureDebutReunion { get; set; }

        [Required]
        [DisplayName("Heure de fin")]
        [StringLength(10)]
        public string heureFinReunion { get; set; }

        [DisplayName("Lieu de la réunion")]
        [StringLength(10)]
        public string lieuReunionODJ { get; set; }

        public int idCreateurOdJ { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SujetPointPrincipal> SujetPointPrincipal { get; set; }
    }
}
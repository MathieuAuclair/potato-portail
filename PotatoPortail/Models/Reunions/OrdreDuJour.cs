using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PotatoPortail.Models.Reunions
{
    [Table("OrdreDuJour")]
    public partial class OrdreDuJour
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrdreDuJour()
        {
            SujetPointPrincipal = new HashSet<SujetPointPrincipal>();
        }

        [Key]
        public int IdOdJ { get; set; }

        [Required]
        [StringLength(50)]
        public string TitreOdJ { get; set; }

        [DisplayName("Date de la réunion")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime DateOdJ { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Début de la réunion : ")]
        public string HeureDebutReunion { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Fin de la réunion : ")]
        public string HeureFinReunion { get; set; }

        [StringLength(100)]
        [Display(Name = "Lieu de la réunion : ")]
        public string LieuReunionODJ { get; set; }

        public int IdModeleOrdreDuJour { get; set; }

       // public virtual ModeleOrdreDuJour ModeleOrdreDuJour { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SujetPointPrincipal> SujetPointPrincipal { get; set; }
    }
}
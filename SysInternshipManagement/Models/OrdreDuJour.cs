namespace SysInternshipManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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

        [Column(TypeName = "date")]
        public DateTime DateOdJ { get; set; }

        [Required]
        [StringLength(50)]
        public string HeureDebutReunion { get; set; }

        [Required]
        [StringLength(50)]
        public string HeureFinReunion { get; set; }

        [StringLength(50)]
        public string LieuReunionODJ { get; set; }

        public int IdModeleOrdreDuJour { get; set; }

        public virtual ModeleOrdreDuJour ModeleOrdreDuJour { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SujetPointPrincipal> SujetPointPrincipal { get; set; }
    }
}

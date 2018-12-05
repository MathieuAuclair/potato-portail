namespace SysInternshipManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ModeleOrdreDuJour")]
    public partial class ModeleOrdreDuJour
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ModeleOrdreDuJour()
        {
            OrdreDuJour = new HashSet<OrdreDuJour>();
        }

        [Key]
        public int IdModele { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; }

        public int NumeroProgramme { get; set; }

        [Required]
        [StringLength(100)]
        public string PointPrincipal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdreDuJour> OrdreDuJour { get; set; }
    }
}

namespace SysInternshipManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SujetPointPrincipal")]
    public partial class SujetPointPrincipal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SujetPointPrincipal()
        {
            SousPointSujet = new HashSet<SousPointSujet>();
        }

        [Key]
        public int IdPointPrincipal { get; set; }

        [Required]
        [StringLength(50)]
        public string SujetPoint { get; set; }

        public int? PositionPP { get; set; }

        public int IdOrdreDuJour { get; set; }

        public virtual OrdreDuJour OrdreDuJour { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SousPointSujet> SousPointSujet { get; set; }
    }
}

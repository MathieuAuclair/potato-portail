using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PotatoPortail.Models.Reunions
{
    [Table("ModeleOrdreDuJour")]
    public partial class ModeleOrdreDuJour
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
       

        [Key]
        public int IdModele { get; set; }

        [Required]
        [StringLength(1)]
        public string Role { get; set; }

        public int NumeroProgramme { get; set; }

        public string PointPrincipal { get; set; }

        //      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        // public virtual ICollection<OrdreDuJour> OrdreDuJour { get; set; }
    }
}

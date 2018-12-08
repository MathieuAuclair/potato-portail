using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PotatoPortail.Models.Reunions
{
    [Table("SousPointSujet")]
    public partial class SousPointSujet
    {
        [Key]
        public int IdSousPoint { get; set; }

        [Required]
        [StringLength(50)]
        public string SujetSousPoint { get; set; }

        public int IdSujetPointPrincipal { get; set; }

        public virtual SujetPointPrincipal SujetPointPrincipal { get; set; }
    }
}

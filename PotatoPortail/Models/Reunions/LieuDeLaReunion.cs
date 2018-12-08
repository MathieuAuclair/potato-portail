using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PotatoPortail.Models.Reunions
{
    [Table("LieuDeLaReunion")]
    public partial class LieuDeLaReunion
    {
        [Key]
        public int IdLieu { get; set; }

        public string EmplacementReunion { get; set; }
    }
}

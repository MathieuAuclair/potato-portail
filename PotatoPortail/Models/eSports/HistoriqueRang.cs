using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PotatoPortail.Models.eSports
{
    public partial class HistoriqueRang
    {
        public HistoriqueRang()
        {

        }

        [Key]
        [Column(Order = 0)]
        public int IdJoueur { get; set; }

        [Key]
        [Column(Order = 1)]
        public int IdRang { get; set; }

        public DateTime Date { get; set; }

        public virtual Joueur Joueurs { get; set; }

        public virtual Rang Rangs { get; set; }
    }
}

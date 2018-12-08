using PotatoPortail.Models.eSports;

namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Item
    {
        public Item()
        {
            Joueur = new HashSet<Joueur>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Nom de l'item")]
        public string NomItem { get; set; }

        [Display(Name = "Caractéristique")]
        public int IdCaracteristique { get; set; }

        public virtual Caracteristique Caracteristique { get; set; }

        public virtual ICollection<Joueur> Joueur { get; set; }
    }
}

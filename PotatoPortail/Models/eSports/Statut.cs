using PotatoPortail.Models.eSports;

namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Statut
    {
        public Statut()
        {

        }

        public int Id { get; set; }

        [Required]
        public string NomStatut { get; set; }

        public virtual ICollection<Jeu> Jeu { get; set; }
    }
}

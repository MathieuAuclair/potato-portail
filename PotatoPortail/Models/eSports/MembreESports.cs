using System.ComponentModel.DataAnnotations;
using PotatoPortail.Models.eSports;

namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;

    public partial class MembreESports
    {
        public MembreESports()
        {

        }

        public string Id { get; set; }

        [Display(Name = "Nom")]
        public string Nom { get; set; }

        [Display(Name = "Prénom")]
        public string Prenom { get; set; }

        public virtual ICollection<Joueur> Joueur { get; set; }

        public virtual ICollection<Profil> Profil { get; set; }

        public string NomComplet
        {
            get
            {
                if (Prenom != null)
                    return Prenom + " " + Nom;
                else
                    return Nom;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using PotatoPortail.Models;

namespace PotatoPortail.Views.SystemeStage.Stage.EditionComponent.Models
{
    public class EntrepriseSelect
    {
        public List<Entreprise> Entreprises { get; set; }
        private readonly BDPortail _bd = new BDPortail();

        public EntrepriseSelect()
        {
            Entreprises = _bd.Entreprise.ToList();
        }
    }
}
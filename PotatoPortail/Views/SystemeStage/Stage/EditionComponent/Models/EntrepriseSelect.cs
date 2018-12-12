using System.Collections.Generic;
using System.Linq;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Views.SystemeStage.Stage.EditionComponent.Models
{
    public class EntrepriseSelect
    {
        public List<Entreprise> Entreprises { get; set; }
        private readonly BdPortail _db = new BdPortail();

        public EntrepriseSelect()
        {
            Entreprises = _db.Entreprise.ToList();
        }
    }
}
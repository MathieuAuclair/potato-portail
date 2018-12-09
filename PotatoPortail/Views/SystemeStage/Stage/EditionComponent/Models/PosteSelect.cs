using System.Collections.Generic;
using System.Linq;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Views.SystemeStage.Stage.EditionComponent.Models

    public class PosteSelect
    {
        public List<Poste> Postes { get; set; }
        private readonly BdPortail _db = new BdPortail();

        public PosteSelect()
        {
            Postes = _db.Poste.ToList();
        }
    }
}
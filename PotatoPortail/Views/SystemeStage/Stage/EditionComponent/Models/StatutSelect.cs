using System.Collections.Generic;
using System.Linq;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Views.SystemeStage.Stage.EditionComponent.Models
{
    public class StatutSelect
    {
        public List<StatutStage> StatutStage { get; set; }
        private readonly BdPortail _db = new BdPortail();

        public StatutSelect()
        {
            StatutStage = _db.StatutStage.ToList();
        }
    }
}
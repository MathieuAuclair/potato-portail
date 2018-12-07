using System.Collections.Generic;
using System.Linq;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Views.SystemeStage.Stage.EditionComponent.Models
{
    public class StatusSelect
    {
        public List<StatutStage> Status { get; set; }
        private readonly BdPortail _bd = new BdPortail();

        public StatusSelect()
        {
            Status = _bd.StatutStage.ToList();
        }
    }
}
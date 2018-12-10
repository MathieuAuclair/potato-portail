using System.Collections.Generic;
using System.Linq;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Views.SystemeStage.Stage.EditionComponent.Models
{
    public class LocationSelect
    {
        public List<Location> Locations { get; set; }
        private readonly BdPortail _db = new BdPortail();

        public LocationSelect()
        {
            Locations = _db.Location.ToList();
        }
    }
}
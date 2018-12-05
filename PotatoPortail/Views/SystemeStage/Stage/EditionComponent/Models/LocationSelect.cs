using System.Collections.Generic;
using System.Linq;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Views.SystemeStage.Stage.EditionComponent.Models
{
    public class LocationSelect
    {
        public List<Location> Locations { get; set; }
        private readonly DatabaseContext _bd = new DatabaseContext();
        
        public LocationSelect()
        {
            Locations = _bd.Location.ToList();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Views.Stage.EditionComponent.Models
{
    public class LocationSelect
    {
        public List<Location> Locations { get; set; }
        private DatabaseContext _bd = new DatabaseContext();
        
        public LocationSelect()
        {
            Locations = _bd.Location.ToList();
        }
    }
}
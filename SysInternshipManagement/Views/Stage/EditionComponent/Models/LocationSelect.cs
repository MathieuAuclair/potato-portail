using System.Linq;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Views.Stage.EditionComponent.Models
{
    public class LocationSelect
    {
        public IQueryable<Location> Locations { get; set; }
        private DatabaseContext _bd = new DatabaseContext();
        
        public LocationSelect()
        {
            Locations = (from entity in _bd.location
                    where entity.IdLocation == 1
                    select entity
                );
        }
    }
}
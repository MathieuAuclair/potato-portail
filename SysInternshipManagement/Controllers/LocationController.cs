using System.Collections.Generic;
using System.Linq;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Controllers
{
    public class LocationController
    {
        private readonly DatabaseContext _bd = new DatabaseContext();

        public IEnumerable<Location> RecolterTousLesLocations()
        {
            return (
                from entity
                    in _bd.location
                select entity
            );
        }
    }
}
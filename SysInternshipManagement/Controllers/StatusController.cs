using System.Collections.Generic;
using System.Linq;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Controllers
{
    public class StatusController 
    {
        private readonly DatabaseContext _bd = new DatabaseContext();

        public IEnumerable<Status> RecolterTousLesStatus()
        {
            return (
                from entity
                    in _bd.status
                select entity
            );
        }
    }
}
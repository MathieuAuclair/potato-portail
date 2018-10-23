using System.Linq;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Views.Stage.EditionComponent.Models
{
    public class StatusSelect
    {
        public IQueryable<Status> Status { get; set; }
        private DatabaseContext _bd = new DatabaseContext();

        public StatusSelect()
        {
            Status = (from entity in _bd.status
                    where entity.IdStatus == 1
                    select entity
                );
        }
    }
}
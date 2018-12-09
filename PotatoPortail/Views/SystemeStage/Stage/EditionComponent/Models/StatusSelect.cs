using System.Collections.Generic;
using System.Linq;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Views.Stage.EditionComponent.Models
{
    public class StatusSelect
    {
        public List<Status> Status { get; set; }
        private DatabaseContext _bd = new DatabaseContext();

        public StatusSelect()
        {
            Status = _bd.status.ToList();
        }
    }
}
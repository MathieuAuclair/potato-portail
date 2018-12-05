using System.Collections.Generic;
using System.Linq;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Views.Stage.EditionComponent.Models
{
    public class StatusSelect
    {
        public List<Status> Status { get; set; }
        private DatabaseContext _bd = new DatabaseContext();

        public StatusSelect()
        {
            Status = _bd.Status.ToList();
        }
    }
}
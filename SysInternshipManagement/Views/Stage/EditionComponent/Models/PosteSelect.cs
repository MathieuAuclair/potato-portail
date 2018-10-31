using System.Collections.Generic;
using System.Linq;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Views.Stage.EditionComponent.Models
{
    public class PosteSelect
    {
        public List<Poste> Postes { get; set; }
        private DatabaseContext _bd = new DatabaseContext();

        public PosteSelect()
        {
            Postes = _bd.poste.ToList();
        }
    }
}
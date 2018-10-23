using System.Linq;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Views.Stage.EditionComponent.Models
{
    public class PosteSelect
    {
        public IQueryable<Poste> Postes { get; set; }
        private DatabaseContext _bd = new DatabaseContext();

        public PosteSelect()
        {
            Postes = (from entity in _bd.poste
                    where entity.IdPoste == 1
                    select entity
                );
        }
    }
}
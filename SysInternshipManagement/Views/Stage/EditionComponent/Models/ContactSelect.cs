using System.Linq;
using SysInternshipManagement.Migrations;

namespace SysInternshipManagement.Views.Stage.EditionComponent.Models
{
    public class ContactSelect
    {
        public IQueryable<SysInternshipManagement.Models.Contact> Contacts { get; set; }
        private DatabaseContext _bd = new DatabaseContext();

        public ContactSelect()
        {
            Contacts = (from entity in _bd.contact
                    where entity.IdContact == 1
                    select entity
                );
        }
    }
}
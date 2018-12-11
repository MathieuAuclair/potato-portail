using System.Collections.Generic;
using System.Linq;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Views.SystemeStage.Stage.EditionComponent.Models
{
    public class ContactSelect
    {
        public List<Contact> Contacts { get; set; }
        private readonly BdPortail _db = new BdPortail();

        public ContactSelect()
        {
            Contacts = _db.Contact.ToList();
        }
    }
}
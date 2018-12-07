using System.Collections.Generic;
using System.Linq;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Views.SystemeStage.Stage.EditionComponent.Models
{
    public class ContactSelect
    {
        public List<Contact> Contacts { get; set; }
        private readonly BdPortail _bd = new BdPortail();

        public ContactSelect()
        {
            Contacts = _bd.Contact.ToList();
        }
    }
}
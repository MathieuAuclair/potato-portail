using System.Collections.Generic;
using System.Linq;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Views.Stage.EditionComponent.Models
{
    public class ContactSelect
    {
        public List<Contact> Contacts { get; set; }
        private readonly DatabaseContext _bd = new DatabaseContext();

        public ContactSelect()
        {
            Contacts = _bd.Contact.ToList();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Views.Stage.EditionComponent.Models
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
using System.Collections.Generic;
using System.Linq;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Controllers
{
    public class ContactController 
    {
        private readonly DatabaseContext _bd = new DatabaseContext();

        public IEnumerable<Contact> RecolterTousLesContacts()
        {
            return (
                from entity
                    in _bd.contact
                select entity
            );
        }
    }
}
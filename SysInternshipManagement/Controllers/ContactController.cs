using System.Linq;
using System.Net;
using System.Web.Mvc;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Controllers
{
    public class ContactController : Controller
    {
        private readonly DatabaseContext _bd = new DatabaseContext();

        [HttpGet]
        public ActionResult Index()
        {
            return View(_bd.contact.ToList());
        }     
        public ActionResult Modifier(int IdContact)
        {
            var contact = _bd.contact.Find(IdContact);

            if (contact == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(contact);
        }

        [HttpPost]
        public ActionResult EnregistrerLesModifications(
            int? idContact,
            string nom,
            string courriel,
            string telephone
        )
        {
           
            if (idContact==null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
           
            var contact = _bd.contact.Find(idContact);

            if (contact == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            contact.Courriel = courriel;
            contact.Nom = nom;
            contact.Telephone = telephone;
            

            _bd.SaveChanges();

            return RedirectToAction("Index", "Contact");
        }

        public ActionResult Creation()
        {
            var contact = new Contact
            {
                Courriel = "courriel@cegepjonquiere.ca",
                Nom = "Nouveau contact",
                Telephone = "123-456-7890"
            };
            _bd.contact.Add(contact);
            _bd.SaveChanges();
            return View("~/Views/Contact/Modifier.cshtml", contact);
        }

        public ActionResult Suppression(int? IdContact)
        {
            var contact = _bd.contact.Find(IdContact);

            if (contact == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var stagesAyantCeContact = from stage in _bd.stage
                                       where stage.Contact.IdContact == IdContact
                                       select stage;

            if (!stagesAyantCeContact.Any())
            {
                _bd.contact.Remove(contact);
                _bd.SaveChanges();
            }

            return RedirectToAction("Index", "Contact");
        }
    }
}
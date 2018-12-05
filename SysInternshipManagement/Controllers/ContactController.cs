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
            return View("~/Views/Contact/Index.cshtml", _bd.contact.ToList());
        }

        [HttpPost]
        public ActionResult Modifier(int? idContact)
        {

            var contact = _bd.contact.Find(idContact);

            if (contact == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View("~/Views/Contact/Modifier.cshtml", contact);
        }

        [HttpPost]
        public ActionResult EnregistrerLesModifications(
            int? idContact,
            string nom,
            string courriel,
            string telephone
        )
        {
            if (idContact == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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

        [HttpPost]
        public ActionResult Creation()
        {
            var contact = new Contact
            {
                Courriel = "courriel@cegepjonquiere.ca",
                Nom = "Nouveau contact",
                Telephone = "123-456-7890"
            };

            return View("~/Views/Contact/Modifier.cshtml", contact);
        }

        public ActionResult Suppression(int? id)
        {
            var contact = _bd.contact.Find(id);

            if (contact == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var stagesAyantCeContact = from stage in _bd.stage
                                       where stage.Contact.IdContact == id
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
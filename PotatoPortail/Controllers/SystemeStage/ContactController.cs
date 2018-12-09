using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers
{
    public class ContactController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

        [HttpGet]
        public ActionResult Index()
        {
            return View(_db.Contact.ToList());
        }     
        public ActionResult Modifier(int IdContact)
        {
            var contact = _db.Contact.Find(IdContact);

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
           
            var contact = _db.Contact.Find(idContact);

            if (contact == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            contact.Courriel = courriel;
            contact.Nom = nom;
            contact.Telephone = telephone;
            

            _db.SaveChanges();

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
            _db.Contact.Add(contact);
            _db.SaveChanges();
            return View("~/Views/Contact/Modifier.cshtml", contact);
        }

        public ActionResult Suppression(int? IdContact)
        {
            var contact = _db.Contact.Find(IdContact);

            if (contact == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var stagesAyantCeContact = from stage in _db.Stage
                                       where stage.Contact.IdContact == IdContact
                                       select stage;

            if (!stagesAyantCeContact.Any())
            {
                _db.Contact.Remove(contact);
                _db.SaveChanges();
            }

            return RedirectToAction("Index", "Contact");
        }
    }
}
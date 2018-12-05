using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers.SystemeStage
{
    public class ContactController : Controller
    {
        private readonly BdPortail _bd = new BdPortail();

        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/SystemeStage/Contact/Index.cshtml", _bd.Contact.ToList());
        }

        [HttpPost]
        public ActionResult Edition(int? idContact)
        {

            var contact = _bd.Contact.Find(idContact);

            if (contact == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View("~/Views/SystemeStage/Contact/Edition.cshtml", contact);
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

            var contact = _bd.Contact.Find(idContact);

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
                Telephone = "123-456-7890",
                Entreprise = _bd.Entreprise.First()
            };

            return View("~/Views/Contact/Edition.cshtml", contact);
        }

        public ActionResult Suppression(int? id)
        {
            var contact = _bd.Contact.Find(id);

            if (contact == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var stagesAyantCeContact = from stage in _bd.Stage
                                       where stage.Contact.IdContact == id
                                       select stage;

            if (!stagesAyantCeContact.Any())
            {
                _bd.Contact.Remove(contact);
                _bd.SaveChanges();
            }

            return RedirectToAction("Index", "Contact");
        }
    }
}
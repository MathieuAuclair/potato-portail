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
        public ActionResult Edition(int? idContact)
        {
            if (idContact == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var contact = _bd.contact.Find(idContact);

            if (contact == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View("~/Views/Contact/Edition.cshtml", contact);
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

            return RedirectToAction("Edition", "Contact");
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

            _bd.contact.Add(contact);
            _bd.SaveChanges();

            return View("~/Views/Contact/Edition.cshtml", contact);
        }
    }
}
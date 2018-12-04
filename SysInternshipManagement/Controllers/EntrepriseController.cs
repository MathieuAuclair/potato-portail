using System;
using System.Data.Entity.Migrations;
using SysInternshipManagement.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Controllers
{
    public class EntrepriseController : Controller
    {
        private readonly DatabaseContext _bd = new DatabaseContext();

        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/Entreprise/Index.cshtml", _bd.entreprise.ToList());
        }

        [HttpPost]
        public ActionResult Edition()
        {
            var entreprise = _bd.entreprise.Find(int.Parse(Request.Form["idEntreprise"]));

            if (entreprise == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View("~/Views/Entreprise/Edition.cshtml", entreprise);
        }

        [HttpPost]
        public ActionResult EnregistrerLesModifications()
        {
            if (!EstCeQueLaRequeteEstValidePourEnregistrerLesModifications())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var entreprise = new Entreprise
            {
                IdEntreprise = Convert.ToInt32(Request.Form["id"]),
                Pays = Request.Form["pays"],
                Province = Request.Form["province"],
                Ville = Request.Form["ville"],
                Rue = Request.Form["rue"],
                NumeroCivique = Convert.ToInt32(Request.Form["numeroCivique"]),
                CodePostal = Request.Form["codePostal"],
                Nom = Request.Form["nom"]
            };
            _bd.entreprise.AddOrUpdate(entreprise);
            _bd.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Creation()
        {
            var entreprise = new Entreprise
            {
                Nom = "Nom d'entreprise",
                Pays = "Canada",
                Province = "Quebec",
                Ville = "Saguenay",
                Rue = "Rue",
                CodePostal = "G7X7W2",
                NumeroCivique = 0,
            };
          

            return View("~/Views/Entreprise/Edition.cshtml", entreprise);
        }


        private bool EstCeQueLaRequeteEstValidePourEnregistrerLesModifications()
        {
            return !(
                
                int.TryParse(Request.Form["numeroCivique"], out _) &&
                Request.Form["id"] == null &&
                Request.Form["numeroCivique"] == null &&
                Request.Form["pays"] == null &&
                Request.Form["pays"] == null &&
                Request.Form["ville"] == null &&
                Request.Form["rue"] == null &&
                Request.Form["province"] == null &&
                Request.Form["codePostal"] == null
            );
        }
    }
}
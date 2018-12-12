using System;
using System.Data.Entity.Migrations;
using PotatoPortail.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers
{
    public class EntrepriseController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/SystemeStage/Entreprise/Index.cshtml", _db.Entreprise.ToList());
        }
        public ActionResult Modifier(int IdEntreprise)
        {
          
            var entreprise = _db.Entreprise.Find(IdEntreprise);

            if (entreprise == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View("~/Views/SystemeStage/Entreprise/Modifier.cshtml", entreprise);
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
            _db.Entreprise.AddOrUpdate(entreprise);
            _db.SaveChanges();

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

            return View("~/Views/SystemeStage/Entreprise/Modifier.cshtml", entreprise);
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
        public ActionResult Suppression(int? IdEntreprise)
        {
            var entreprise = _db.Entreprise.Find(IdEntreprise);

            if (entreprise == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }            
             _db.Entreprise.Remove(entreprise);
            _db.SaveChanges();

            return RedirectToAction("Index", "Entreprise");
        }
    }
}
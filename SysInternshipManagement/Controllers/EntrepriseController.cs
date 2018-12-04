using System;
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
            if (!EstCeQueLaRequeteEstValidePourUneEdition())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

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

            var entreprise = _bd.entreprise.Find(int.Parse(Request.Form["idEntreprise"]));

            if (entreprise == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            entreprise.Pays = Request.Form["pays"];
            entreprise.Province = Request.Form["province"];
            entreprise.Ville = Request.Form["ville"];
            entreprise.Rue = Request.Form["rue"];
            entreprise.NumeroCivique = Convert.ToInt32(Request.Form["numeroCivique"]);
            entreprise.CodePostal = Request.Form["codePostal"];
            entreprise.Nom = Request.Form["nom"];

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

            _bd.entreprise.Add(entreprise);
            _bd.SaveChanges();

            return View("~/Views/Entreprise/Edition.cshtml", entreprise);
        }

        private bool EstCeQueLaRequeteEstValidePourUneEdition()
        {
            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                int.Parse(Request.Form["idEntreprise"]);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool EstCeQueLaRequeteEstValidePourEnregistrerLesModifications()
        {
            return !(
                int.TryParse(Request.Form["id"], out _) &&
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

        public ActionResult Suppression(int? id)
        {
            var entreprise = _bd.entreprise.Find(id);

            if (entreprise == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            _bd.entreprise.Remove(entreprise);
            _bd.SaveChanges();

            return RedirectToAction("Index", "Entreprise");
        }
    }
}
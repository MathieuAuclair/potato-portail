using System;
using System.Collections.Generic;
using SysInternshipManagement.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Controllers
{
    public class EntrepriseController : Controller
    {
        private readonly DatabaseContext _bd = new DatabaseContext();

        public ActionResult Index()
        {
            return View(_bd.entreprise.ToList());
        }

        [HttpPost]
        public ActionResult Edition()
        {
            if (!EstCeQueLaRequeteEstValidePourUneEdition())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,
                    "Les paramètres fournis ne sont pas valide!");
            }

            int idEntreprise = Convert.ToInt32(Request.Form["id"] ?? Request.QueryString["id"]);
            
            Entreprise entreprise = (
                from entity
                    in _bd.entreprise
                where entity.IdEntreprise == idEntreprise
                select entity
            ).First();
            
            return View(entreprise);
        }

        [HttpPost]
        public ActionResult EnregistrerLesModifications()
        {
            if (!EstCeQueLaRequeteEstValidePourEnregistrerLesModifications())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,
                    "Les paramètres fournis ne sont pas valide!");
            }

            int IdEntreprise = Convert.ToInt32(Request.Form["id"]);
            
            Entreprise entreprise = (
                from entity
                    in _bd.entreprise
                where entity.IdEntreprise == IdEntreprise
                select entity
            ).First();

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
            Entreprise entreprise = new Entreprise
            {
                Nom = "Nom d'entreprise",
                Pays = "Pays",
                Province = "Province",
                Rue = "Rue",
                Ville = "Ville",
                CodePostal = "Code postal",
                NumeroCivique = 123,
            };

            _bd.entreprise.Add(entreprise);
            _bd.SaveChanges();

            return View("Edition", entreprise);
        }

        private bool EstCeQueLaRequeteEstValidePourUneEdition()
        {
            try
            {
                Convert.ToInt32(Request.Form["id"]);
            }
            catch (Exception e)
            {
                return false;
            }

            return (Request.Form["id"] != null);
        }

        private bool EstCeQueLaRequeteEstValidePourEnregistrerLesModifications()
        {
            try
            {
                Convert.ToInt32(Request.Form["id"]);
                Convert.ToInt32(Request.Form["numeroCivique"]);
            }
            catch (Exception e)
            {
                return false;
            }

            return !(
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
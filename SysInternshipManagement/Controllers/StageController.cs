using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Controllers
{
    public class StageController : Controller
    {
        private readonly DatabaseContext _bd = new DatabaseContext();

        [HttpGet]
        public ActionResult Edition()
        {
            if (Request.QueryString["IdStage"] == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.IdStage = Request.QueryString["IdStage"];
            ViewBag.Poste = _bd.poste.ToList();
            ViewBag.Contact = _bd.contact.ToList();
            ViewBag.Status = _bd.status.ToList();
            ViewBag.Location = _bd.location.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Edition(HttpPostedFileBase fichier)
        {
            string nomFichier = null;

            if (fichier != null && fichier.ContentLength > 0)
            {
                nomFichier = Path.GetFileName(fichier.FileName);
                var chemin = Path.Combine(Server.MapPath("~/DescriptionStage"), nomFichier ?? "sample.txt");
                fichier.SaveAs(chemin);
            }

            if (!EstCeQueLaRequeteContientLesParametres())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,
                    "Les paramètres fournis ne sont pas valide!");
            }

            Stage stage = (
                from entity in _bd.stage
                where entity.IdStage == 1
                select entity
            ).First();

            Poste poste = (
                from entity
                    in _bd.poste
                where entity.IdPoste == Convert.ToInt32(Request.Form["idPoste"])
                select entity
            ).First();

            Contact contact = (
                from entity
                    in _bd.contact
                where entity.IdContact == Convert.ToInt32(Request.Form["idContact"])
                select entity
            ).First();

            Status status = (
                from entity
                    in _bd.status
                where entity.IdStatus == Convert.ToInt32(Request.Form["idStatus"])
                select entity
            ).First();

            Location location = (
                from entity
                    in _bd.location
                where entity.IdLocation == Convert.ToInt32(Request.Form["idLocation"])
                select entity
            ).First();

            stage.Poste = poste;
            stage.Contact = contact;
            stage.Status = status;
            stage.Location = location;
            stage.Description = Request.Form["Description"];
            stage.NomDocument = nomFichier;
            stage.CodePostal = Request.Form["CodePostal"];
            stage.Salaire = Convert.ToSingle(Request.Form["Salaire"]);

            _bd.SaveChanges();

            ViewBag.Stage = _bd.stage.ToList();
            ViewBag.Poste = _bd.poste.ToList();
            ViewBag.Contact = _bd.contact.ToList();
            ViewBag.Status = _bd.status.ToList();
            ViewBag.Location = _bd.location.ToList();

            return RedirectToAction("Index");
        }

        private bool EstCeQueLaRequeteContientLesParametres()
        {
            return (
                Request.Form["IdPoste"] != null &&
                Request.Form["IdContact"] != null &&
                Request.Form["IdStatus"] != null &&
                Request.Form["IdLocation"] != null &&
                Request.Form["Description"] != null &
                Request.Form["Adresse"] != null &&
                Request.Form["CodePostal"] != null &&
                Request.Form["Salaire"] != null
            );
        }

        public ActionResult Index()
        {
            ViewBag.Stage = _bd.stage.ToList();
            ViewBag.Poste = _bd.poste.ToList();
            ViewBag.Contact = _bd.contact.ToList();
            ViewBag.Status = _bd.status.ToList();
            ViewBag.Location = _bd.location.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult TeleverserFichier()
        {
            var nomDeFichier = Request.Form["DocumentName"];

            if (nomDeFichier == null || nomDeFichier == "")
            {
                return RedirectToAction("Index");
            }

            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("content-disposition", "attachement;filename=" + nomDeFichier);
            Response.TransmitFile(Server.MapPath("~/DescriptionStage/" + nomDeFichier));
            Response.End();

            return View("index");
        }

        [HttpPost]
        public ActionResult AjouterStage()
        {
            var poste = new Poste {Nom = "test"};
            _bd.poste.Add(poste);

            _bd.SaveChanges();

            var status = new Status {StatusStage = "test"};
            _bd.status.Add(status);

            _bd.SaveChanges();

            var location = new Location {Nom = "test"};
            _bd.location.Add(location);

            _bd.SaveChanges();

            var contact = new Contact {Nom = "test", Courriel = "test", Telephone = "test"};
            _bd.contact.Add(contact);

            _bd.SaveChanges();

            var stage = new Stage
            {
                Location = location,
                CivicNumber = 100,
                NomRue = "test",
                Ville = "test",
                Province = "test",
                Pays = "CANADA",
                CodePostal = "test",
                Poste = poste,
                Status = status,
                Contact = contact,
                Description = "test",
                NomDocument = "sample.txt",
                Salaire = 15,
            };
            
            _bd.stage.Add(stage);
            _bd.SaveChanges();

            ViewBag.Internship = _bd.stage.ToList();
            ViewBag.Post = _bd.poste.ToList();
            ViewBag.Contact = _bd.contact.ToList();
            ViewBag.Status = _bd.status.ToList();
            ViewBag.Location = _bd.location.ToList();

            return View("Edition", stage.IdStage);
        }
    }
}
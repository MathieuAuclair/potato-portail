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

            Stage stage = (
                from entity in _bd.stage
                where entity.IdStage == 1
                select entity
            ).First();

            return View(stage);
        }

        [HttpPost]
        public ActionResult Edition(
            HttpPostedFileBase fichier,
            string location,
            string numeroCivique,
            string nomRue,
            string ville,
            string province,
            string pays,
            string codePostal,
            string poste,
            string status,
            string contact,
            string description,
            string nomDocument,
            string salaire
        )
        {
            string nomFichier = null;

            if (fichier != null && fichier.ContentLength > 0)
            {
                nomFichier = Path.GetFileName(fichier.FileName);
                var chemin = Path.Combine(Server.MapPath("~/DescriptionStage"), nomFichier ?? "sample.txt");
                fichier.SaveAs(chemin);
            }

            if (!EstCeQueLaRequeteContientLesParametresPourEdition())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Stage stageInstance = (
                from entity in _bd.stage
                where entity.IdStage == 1
                select entity
            ).First();

            int idPoste = Convert.ToInt32(Request.Form["idPoste"]);

            Poste posteInstance = (
                from entity
                    in _bd.poste
                where entity.IdPoste == idPoste
                select entity
            ).First();

            int idContact = Convert.ToInt32(Request.Form["idContact"]);

            Contact contactInstance = (
                from entity
                    in _bd.contact
                where entity.IdContact == idContact
                select entity
            ).First();

            int idStatus = Convert.ToInt32(Request.Form["idStatus"]);

            Status statusInstance = (
                from entity
                    in _bd.status
                where entity.IdStatus == idStatus
                select entity
            ).First();

            int idLocation = Convert.ToInt32(Request.Form["idLocation"]);

            Location locationInstance = (
                from entity
                    in _bd.location
                where entity.IdLocation == idLocation
                select entity
            ).First();

            stageInstance.Poste = posteInstance;
            stageInstance.Contact = contactInstance;
            stageInstance.Status = statusInstance;
            stageInstance.Location = locationInstance;
            stageInstance.Description = Request.Form["Description"];
            stageInstance.NomDocument = nomFichier;
            stageInstance.CodePostal = Request.Form["CodePostal"];
            stageInstance.Salaire = Convert.ToSingle(Request.Form["Salaire"]);

            _bd.SaveChanges();

            return RedirectToAction("Index");
        }

        private bool EstCeQueLaRequeteContientLesParametresPourEdition()
        {
            return (
                Request.Form["IdPoste"] != null &&
                Request.Form["IdContact"] != null &&
                Request.Form["IdStatus"] != null &&
                Request.Form["IdLocation"] != null &&
                Request.Form["Description"] != null &
                Request.Form["Pays"] != null &&
                Request.Form["Province"] != null &&
                Request.Form["Ville"] != null &&
                Request.Form["Rue"] != null &&
                Request.Form["NumeroCivique"] != null &&
                Request.Form["CodePostal"] != null &&
                Request.Form["Salaire"] != null
            );
        }

        public ActionResult Index()
        {
            return View(_bd.stage.ToList());
        }

        [HttpPost]
        public ActionResult TeleverserFichier()
        {
            var nomDeFichier = Request.Form["DocumentName"];

            if (string.IsNullOrEmpty(nomDeFichier))
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
                NumeroCivique = 100,
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

            return View("Edition", stage);
        }
    }
}
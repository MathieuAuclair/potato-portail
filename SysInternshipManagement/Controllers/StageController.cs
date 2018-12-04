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
        public ActionResult Edition(int? idStage)
        {
            if (idStage == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var stage = _bd.stage.Find(idStage);
            return View(stage);
        }

        [HttpPost]
        public ActionResult Edition(
            HttpPostedFileBase fichier,
            int? idLocation,
            int? idStatus,
            int? idPoste,
            int? idContact,
            int? numeroCivique,
            int? idStage,
            float? salaire,
            string nomRue,
            string ville,
            string province,
            string pays,
            string codePostal,
            string description,
            string nomDocument
        )
        {
            string nomFichier = null;

            if (fichier != null && fichier.ContentLength > 0)
            {
                nomFichier = Path.GetFileName(fichier.FileName) ?? string.Empty;
                fichier.SaveAs(Path.Combine(Server.MapPath("~/DescriptionStage"), nomFichier));
            }

            if (!EstCeQueLaRequeteContientLesParametresPourEdition())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var stageInstance = _bd.stage.Find(idStage);
            var posteInstance = _bd.poste.Find(idPoste);
            var contactInstance = _bd.contact.Find(idContact);
            var statusInstance = _bd.status.Find(idStatus);
            var locationInstance = _bd.location.Find(idLocation);

            stageInstance.Poste = posteInstance;
            stageInstance.Contact = contactInstance;
            stageInstance.Status = statusInstance;
            stageInstance.Location = locationInstance;
            stageInstance.Description = description;
            stageInstance.NomDocument = nomFichier;
            stageInstance.CodePostal = codePostal;
            stageInstance.Salaire = salaire ?? 0.0f;

            _bd.SaveChanges();

            return RedirectToAction("Index");
        }

        private bool EstCeQueLaRequeteContientLesParametresPourEdition()
        {
            return (
                Request.Form["idPoste"] != null &&
                Request.Form["idContact"] != null &&
                Request.Form["idStatus"] != null &&
                Request.Form["idLocation"] != null &&
                Request.Form["idStage"] != null &&
                Request.Form["description"] != null &
                Request.Form["pays"] != null &&
                Request.Form["province"] != null &&
                Request.Form["ville"] != null &&
                Request.Form["rue"] != null &&
                Request.Form["numeroCivique"] != null &&
                Request.Form["codePostal"] != null &&
                Request.Form["salaire"] != null
            );
        }

        public ActionResult Index()
        {
            return View(_bd.stage.ToList());
        }

        [HttpPost]
        public ActionResult TeleverserFichier()
        {
            var nomDeFichier = Request.Form["fichier"];

            if (string.IsNullOrEmpty(nomDeFichier))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
            var stage = new Stage
            {
                Location = _bd.location.First(),
                NumeroCivique = 0,
                NomRue = "nom de rue",
                Ville = "Saguenay",
                Province = "Québec",
                Pays = "Canada",
                CodePostal = "G7X7W2",
                Poste = _bd.poste.First(),
                Status = _bd.status.First(),
                Contact = _bd.contact.First(),
                Description = "Description du stage",
                NomDocument = "",
                Salaire = 0,
            };

            _bd.stage.Add(stage);
            _bd.SaveChanges();

            return View("~/Views/Stage/Edition.cshtml", stage);
        }

        public ActionResult Suppression(int? id)
        {
            var stage = _bd.stage.Find(id);

            if (stage == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var applicationsPourCeStage = from application in _bd.application
                                          where application.Stage.IdStage == id
                                          select application;

            if (!applicationsPourCeStage.Any())
            {
                _bd.stage.Remove(stage);
                _bd.SaveChanges();
            }

            return RedirectToAction("Index", "Stage");
        }
    }
}
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
            var poste = new Poste {Nom = "Nouveau stage"};
            _bd.poste.Add(poste);
            var status = new Status {StatusStage = "disponible"};
            _bd.status.Add(status);
            var location = new Location {Nom = "Saguenay"};
            _bd.location.Add(location);
            var contact = new Contact {Nom = "Nom contact", Courriel = "Courriel", Telephone = "numéro téléphone"};
            _bd.contact.Add(contact);
            
            var stage = new Stage
            {
                Location = location,
                NumeroCivique = 0,
                NomRue = "nom de rue",
                Ville = "Saguenay",
                Province = "Québec",
                Pays = "Canada",
                CodePostal = "G7X 7W2",
                Poste = poste,
                Status = status,
                Contact = contact,
                Description = "Description du stage",
                NomDocument = "",
                Salaire = 0,
            };

            _bd.stage.Add(stage);
            _bd.SaveChanges();

            return View("Edition", stage);
        }
    }
}
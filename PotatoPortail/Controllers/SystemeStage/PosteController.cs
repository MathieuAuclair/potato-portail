using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models;
using PotatoPortail.Toast;

namespace PotatoPortail.Controllers
{
    public class PosteController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/SystemeStage/Poste/Index.cshtml",_db.Poste.ToList());
        }        
        public ActionResult Modifier(int? IdPoste)
        {
            if (IdPoste == null)
            {
                this.AddToastMessage("Confirmation de modification", "La modification n'a pas bien été effectué", ToastType.Error, true);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var poste = _db.Poste.Find(IdPoste);

            if (poste == null)
            {
                this.AddToastMessage("Confirmation de modification", "La modification n'a pas bien été effectué", ToastType.Error, true);
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View("~/Views/SystemeStage/Poste/Modifier.cshtml",poste);
        }

        [HttpPost]
        public ActionResult EnregistrerLesModifications(
            int? idPoste,
            string nom
        )
        {
            if (idPoste == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var poste = _db.Poste.Find(idPoste);

            if (poste == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            poste.Nom = nom;
            _db.SaveChanges();
            this.AddToastMessage("Confirmation", "l'opération s'est effectué avec succes", ToastType.Success, true);
            return RedirectToAction("Index");
        }

        public ActionResult Creation()
        {
            var poste = new Poste {Nom = "Nouveau poste"};
            this.AddToastMessage("Confirmation de création", "La création a bien été effectué", ToastType.Success, true);
            _db.Poste.Add(poste);
            _db.SaveChanges();

            return View("~/Views/SystemeStage/Poste/Modifier.cshtml", poste);
        }
        public ActionResult Suppression(int? IdPoste)
        {
            var poste = _db.Poste.Find(IdPoste);

            if (poste == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            var stagesAyantCePoste = from stage in _db.Stage
                                     where stage.Poste.IdPoste == IdPoste
                                     select stage;

            if (!stagesAyantCePoste.Any())
            {
                this.AddToastMessage("Confirmation de supression", "La supression a bien été effectué", ToastType.Success, true);
                _db.Poste.Remove(poste);
                _db.SaveChanges();
            }
            else
            {
                this.AddToastMessage("Confirmation de supression", "La supression n'a bien pas été effectué", ToastType.Error, true);
            }
            
            return RedirectToAction("Index", "Poste");
        }
    }
}
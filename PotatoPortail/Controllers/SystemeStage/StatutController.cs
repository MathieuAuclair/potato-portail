using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models;
using PotatoPortail.Toast;

namespace PotatoPortail.Controllers
{
    public class StatutController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/SystemeStage/Statut/Index.cshtml", _db.StatutStage.ToList());
        }
        public ActionResult Modifier(int? IdStatutStage)
        {
            if (IdStatutStage == null)
            {
                this.AddToastMessage("Confirmation de modification", "La modification n'a pas bien été effectué", ToastType.Error, true);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var statut = _db.StatutStage.Find(IdStatutStage);

            if (statut == null)
            {
                this.AddToastMessage("Confirmation de modification", "La modification n'a pas bien été effectué", ToastType.Error, true);
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View("~/Views/SystemeStage/Statut/Modifier.cshtml",statut);
        }
        [HttpPost]
        public ActionResult EnregistrerLesModifications(
            int? IdStatutStage,
            string NomStatutStage
        )
        {
            if (IdStatutStage == null)
            {
                this.AddToastMessage("Confirmation", "l'opération ne s'est pas effectué avec succes", ToastType.Error, true);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var statut = _db.StatutStage.Find(IdStatutStage);

            if (statut == null)
            {
                this.AddToastMessage("Confirmation", "l'opération ne s'est pas effectué avec succes", ToastType.Error, true);
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            statut.NomStatutStage = NomStatutStage;
            _db.SaveChanges();
            this.AddToastMessage("Confirmation", "l'opération c'est effectué avec succes", ToastType.Success, true);
            return RedirectToAction("Index");
        }
        public ActionResult Creation()
        {
            var statut = new StatutStage { NomStatutStage = "Nouveau statuts"};
            this.AddToastMessage("Confirmation de création", "La création a bien été effectué", ToastType.Success, true);
            _db.StatutStage.Add(statut);
            _db.SaveChanges();

            return View("~/Views/SystemeStage/Statut/Modifier.cshtml", statut);
        }
        public ActionResult Suppression(int? IdStatutStage)
        {
            var statut = _db.StatutStage.Find(IdStatutStage);

            if (statut == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var stagesAyantCeStatut = from stage in _db.Stage
                                      where stage.StatutStage.IdStatutStage == IdStatutStage
                                      select stage;

            if (!stagesAyantCeStatut.Any())
            {
                this.AddToastMessage("Confirmation de supression", "La supression a bien été effectué", ToastType.Success, true);
                _db.StatutStage.Remove(statut);
                _db.SaveChanges();
            }
            else
            {
                this.AddToastMessage("Confirmation de supression", "La supression a bien été effectué", ToastType.Error, true);
            }

            return RedirectToAction("Index", "Statut");
        }
    }
}
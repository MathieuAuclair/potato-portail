using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers.SystemeStage
{
    public class StatusController : Controller
    {
        private readonly BdPortail _bd = new BdPortail();

        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/SystemeStage/Status/Index.cshtml", _bd.StatutStage.ToList());
        }

        [HttpPost]
        public ActionResult Edition(int? idStatus)
        {
            if (idStatus == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var status = _bd.StatutStage.Find(idStatus);

            if (status == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View("~/Views/SystemeStage/Status/Edition.cshtml", status);
        }

        [HttpPost]
        public ActionResult EnregistrerLesModifications(
            int? idStatus,
            string nom
        )
        {
            if (idStatus == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var status = _bd.StatutStage.Find(idStatus);

            if (status == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            status.NomStatutStage = nom;
            _bd.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Creation()
        {
            var status = new StatutStage {NomStatutStage = "Nouveau status"};

            _bd.StatutStage.Add(status);
            _bd.SaveChanges();

            return View("~/Views/SystemeStage/Status/Edition.cshtml", status);
        }

        public ActionResult Suppression(int? id)
        {
            var statut = _bd.StatutStage.Find(id);

            if (statut == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var stagesAyantCeStatut = from stage in _bd.Stage
                                      where stage.StatutStage.IdStatutStage == id
                                      select stage;

            if (stagesAyantCeStatut.Any()) return RedirectToAction("Index", "Status");
            _bd.StatutStage.Remove(statut);
            _bd.SaveChanges();

            return RedirectToAction("Index", "Status");
        }
    }
}
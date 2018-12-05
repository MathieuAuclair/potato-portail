
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers.SystemeStage
{
    public class StatusController : Controller
    {
        private readonly DatabaseContext _bd = new DatabaseContext();

        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/SystemeStage/Status/Index.cshtml", _bd.Status.ToList());
        }

        [HttpPost]
        public ActionResult Edition(int? idStatus)
        {
            if (idStatus == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var status = _bd.Status.Find(idStatus);

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

            var status = _bd.Status.Find(idStatus);

            if (status == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            status.StatusStage = nom;
            _bd.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Creation()
        {
            var status = new StatutStage {StatusStage = "Nouveau status"};

            _bd.Status.Add(status);
            _bd.SaveChanges();

            return View("~/Views/SystemeStage/Status/Edition.cshtml", status);
        }

        public ActionResult Suppression(int? id)
        {
            var statut = _bd.status.Find(id);

            if (statut == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var stagesAyantCeStatut = from stage in _bd.Stage
                                      where stage.Status.IdStatus == id
                                      select stage;

            if (!stagesAyantCeStatut.Any())
            {
                _bd.status.Remove(statut);
                _bd.SaveChanges();
            }

            return RedirectToAction("Index", "Status");
        }
    }
}
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Controllers
{
    public class StatusController : Controller
    {
        private readonly DatabaseContext _bd = new DatabaseContext();

        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/Status/Index.cshtml", _bd.status.ToList());
        }

        [HttpPost]
        public ActionResult Modifier(int? idStatus)
        {
            if (idStatus == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var status = _bd.status.Find(idStatus);

            if (status == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View("~/Views/Status/Modifier.cshtml", status);
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

            var status = _bd.status.Find(idStatus);

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
            var status = new Status {StatusStage = "Nouveau status"};

            _bd.status.Add(status);
            _bd.SaveChanges();

            return View("~/Views/Status/Modifier.cshtml", status);
        }

        public ActionResult Suppression(int? id)
        {
            var statut = _bd.status.Find(id);

            if (statut == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var stagesAyantCeStatut = from stage in _bd.stage
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
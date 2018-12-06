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
            return View(_bd.status.ToList());
        }

        [HttpPost]
        public ActionResult Modifier(int? IdStatuts)
        {
            if (IdStatuts == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var statuts = _bd.status.Find(IdStatuts);

            if (statuts == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(statuts);
        }

        [HttpPost]
        public ActionResult EnregistrerLesModifications(
            int? idStatuts,
            string nom
        )
        {
            if (idStatuts == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var statuts = _bd.status.Find(idStatuts);

            if (statuts == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            statuts.StatusStage = nom;
            _bd.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Creation()
        {
            var statuts = new Status {StatusStage = "Nouveau statuts"};

            _bd.status.Add(statuts);
            _bd.SaveChanges();

            return View("~/Views/Status/Modifier.cshtml", statuts);
        }

        public ActionResult Suppression(int? IdStatuts)
        {
            var statut = _bd.status.Find(IdStatuts);

            if (statut == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var stagesAyantCeStatut = from stage in _bd.stage
                                      where stage.Status.IdStatus == IdStatuts
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
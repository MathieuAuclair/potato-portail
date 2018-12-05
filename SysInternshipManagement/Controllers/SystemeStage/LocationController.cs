using System.Linq;
using System.Net;
using System.Web.Mvc;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Controllers.SystemeStage
{
    public class LocationController : Controller
    {
        private readonly DatabaseContext _bd = new DatabaseContext();

        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/SystemeStage/Location/Index.cshtml", _bd.Location.ToList());
        }

        [HttpPost]
        public ActionResult Edition(int? idLocation)
        {
            if (idLocation == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var location = _bd.Location.Find(idLocation);

            if (location == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View("~/Views/SystemeStage/Location/Edition.cshtml", location);
        }

        [HttpPost]
        public ActionResult EnregistrerLesModifications(
            int? idLocation,
            string nom
        )
        {
            if (idLocation == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var location = _bd.Location.Find(idLocation);

            if (location == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            location.Nom = nom;
            _bd.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Creation()
        {
            var location = new Location {Nom = "Nouvelle location"};

            _bd.Location.Add(location);
            _bd.SaveChanges();

            return View("~/Views/SystemeStage/Location/Edition.cshtml", location);
        }

        public ActionResult Suppression(int? id)
        {
            var location = _bd.location.Find(id);

            if (location == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var stagesAyantCeStatut = from stage in _bd.stage
                                      where stage.Status.IdStatus == id
                                      select stage;

            if (!stagesAyantCeStatut.Any())
            {
                _bd.location.Remove(location);
                _bd.SaveChanges();
            }

            return RedirectToAction("Index", "Location");
        }
    }
}
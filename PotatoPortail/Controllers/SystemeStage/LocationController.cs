using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers.SystemeStage
{
    public class LocationController : Controller
    {
        private readonly BDPortail _bd = new BDPortail();

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
            var location = _bd.Location.Find(id);

            if (location == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var stagesAyantCeStatut = from stage in _bd.Stage
                                      where stage.StatutStage.IdStatutStage == id
                                      select stage;

            if (!stagesAyantCeStatut.Any())
            {
                _bd.Location.Remove(location);
                _bd.SaveChanges();
            }

            return RedirectToAction("Index", "Location");
        }
    }
}
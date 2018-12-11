using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers
{
    public class LocationController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/SystemeStage/Location/Index.cshtml", _db.Location.ToList());
        }

        [HttpPost]
        public ActionResult Modifier(int? IdLocation)
        {
            if (IdLocation == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var location = _db.Location.Find(IdLocation);

            if (location == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View("~/Views/SystemeStage/Location/Modifier.cshtml",location);
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

            var location = _db.Location.Find(idLocation);

            if (location == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            location.Nom = nom;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Creation()
        {
            var location = new Location {Nom = "Nouvelle location"};

            _db.Location.Add(location);
            _db.SaveChanges();
            
            return View("~/Views/SystemeStage/Location/Modifier.cshtml", location);
        }

        public ActionResult Suppression(int? IdLocation)
        {
            var location = _db.Location.Find(IdLocation);

            if (location == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            _db.Location.Remove(location);
            _db.SaveChanges();
 
            return RedirectToAction("Index", "Location");
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models;
using PotatoPortail.Toast;

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
        public ActionResult Modifier(int? IdLocation)
        {
            if (IdLocation == null)
            {
                this.AddToastMessage("Confirmation de modification", "La modification n'a pas bien été effectué", ToastType.Error, true);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var location = _db.Location.Find(IdLocation);

            if (location == null)
            {
                this.AddToastMessage("Confirmation de modification", "La modification n'a pas bien été effectué", ToastType.Error, true);
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
                this.AddToastMessage("Confirmation", "l'opération ne s'est pas effectué avec succes", ToastType.Error, true);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var location = _db.Location.Find(idLocation);

            if (location == null)
            {
                this.AddToastMessage("Confirmation", "l'opération s'est effectué avec succes", ToastType.Error, true);
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            this.AddToastMessage("Confirmation", "l'opération s'est effectué avec succes", ToastType.Success, true);
            location.Nom = nom;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Creation()
        {
            var location = new Location {Nom = " "};
            this.AddToastMessage("Confirmation de création", "La création a bien été effectué", ToastType.Success, true);
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
            var stagesAyantCetteLocation = from stage in _db.Stage
                                      where stage.Location.IdLocation == IdLocation
                                           select stage;

            if (!stagesAyantCetteLocation.Any())
            {
                this.AddToastMessage("Confirmation de supression", "La supression a bien été effectué", ToastType.Success, true);
                _db.Location.Remove(location);
                _db.SaveChanges();
            }
            else
            {
                this.AddToastMessage("Confirmation de supression", "La supression a bien été effectué", ToastType.Error, true);
            }


            return RedirectToAction("Index", "Location");
        }
    }
}
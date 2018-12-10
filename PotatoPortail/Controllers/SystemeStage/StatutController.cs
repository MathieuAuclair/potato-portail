using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var statut = _db.StatutStage.Find(IdStatutStage);

            if (statut == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View("~/Views/SystemeStage/Statut/Modifier.cshtml",statut);
        }
        [HttpPost]
        public ActionResult EnregistrerLesModifications(
            int? IdStatutStage,
            string nom
        )
        {
            if (IdStatutStage == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var statut = _db.StatutStage.Find(IdStatutStage);

            if (statut == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            statut.NomStatutStage = nom;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Creation()
        {
            var statut = new StatutStage { NomStatutStage = "Nouveau statuts"};
    
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
                _db.StatutStage.Remove(statut);
                _db.SaveChanges();
            }

            return RedirectToAction("Index", "Statut");
        }
    }
}
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers.SystemeStage
{
    public class PosteController : Controller
    {
        private readonly DatabaseContext _bd = new DatabaseContext();

        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/SystemeStage/Poste/Index.cshtml", _bd.Poste.ToList());
        }

        [HttpPost]
        public ActionResult Edition(int? idPoste)
        {
            if (idPoste == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var poste = _bd.Poste.Find(idPoste);

            if (poste == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View("~/Views/SystemeStage/Poste/Edition.cshtml", poste);
        }

        [HttpPost]
        public ActionResult EnregistrerLesModifications(
            int? idPoste,
            string nom
        )
        {
            if (idPoste == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var poste = _bd.Poste.Find(idPoste);

            if (poste == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            poste.Nom = nom;
            _bd.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Creation()
        {
            var poste = new Poste {Nom = "Nouveau poste"};

            _bd.Poste.Add(poste);
            _bd.SaveChanges();

            return View("~/Views/SystemeStage/Poste/Edition.cshtml", poste);
        }
    }
}
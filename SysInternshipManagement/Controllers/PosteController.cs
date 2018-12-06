using System.Linq;
using System.Net;
using System.Web.Mvc;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Controllers
{
    public class PosteController : Controller
    {
        private readonly DatabaseContext _bd = new DatabaseContext();

        [HttpGet]
        public ActionResult Index()
        {
            return View(_bd.poste.ToList());
        }

        [HttpPost]
        public ActionResult Modifier(int? IdPoste)
        {
            if (IdPoste == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var poste = _bd.poste.Find(IdPoste);

            if (poste == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(poste);
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

            var poste = _bd.poste.Find(idPoste);

            if (poste == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            poste.Nom = nom;
            _bd.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Creation()
        {
            var poste = new Poste {Nom = "Nouveau poste"};

            _bd.poste.Add(poste);
            _bd.SaveChanges();

            return View("~/Views/Poste/Modifier.cshtml", poste);
        }
    }
}
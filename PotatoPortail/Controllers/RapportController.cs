using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using PotatoPortail.Migrations;
using PotatoPortail.Models;
using Rotativa.Core;
using Rotativa.MVC;

namespace PotatoPortail.Controllers
{
    public class RapportController : Controller
    {
        private readonly BdPortail _db = new BdPortail();
        
        public ActionResult Index()
        {
            dynamic model = new ExpandoObject();

            model.Programme = _db.Programme.ToList();
            model.PlanCadre = _db.PlanCadre.ToList();

            return View(model);
        }

        public ActionResult RapportPlanCadre(int id)
        {
            var planListe = new List<PlanCadre>();

            var planCadre = from a in _db.PlanCadre
                join b in _db.PlanCadrePrealable on a.IdPlanCadre equals b.IdPlanCadre
                where b.IdPrealable == id
                select a;

            foreach (var plan in planCadre)
            {
                planListe.Add(plan);
            }

            ViewData["listPcPrealableA"] = planListe;
            var footer = Server.MapPath("~/Views/static/footer.html");

            var reglage = $"--header-html  \"{null}\" " + "--header-spacing \"3\" " +
                             $"--footer-html \"{footer}\" " + "--footer-spacing \"10\" " + "--footer-font-size \"10\" ";

            return new ViewAsPdf("RapportPlanCadre", _db.PlanCadre.Find(id))
            {
                RotativaOptions = new DriverOptions
                {
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    PageSize = Rotativa.Core.Options.Size.A4,
                    CustomSwitches = reglage
                }
            };
        }

        public ActionResult RapportProgramme(int id)
        {
            var prog = _db.Programme.Find(id);
            var footer = Server.MapPath("~/Views/static/footer.html");
            var reglage = string.Format(
                "--footer-html \"{1}\" " +
                "--footer-spacing \"0\" " +
                "--footer-font-size \"10\" "
                , null, footer);

            return new ViewAsPdf("RapportPlanCadre", _db.Programme.Find(id))
            {
                RotativaOptions = new DriverOptions
                {
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    PageSize = Rotativa.Core.Options.Size.A4,
                    CustomSwitches = reglage
                }
            };
        }
    }
}
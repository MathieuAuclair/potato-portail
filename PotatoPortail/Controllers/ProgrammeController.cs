using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PotatoPortail.Helpers;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers
{
    [RcpProgrammeAuthorize]
    public class ProgrammeController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

        private IEnumerable<Programme> GetRcpProgramme()
        {
            var username = User.Identity.GetUserName();
            return from programme in _db.Programme
                   join devisMinistere in _db.DevisMinistere on programme.IdDevis equals devisMinistere.IdDevis
                   join departement in _db.Departement on devisMinistere.Discipline equals departement.Discipline
                   join accesProgramme in _db.AccesProgramme on departement.Discipline equals accesProgramme.Discipline
                   where accesProgramme.UserMail == username
                   select programme;
        }
  
        public ActionResult ListeProgramme(int? idPlan)
        {
            if (idPlan != null)
            {
                ViewBag.idPlanCadreActuel = idPlan;
            }
            return PartialView(GetRcpProgramme().ToList());
        }

        public ActionResult Index()
        {
            return View(GetRcpProgramme().ToList());
        }

        public ActionResult Info(int? idProgramme)
        {
            if (idProgramme == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var programme = _db.Programme.Find(idProgramme);
            if (programme == null)
            {
                return HttpNotFound();
            }
            return View(programme);
        }
    }
}

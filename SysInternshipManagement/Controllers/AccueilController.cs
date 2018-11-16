using System.Web.Mvc;

namespace SysInternshipManagement.Controllers
{
    public class AccueilController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.prenom = "test";
            return View("/Views/Accueil/Index.cshtml");
        }

        [Authorize(Roles = "RCP")]
        public ActionResult Pedagogie()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ConsoleAdmin()
        {
            return View();
        }

        [Authorize]
        public ActionResult ESport()
        {
            return View();
        }

        [Authorize]
        public ActionResult Reunions()
        {
            return View();
        }
    }
}
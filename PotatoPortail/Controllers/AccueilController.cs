using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace PotatoPortail.Controllers
{
    public class AccueilController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.prenom = HttpContext.GetOwinContext()
                .GetUserManager<ApplicationUserManager>()
                .FindById(User.Identity.GetUserId()).prenom;
            return View();
        }

       
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

        public ActionResult Reunions()
        {
            return View();
        }
        [Authorize]

        public ActionResult OrdreDuJour()
        {
            return View();
        }
        public ActionResult Stage()
        {
            return View();
        }
    }
}
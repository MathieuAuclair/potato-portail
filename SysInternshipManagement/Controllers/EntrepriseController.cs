using SysInternshipManagement.Migrations;
using System.Linq;
using System.Web.Mvc;

namespace SysInternshipManagement.Controllers
{
    public class EntrepriseController : Controller
    {
        DatabaseContext bd = new DatabaseContext();
        public ActionResult Index()
        {
            return View(bd.entreprise.ToList());
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}
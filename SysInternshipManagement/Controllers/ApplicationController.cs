using System.Web.Mvc;

namespace SysInternshipManagement.Controllers
{
    public class ApplicationController : Controller
    {
        // GET
        public ActionResult Index()
        {
            return
            View();
        }
    }
}
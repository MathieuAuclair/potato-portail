using System.Web.Mvc;

namespace SysInternshipManagement.Controllers.SystemeStage
{
    public class ApplicationController : Controller
    {
        public ActionResult Index()
        {
            return View("/Views/SystemeStage/Application/Index.cshtml");
        }
    }
}
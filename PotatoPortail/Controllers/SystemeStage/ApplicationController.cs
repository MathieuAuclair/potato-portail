using System.Web.Mvc;

namespace PotatoPortail.Controllers.SystemeStage
{
    public class ApplicationController : Controller
    {
        public ActionResult Index()
        {
            return View("/Views/SystemeStage/Application/Index.cshtml");
        }
    }
}
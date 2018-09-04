using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SysInternshipManagement.Controllers
{
    public class InternshipController : Controller
    {
        // GET: Intership
        public ActionResult Edit()
        {
            return View();
        } 

        public ActionResult Index()
        {
            return View();
        }
    }
}
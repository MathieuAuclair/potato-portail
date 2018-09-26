using SysInternshipManagement.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SysInternshipManagement.Controllers
{
    public class EntrepriseController : Controller
    {
        DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            return View(db.business.ToList());
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}
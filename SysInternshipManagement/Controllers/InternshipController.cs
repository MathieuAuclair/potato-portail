using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SysInternshipManagement.Migrations;
using System.Web;
using System.Web.Mvc;

namespace SysInternshipManagement.Controllers
{
    public class InternshipController : Controller
    {
        public static string fileName;
        private DatabaseContext db = new DatabaseContext();

        // GET: Intership
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var Name = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/DescriptionStage"), Name);
                fileName = Name;
                file.SaveAs(path);
            }

            return RedirectToAction("Index");
        }


        public ActionResult Index()
        {
            return View(db.internship.ToList());
        }

        public ActionResult UploadFile()
        {
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("content-disposition", "attachement;filename=" + fileName);
            Response.TransmitFile(Server.MapPath("~/DescriptionStage/" + fileName));
            Response.End();

            return View();
        }


    }
}
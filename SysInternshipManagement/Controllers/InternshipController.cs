using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SysInternshipManagement.Migrations;
using System.Web;
using System.Web.Mvc;
using SysInternshipManagement.Models;

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
            var post = new Post();
            post.Name = "test";
            db.post.Add(post);

            db.SaveChanges();

            var status = new Status();
            status.StatusInternship = "test";
            db.status.Add(status);

            db.SaveChanges();

            var location = new Location();
            location.Name = "test";
            db.location.Add(location);

            db.SaveChanges();

            var contact = new Contact();
            contact.Name = "test";
            contact.Email = "test";
            contact.Phone = "test";
            db.contact.Add(contact);

            db.SaveChanges();

            var internship = new Internship();
            internship.Location = location;
            internship.Post = post;
            internship.Status = status;
            internship.Contact = contact;
            internship.Address = "test";
            internship.Description = "test";
            internship.PostalCode = "test";
            internship.Salary = 15;
            db.internship.Add(internship);

            db.SaveChanges();

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

        public ActionResult addInternship()
        {
            return RedirectToAction("Index");
        }
    }
}
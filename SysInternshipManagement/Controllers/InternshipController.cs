using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SysInternshipManagement.Migrations;
using System.Web;
using System.Web.Mvc;
using SysInternshipManagement.Models;
using System.Data.Entity;
using System.Diagnostics;

namespace SysInternshipManagement.Controllers
{
    public class InternshipController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Edit()
        {
            if (Request.QueryString["IdInternship"] == null) {
                return RedirectToAction("Index");
            }

            ViewBag.IdInternship = Request.QueryString["IdInternship"];
            ViewBag.Post = db.post.ToList();
            ViewBag.Contact = db.contact.ToList();
            ViewBag.Status = db.status.ToList();
            ViewBag.Location = db.location.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase file)
        {
            string Name = null;

            if (file != null && file.ContentLength > 0)
            {
                Name = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/DescriptionStage"), Name);
                file.SaveAs(path);
            }

            if (!isRequestValidForInternshipUpdate())
            {
                Response.StatusCode = 400;
                Response.End();
            }

            Internship internship = (
                from entity in db.internship
                where entity.IdInternship == 1
                select entity
                ).First();

            Post post = (
                from entity
                in db.post
                where entity.IdPost == Convert.ToInt32(Request.Form["idPost"])
                select entity
                ).First();

            Contact contact = (
                from entity
                in db.contact
                where entity.IdContact == Convert.ToInt32(Request.Form["idContact"])
                select entity
                ).First();

            Status status = (
                from entity
                in db.status
                where entity.IdStatus == Convert.ToInt32(Request.Form["idStatus"])
                select entity
                ).First();

            Location location = (
                from entity
                in db.location
                where entity.IdLocation == Convert.ToInt32(Request.Form["idLocation"])
                select entity
                ).First();

            internship.Post = post;
            internship.Contact = contact;
            internship.Status = status;
            internship.Location = location;
            internship.Description = Request.Form["Description"];
            internship.Address = Request.Form["Address"];
            internship.DocumentName = Name;
            internship.PostalCode = Request.Form["PostalCode"];
            internship.Salary = Convert.ToSingle(Request.Form["Salary"]);

            db.SaveChanges();

            ViewBag.Internship = db.internship.ToList();
            ViewBag.Post = db.post.ToList();
            ViewBag.Contact = db.contact.ToList();
            ViewBag.Status = db.status.ToList();
            ViewBag.Location = db.location.ToList();

            return RedirectToAction("Index");
        }

        private bool isRequestValidForInternshipUpdate()
        {
            return (
                Request.Form["IdPost"] != null &&
                Request.Form["IdContact"] != null &&
                Request.Form["IdStatus"] != null &&
                Request.Form["IdLocation"] != null &&
                Request.Form["Description"] != null &
                Request.Form["Address"] != null &&
                Request.Form["PostalCode"] != null &&
                Request.Form["Salary"] != null
                );
        }

        public ActionResult Index()
        {
            ViewBag.Internship = db.internship.ToList();
            ViewBag.Post = db.post.ToList();
            ViewBag.Contact = db.contact.ToList();
            ViewBag.Status = db.status.ToList();
            ViewBag.Location = db.location.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            var fileName = Request.Form["DocumentName"];

            if (fileName == null || fileName == "") {
                return RedirectToAction("Index");
            }

            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("content-disposition", "attachement;filename=" + fileName);
            Response.TransmitFile(Server.MapPath("~/DescriptionStage/" + fileName));
            Response.End();

            return View();
        }

        [HttpPost]
        public ActionResult AddInternship()
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
            internship.DocumentName = "sample.txt";
            db.internship.Add(internship);

            db.SaveChanges();

            ViewBag.Internship = db.internship.ToList();
            ViewBag.Post = db.post.ToList();
            ViewBag.Contact = db.contact.ToList();
            ViewBag.Status = db.status.ToList();
            ViewBag.Location = db.location.ToList();

            return RedirectToAction("/Edit", new { IdInternship = internship.IdInternship });
        }
    }
}
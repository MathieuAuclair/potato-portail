using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly DatabaseContext _db = new DatabaseContext();

        public ActionResult Index()
        {
            return View(_db.application.ToList());
        }

        public ActionResult Creation()
        {
            List<SelectListItem> listeDeSelectionDeStage = new List<SelectListItem>();

            foreach (Stage stage in _db.stage)
            {
                listeDeSelectionDeStage.Add(new SelectListItem
                {
                    Text = stage.Description,
                    Value = stage.IdStage.ToString()
                });
            }

            ViewBag.Stages = listeDeSelectionDeStage;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation(int? idStage)
        {
            if (idStage == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Stage stage = _db.stage.Find(idStage);

            if (stage == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            Application application = new Application();

            application.Timestamp = DateTime.Now;
            application.Etudiant = _db.etudiant.First();
            application.Stage = stage;
            _db.application.Add(application);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edition(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Application application = _db.application.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }

            return View(application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edition([Bind(Include = "IdApplication,Timestamp")]
            Application application)
        {
            if (!ModelState.IsValid)
            {
                return View(application);
            }

            _db.Entry(application).State = EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("Index");

        }

        public ActionResult Suppression(int? id)
        {
            var application = _db.application.Find(id);

            if (application == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            _db.application.Remove(application);
            _db.SaveChanges();

            return RedirectToAction("Index", "Application");
        }

        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmerSupprimer(int id)
        {
            var application = _db.application.Find(id);

            _db.application.Remove(application ?? throw new InvalidOperationException());
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
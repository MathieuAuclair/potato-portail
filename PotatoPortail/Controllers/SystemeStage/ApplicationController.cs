using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models;
using PotatoPortail.Toast;

namespace PotatoPortail.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

        public ActionResult Index()
        {
            return View("~/Views/SystemeStage/Application/Index.cshtml",_db.Application.ToList());
        }

        public ActionResult Creation()
        {
            List<SelectListItem> listeDeSelectionDeStage = new List<SelectListItem>();

            foreach (Stage stage in _db.Stage)
            {
                listeDeSelectionDeStage.Add(new SelectListItem
                {
                    Text = stage.Poste.Nom + " dans la region de " + stage.Location,
                    Value = stage.IdStage.ToString()
                });
            }

            ViewBag.Stages = listeDeSelectionDeStage;

            return View("~/Views/SystemeStage/Application/Creation.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation(int? IdStage)
        {
            if (IdStage == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Stage stage = _db.Stage.Find(IdStage);

            if (stage == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            Application application = new Application();

            application.Timestamp = DateTime.Now;
            application.Etudiant = _db.Etudiant.First();
            application.Stage = stage;
            _db.Application.Add(application);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Modifier(int? IdApplication)
        {
            if (IdApplication == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Application application = _db.Application.Find(IdApplication);
            if (application == null)
            {
                return HttpNotFound();
            }

            return View("~/Views/SystemeStage/Application/Modifier.cshtml", application);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "IdApplication,Timestamp")] Application application)
        {
            if (!ModelState.IsValid)
            {
                this.AddToastMessage("Confirmation", "l'opération ne s'est pas effectué avec succes", ToastType.Error, true);
                return View(application);
            }

            _db.Entry(application).State = EntityState.Modified;
            _db.SaveChanges();
            this.AddToastMessage("Confirmation de modification", "l'opération c'est effectué avec succes", ToastType.Success, true);
            return RedirectToAction("Index");

        }

        public ActionResult Suppression(int? IdApplication)
        {
            var application = _db.Application.Find(IdApplication);

            if (application == null)
            {
                this.AddToastMessage("Confirmation", "l'opération ne s'est pas effectué avec succes", ToastType.Error, true);
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            this.AddToastMessage("Confirmation de supression", "La supression a bien été effectué", ToastType.Success, true);
            _db.Application.Remove(application);
            _db.SaveChanges();

            return RedirectToAction("Index", "Application");
        }

        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmerSupprimer(int IdStage)
        {
            var application = _db.Application.Find(IdStage);
            this.AddToastMessage("Confirmation de supression", "La supression a bien été effectué", ToastType.Success, true);
            _db.Application.Remove(application ?? throw new InvalidOperationException());
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
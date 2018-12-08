using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers
{
    public class GrilleCoursController : Controller
    {
        private readonly BdPortail _db = new BdPortail();
        
        public ActionResult Index()
        {
            return View(_db.GrilleCours.ToList());
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var grilleCours = _db.GrilleCours.Find(id);
            if (grilleCours == null)
            {
                return HttpNotFound();
            }
            return View(grilleCours);
        }

        public ActionResult Creation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "idGrille,nom")] GrilleCours grilleCours)
        {
            if (ModelState.IsValid)
            {
                _db.GrilleCours.Add(grilleCours);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grilleCours);
        }

        public ActionResult ListeGrilleCours()
        {
            return PartialView(_db.GrilleCours.ToList());
        }

        public ActionResult _Structure(int id)
        {
            return PartialView(_db.GrilleCours.Find(id));
        }

        public ActionResult Modifier(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var grilleCours = _db.GrilleCours.Find(id);
            if (grilleCours == null)
            {
                return HttpNotFound();
            }
            return View(grilleCours);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "idGrille,nom")] GrilleCours grilleCours)
        {
            if (!ModelState.IsValid) return View(grilleCours);
            _db.Entry(grilleCours).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Info(int id)
        {
            return View(_db.GrilleCours.Find(id));
        }

        public ActionResult InfoHeure(int id) {
            return View(_db.GrilleCours.Find(id));
        }

        public ActionResult _Heure(int id)
        {
            return PartialView(_db.GrilleCours.Find(id));
        }

        public ActionResult InfoPrealable(int id)
        {
            return View(_db.GrilleCours.Find(id));
        }

        public ActionResult _Prealable(int id)
        {
            return PartialView(_db.GrilleCours.Find(id));
        }

        public ActionResult Supression(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var grilleCours = _db.GrilleCours.Find(id);
            if (grilleCours == null)
            {
                return HttpNotFound();
            }
            return View(grilleCours);
        }

        [HttpPost, ActionName("Supression")]
        [ValidateAntiForgeryToken]
        public ActionResult SurpressionConfirmer(int id)
        {
            var grilleCours = _db.GrilleCours.Find(id);
            _db.GrilleCours.Remove(grilleCours ?? throw new InvalidOperationException());
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposer)
        {
            if (disposer)
            {
                _db.Dispose();
            }
            base.Dispose(disposer);
        }

    }
}


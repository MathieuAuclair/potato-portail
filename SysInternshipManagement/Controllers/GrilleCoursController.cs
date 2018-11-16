using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ApplicationPlanCadre.Models;
using SysInternshipManagement.Migrations;

namespace SysInternshipManagement.Controllers
{
    public class GrilleCoursController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            return View(db.GrilleCours.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GrilleCours grilleCours = db.GrilleCours.Find(id);
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
                db.GrilleCours.Add(grilleCours);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(grilleCours);
        }

        public ActionResult ListeGrilleCours()
        {
            return PartialView(db.GrilleCours.ToList());
        }

        public ActionResult _Structure(int id)
        {
            return PartialView(db.GrilleCours.Find(id));
        }

        public ActionResult Modifier(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GrilleCours grilleCours = db.GrilleCours.Find(id);
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
            if (ModelState.IsValid)
            {
                db.Entry(grilleCours).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(grilleCours);
        }

        public ActionResult Info(int id)
        {
            return View(db.GrilleCours.Find(id));
        }

        public ActionResult InfoHeure(int id)
        {
            return View(db.GrilleCours.Find(id));
        }

        public ActionResult _Heure(int id)
        {
            return PartialView(db.GrilleCours.Find(id));
        }

        public ActionResult InfoPrealable(int id)
        {
            return View(db.GrilleCours.Find(id));
        }

        public ActionResult _Prealable(int id)
        {
            return PartialView(db.GrilleCours.Find(id));
        }

        public ActionResult Supression(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GrilleCours grilleCours = db.GrilleCours.Find(id);
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
            GrilleCours grilleCours = db.GrilleCours.Find(id);

            if (grilleCours == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            db.GrilleCours.Remove(grilleCours);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposer)
        {
            if (disposer)
            {
                db.Dispose();
            }

            base.Dispose(disposer);
        }
    }
}
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models;
using PotatoPortail.Models.Plan_Cours;
using PotatoPortail.ViewModels.PlanCours;

namespace PotatoPortail.Controllers.PlanCours
{
    public class PlanCoursController : Controller
    {
        private readonly BdPortail _db = new BdPortail();
        private readonly MainPageViewModel _mainPageViewModel = new MainPageViewModel();
        
        public ActionResult Index()
        {
            _mainPageViewModel.PlanCours = _db.PlanCours.ToList();

            _mainPageViewModel.ContenuSection = _db.ContenuSection.ToList();
            _mainPageViewModel.NomSection = _db.NomSection.ToList();
            return View(_mainPageViewModel);
        }

        // GET: PlanCours/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContenuSection contenuSection = _db.ContenuSection.Find(id);
            if (contenuSection == null)
            {
                return HttpNotFound();
            }
            return View(contenuSection);
        }

        // GET: PlanCours/Create


        

        // POST: PlanCours/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Ajouter plan cours 
        public ActionResult Create([Bind(Include = "idPlanCours,dateCreation,statusPlanCours,idPlanCadre")] Models.Plan_Cours.PlanCours planCours)
        {
            if (ModelState.IsValid)
            {
                planCours.Cours = _db.Cours.Find(planCours.IdCours);
                _db.PlanCours.Add(planCours);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(planCours);
        }

        // GET: PlanCours/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Plan_Cours.PlanCours planCours = _db.PlanCours.Find(id);
            if (planCours == null)
            {
                return HttpNotFound();
            }
            return View(planCours);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlanCoursID,dateCreation,statusPlanCours")] Models.Plan_Cours.PlanCours planCours)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(planCours).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(planCours);
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Plan_Cours.PlanCours planCours = _db.PlanCours.Find(id);
            if (planCours == null)
            {
                return HttpNotFound();
            }
            return View(planCours);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var planCours = _db.PlanCours.Find(id);
            _db.PlanCours.Remove(planCours ?? throw new InvalidOperationException());
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

        public ActionResult GetMessage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Plan_Cours.PlanCours planCours = _db.PlanCours.Find(id);
            if (planCours == null)
            {
                return HttpNotFound();
            }
            var queryContenuSection = from contenu in _db.ContenuSection
                                      join texteSection in _db.TexteSection on contenu.IdContenuSection equals texteSection.IdContenuSection
                                      join cours in _db.PlanCours on texteSection.IdPlanCours equals cours.IdPlanCours
                                      where cours.IdPlanCours == id
                                      select contenu.TexteContenu;

            return new JsonResult { Data = queryContenuSection, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GetTitreSection(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Plan_Cours.PlanCours planCours = _db.PlanCours.Find(id);
            if (planCours == null)
            {
                return HttpNotFound();
            }
            var queryContenuSection = from contenu in _db.NomSection
                                      join contenuSection in _db.ContenuSection on contenu.IdNomSection equals contenuSection.IdContenuSection
                                      join texteSection in _db.TexteSection on contenuSection.IdContenuSection equals texteSection.IdContenuSection
                                      join cours in _db.PlanCours on texteSection.IdPlanCours equals cours.IdPlanCours
                                      where cours.IdPlanCours == id
                                      select contenu.TitreSection;

            return new JsonResult { Data = queryContenuSection, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}

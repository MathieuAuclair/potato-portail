using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApplicationPlanCadre.Models;
using ApplicationPlanCadre.ViewModels;
using ApplicationPlanCadre.App_Code;
/*using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using OpenXmlPowerTools;*/
using System.IO;
using System.Text;
using System.Drawing.Imaging;
using System.Xml.Linq;

namespace ApplicationPlanCadre.Controllers
{
    public class PlanCoursController : Controller
    {
        private BDPlanCadre db = new BDPlanCadre();
        private MainPageViewModel mainPageViewModel = new MainPageViewModel();

        // GET: PlanCours
        public ActionResult Index()
        {
            mainPageViewModel.PlanCours = db.PlanCours.ToList();

            mainPageViewModel.contenuSection = db.ContenuSection.ToList();
            mainPageViewModel.nomSection = db.NomSection.ToList();
            return View(mainPageViewModel);
        }

        // GET: PlanCours/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContenuSection contenuSection = db.ContenuSection.Find(id);
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
        public ActionResult Create([Bind(Include = "idPlanCours,dateCreation,statusPlanCours,idPlanCadre")] PlanCours planCours)
        {
            if (ModelState.IsValid)
            {
                planCours.Cours = db.Cours.Find(planCours.idCours);
                db.PlanCours.Add(planCours);
                db.SaveChanges();
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
            PlanCours planCours = db.PlanCours.Find(id);
            if (planCours == null)
            {
                return HttpNotFound();
            }
            return View(planCours);
        }

        // POST: PlanCours/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlanCoursID,dateCreation,statusPlanCours")] PlanCours planCours)
        {
            if (ModelState.IsValid)
            {
                db.Entry(planCours).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(planCours);
        }

        // GET: PlanCours/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanCours planCours = db.PlanCours.Find(id);
            if (planCours == null)
            {
                return HttpNotFound();
            }
            return View(planCours);
        }

        // POST: PlanCours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlanCours planCours = db.PlanCours.Find(id);
            db.PlanCours.Remove(planCours);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GetMessage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanCours planCours = db.PlanCours.Find(id);
            if (planCours == null)
            {
                return HttpNotFound();
            }
            var queryContenuSection = from contenu in db.ContenuSection
                                      join CPC in db.TexteSection on contenu.idContenuSection equals CPC.idContenuSection
                                      join PC in db.PlanCours on CPC.idPlanCours equals PC.idPlanCours
                                      where PC.idPlanCours == id
                                      select contenu.texteContenu;

            return new JsonResult { Data = queryContenuSection, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GetTitreSection(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanCours planCours = db.PlanCours.Find(id);
            if (planCours == null)
            {
                return HttpNotFound();
            }
            var queryContenuSection = from contenu in db.NomSection
                                      join CS in db.ContenuSection on contenu.idNomSection equals CS.idContenuSection
                                      join TS in db.TexteSection on CS.idContenuSection equals TS.idContenuSection
                                      join PC in db.PlanCours on TS.idPlanCours equals PC.idPlanCours
                                      where PC.idPlanCours == id
                                      select contenu.titreSection;

            return new JsonResult { Data = queryContenuSection, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}

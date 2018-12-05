using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApplicationPlanCadre;
using ApplicationPlanCadre.Models;

namespace ApplicationPlanCadre.Controllers.eSports
{
    public class EntraineurController : Controller
    {
        private BDPlanCadre db = new BDPlanCadre();

        // GET: Entraineur
        public ActionResult Index()
        {
            return View(db.Entraineurs.OrderBy(e => e.nomEntraineur).ToList());
        }

        // GET: Entraineur/Create
        public ActionResult Creation()
        {
            return View();
        }

        // POST: Entraineur/Creation
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "id,nomEntraineur,prenomEntraineur,pseudoEntraineur,numTel,adresseCourriel")] Entraineur entraineur)
        {
            if (ModelState.IsValid)
            {
                db.Entraineurs.Add(entraineur);
                db.SaveChanges();
                this.AddToastMessage("Ajout d'entraîneur effectué.", entraineur.nomComplet + " a été ajouté(e) à la liste d'entraîneurs sous le pseudo « " + entraineur.pseudoEntraineur + " ».", Toast.ToastType.Success);
                return RedirectToAction("Index");
            }

            return View(entraineur);
        }

        //GET: Entraineur/Modifier/5
        public ActionResult Modifier(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entraineur entraineur = db.Entraineurs.Find(id);
            if (entraineur == null)
            {
                return HttpNotFound();
            }
            return View(entraineur);
        }

        // POST: Entraineur/Modifier/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "id,nomEntraineur,prenomEntraineur,pseudoEntraineur,numTel,adresseCourriel")] Entraineur entraineur)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entraineur).State = EntityState.Modified;
                db.SaveChanges();
                this.AddToastMessage("Modifications apportées.", "Les changements apportés à l'entraîneur " + entraineur.nomComplet + " ont été enregistrés.", Toast.ToastType.Success);
                return RedirectToAction("Index");
            }
            return View(entraineur);
        }

        // GET: Entraineur/Supprimer/5
        public ActionResult Supprimer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entraineur entraineur = db.Entraineurs.Find(id);
            if (entraineur == null)
            {
                return HttpNotFound();
            }
            return View(entraineur);
        }

        // POST: Entraineur/Delete/5
        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmationSupprimer(int id)
        {
            Entraineur entraineur = db.Entraineurs.Find(id);
            db.Entraineurs.Remove(entraineur);
            db.SaveChanges();
            this.AddToastMessage("Suppression effectuée.", entraineur.nomComplet + " a été supprimé(e) de la liste.", Toast.ToastType.Success);
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
    }
}
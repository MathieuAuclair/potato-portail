using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models.eSports;
using PotatoPortail.Toast;

namespace PotatoPortail.Controllers.eSports
{
    public class EntraineurController : Controller
    {
        private readonly BdPortail _db = new BdPortail();
        
        public ActionResult Index()
        {
            return View(_db.Entraineur.OrderBy(entraineur => entraineur.NomEntraineur).ToList());
        }
        
        public ActionResult Creation()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "id,nomEntraineur,prenomEntraineur,pseudoEntraineur,numTel,adresseCourriel")] Entraineur entraineur)
        {
            if (!ModelState.IsValid) return View(entraineur);
            _db.Entraineur.Add(entraineur);
            _db.SaveChanges();
            this.AddToastMessage("Ajout d'entraîneur effectué.", entraineur.NomComplet + " a été ajouté(e) à la liste d'entraîneurs sous le pseudo « " + entraineur.PseudoEntraineur + " ».", ToastType.Success);
            return RedirectToAction("Index");

        }
        
        public ActionResult Modifier(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var entraineur = _db.Entraineur.Find(id);
            if (entraineur == null)
            {
                return HttpNotFound();
            }
            return View(entraineur);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "id,nomEntraineur,prenomEntraineur,pseudoEntraineur,numTel,adresseCourriel")] Entraineur entraineur)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(entraineur).State = EntityState.Modified;
                _db.SaveChanges();
                this.AddToastMessage("Modifications apportées.", "Les changements apportés à l'entraîneur " + entraineur.NomComplet + " ont été enregistrés.", ToastType.Success);
                return RedirectToAction("Index");
            }
            return View(entraineur);
        }
        
        public ActionResult Supprimer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entraineur = _db.Entraineur.Find(id);
            if (entraineur == null)
            {
                return HttpNotFound();
            }
            return View(entraineur);
        }
        
        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmationSupprimer(int id)
        {
            Entraineur entraineur = _db.Entraineur.Find(id);
            _db.Entraineur.Remove(entraineur ?? throw new InvalidOperationException());
            _db.SaveChanges();
            this.AddToastMessage("Suppression effectuée.", entraineur.NomComplet + " a été supprimé(e) de la liste.", ToastType.Success);
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
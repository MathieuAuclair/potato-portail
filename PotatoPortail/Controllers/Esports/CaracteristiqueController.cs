using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApplicationPlanCadre.Models;
using ApplicationPlanCadre.Models.eSports;

namespace ApplicationPlanCadre.Controllers
{
    public class CaracteristiqueController : Controller
    {
        private BDPlanCadre db = new BDPlanCadre();

        // GET: Caracteristique
        public ActionResult Index()
        {
            return View(db.Caracteristiques.ToList());
        }

        // GET: Caracteristique/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caracteristique caracteristique = db.Caracteristiques.Find(id);
            if (caracteristique == null)
            {
                return HttpNotFound();
            }
            return View(caracteristique);
        }

        // GET: Caracteristique/Create
        public ActionResult Creation(int? JeuId, string nomJeu)
        {
            Jeu jeu = db.Jeux.Find(JeuId);
            if (jeu == null)
            {
                return HttpNotFound();
            }

            ViewBag.nomJeu = nomJeu;

            return View();
        }

        // POST: Caracteristique/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "id,nomCaracteristique,JeuId")] Caracteristique caracteristique)
        {
            if (ModelState.IsValid)
            {
                Jeu jeu = db.Jeux.Find(caracteristique.JeuId);

                db.Caracteristiques.Add(caracteristique);
                db.SaveChanges();
                return RedirectToAction("Modifier", "Jeu", new { jeu.id, jeu.nomJeu});
            }

            return View(caracteristique);
        }

        // GET: Caracteristique/Edit/5
        public ActionResult Modifier(int? id, string nomCarac, string nomJeu)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caracteristique caracteristique = db.Caracteristiques.Find(id);
            if (caracteristique == null)
            {
                return HttpNotFound();
            }

            ViewBag.nomCarac = nomCarac;
            ViewBag.nomJeu = nomJeu;

            return View(caracteristique);
        }

        // POST: Caracteristique/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "id,nomCaracteristique,JeuId")] Caracteristique caracteristique)
        {
            if (ModelState.IsValid)
            {
                Jeu jeu = db.Jeux.Find(caracteristique.JeuId);

                db.Entry(caracteristique).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Modifier", "Jeu", new { jeu.id, jeu.nomJeu});
            }
            return View(caracteristique);
        }

        // GET: Caracteristique/Delete/5
        public ActionResult Supprimer(int? id, string nomJeu)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caracteristique caracteristique = db.Caracteristiques.Find(id);
            if (caracteristique == null)
            {
                return HttpNotFound();
            }

            var itemsCarac = from i in db.Items
                             where i.CaracteristiqueId == id
                             select i;

            ViewBag.nomJeu = nomJeu;
            ViewBag.itemsCarac = itemsCarac.ToList();

            return View(caracteristique);
        }

        // POST: Caracteristique/Delete/5
        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmationSupprimer(int id)
        {
            Caracteristique caracteristique = db.Caracteristiques.Find(id);
            Jeu jeu = db.Jeux.Find(caracteristique.JeuId);

            db.Caracteristiques.Remove(caracteristique);
            db.SaveChanges();
            return RedirectToAction("Modifier", "Jeu", new { jeu.id, jeu.nomJeu});
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
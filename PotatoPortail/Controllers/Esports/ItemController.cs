using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Migrations;
using ApplicationPlanCadre.Models;
using ApplicationPlanCadre.Models.eSports;

namespace ApplicationPlanCadre.Controllers
{
    public class ItemController : Controller
    {
        private BDPlanCadre db = new BDPlanCadre();

        // GET: Item
        public ActionResult Index(int? id, string nomCarac, string nomJeu)
        {
            var itemsCarac = from i in db.Items
                             where i.CaracteristiqueId == id
                             select i;

            Caracteristique carac = db.Caracteristiques.Find(id);
            Jeu jeu = db.Jeux.Find(carac.JeuId);

            ViewBag.CaracteristiqueId = id;
            ViewBag.nomCaracteristique = nomCarac;
            ViewBag.JeuId = jeu.id;
            ViewBag.nomJeu = nomJeu;
            ViewBag.itemsCarac = itemsCarac.ToList();

            return View(itemsCarac.OrderBy(i => i.nomItem).ToList());
        }

        // GET: Item/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Item/Create
        public ActionResult Creation(int? CaracteristiqueId, string nomCarac, string nomJeu)
        {
            Caracteristique carac = db.Caracteristiques.Find(CaracteristiqueId);

            ViewBag.CaracteristiqueId = CaracteristiqueId;
            ViewBag.nomCarac = nomCarac;
            ViewBag.nomJeu = nomJeu;

            return View();
        }

        // POST: Item/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "id,nomItem,CaracteristiqueId")] Item item)
        {
            if (ModelState.IsValid)
            {
                Caracteristique carac = db.Caracteristiques.Find(item.CaracteristiqueId);
                Jeu jeu = db.Jeux.Find(carac.JeuId);

                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index", "Item", new { carac.id, nomCarac = carac.nomCaracteristique, jeu.nomJeu});
            }

            ViewBag.carac = new SelectList(db.Caracteristiques, "id", "nomCaracteristique", item.CaracteristiqueId);
            return View(item);
        }

        // GET: Item/Edit/5
        public ActionResult Modifier(int? id, string nomCarac, string nomJeu)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            ViewBag.nomCaracteristique = nomCarac;
            ViewBag.nomJeu = nomJeu;
            ViewBag.CaracteristiqueId = new SelectList(db.Caracteristiques, "id", "nomCaracteristique", item.CaracteristiqueId);

            return View(item);
        }

        // POST: Item/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "id,nomItem,CaracteristiqueId")] Item item)
        {
            if (ModelState.IsValid)
            {
                Caracteristique carac = db.Caracteristiques.Find(item.CaracteristiqueId);
                Jeu jeu = db.Jeux.Find(carac.JeuId);
                db.Set<Item>().AddOrUpdate(item);
                db.SaveChanges();
                return RedirectToAction("Modifier", "Jeu", new { jeu.id, jeu.nomJeu});
            }
            ViewBag.CaracteristiqueId = new SelectList(db.Caracteristiques, "id", "nomCaracteristique", item.CaracteristiqueId);
            return View(item);
        }

        // GET: Item/Delete/5
        public ActionResult Supprimer(int? id, string nomJeu)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            ViewBag.nomJeu = nomJeu;

            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmationSupprimer(int id)
        {
            Item item = db.Items.Find(id);
            Caracteristique carac = db.Caracteristiques.Find(item.CaracteristiqueId);
            Jeu jeu = db.Jeux.Find(carac.JeuId);
            
            db.Items.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Modifier", "Jeu", new { jeu.id, jeu.nomJeu });
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
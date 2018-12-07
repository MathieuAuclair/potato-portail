using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers.eSports
{
    public class ItemController : Controller
    {
        private readonly BdPortail _db = new BdPortail();
        
        public ActionResult Index(int? id, string nomCarac, string nomJeu)
        {
            var itemsCarac = from i in _db.Items
                where i.IdCaracteristique == id
                select i;

            var caracteristique = _db.Caracteristiques.Find(id);

            if (caracteristique == null)
            {
                return HttpNotFound();
            }

            var jeu = _db.Jeux.Find(caracteristique.IdJeu);

            if (jeu == null)
            {
                return HttpNotFound();
            }

            ViewBag.CaracteristiqueId = id;
            ViewBag.nomCaracteristique = nomCarac;
            ViewBag.JeuId = jeu.Id;
            ViewBag.nomJeu = nomJeu;
            ViewBag.itemsCarac = itemsCarac.ToList();

            return View(itemsCarac.OrderBy(i => i.NomItem).ToList());
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Item item = _db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }
        
        public ActionResult Creation(int? caracteristiqueId, string nomCarac, string nomJeu)
        {
            ViewBag.CaracteristiqueId = caracteristiqueId;
            ViewBag.nomCarac = nomCarac;
            ViewBag.nomJeu = nomJeu;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "id,nomItem,CaracteristiqueId")]
            Item item)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.carac = new SelectList(_db.Caracteristiques, "id", "nomCaracteristique", item.IdCaracteristique);
                return View(item);
            }

            var caracteristique = _db.Caracteristiques.Find(item.IdCaracteristique);

            if (caracteristique == null)
            {
                return HttpNotFound();
            }

            var jeu = _db.Jeux.Find(caracteristique.IdJeu);

            if (jeu == null)
            {
                return HttpNotFound();
            }

            _db.Items.Add(item);
            _db.SaveChanges();

            return RedirectToAction("Index", "Item",
                new {caracteristique.Id, nomCarac = caracteristique.NomCaracteristique, jeu.NomJeu});
        }

        public ActionResult Modifier(int? id, string nomCarac, string nomJeu)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var item = _db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            ViewBag.nomCaracteristique = nomCarac;
            ViewBag.nomJeu = nomJeu;
            ViewBag.CaracteristiqueId =
                new SelectList(_db.Caracteristiques, "id", "nomCaracteristique", item.IdCaracteristique);

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "id,nomItem,CaracteristiqueId")]
            Item item)
        {
            if (ModelState.IsValid)
            {
                var caracteristique = _db.Caracteristiques.Find(item.IdCaracteristique);

                if (caracteristique == null)
                {
                    return HttpNotFound();
                }

                var jeu = _db.Jeux.Find(caracteristique.IdJeu);

                if (jeu == null)
                {
                    return HttpNotFound();
                }

                _db.Set<Item>().AddOrUpdate(item);
                _db.SaveChanges();

                return RedirectToAction("Modifier", "Jeu", new {jeu.Id, jeu.NomJeu});
            }

            ViewBag.CaracteristiqueId =
                new SelectList(_db.Caracteristiques, "id", "nomCaracteristique", item.IdCaracteristique);
            return View(item);
        }

        public ActionResult Supprimer(int? id, string nomJeu)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Item item = _db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            ViewBag.nomJeu = nomJeu;

            return View(item);
        }

        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmationSupprimer(int id)
        {
            var item = _db.Items.Find(id);

            if (item == null)
            {
                return HttpNotFound();
            }

            var caracteristique = _db.Caracteristiques.Find(item.IdCaracteristique);

            if (caracteristique == null)
            {
                return HttpNotFound();
            }

            var jeu = _db.Jeux.Find(caracteristique.IdJeu);

            if (jeu == null)
            {
                return HttpNotFound();
            }

            _db.Items.Remove(item);
            _db.SaveChanges();
            return RedirectToAction("Modifier", "Jeu", new {jeu.Id, jeu.NomJeu});
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
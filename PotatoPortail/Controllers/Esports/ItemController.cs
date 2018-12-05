using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers.Esports
{
    public class ItemController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

        public ActionResult Index(int? id, string nomCaracterisque, string nomJeu)
        {
            var itemsCaracteristique = from entity in _db.Items
                where entity.IdCaracteristique == id
                select entity;

            Caracteristique caracteristique = _db.Caracteristiques.Find(id);

            if (caracteristique == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            Jeu jeu = _db.Jeux.Find(caracteristique.JeuId);

            if (jeu == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            ViewBag.CaracteristiqueId = id;
            ViewBag.nomCaracteristique = nomCaracterisque;
            ViewBag.JeuId = jeu.id;
            ViewBag.nomJeu = nomJeu;
            ViewBag.itemsCarac = itemsCaracteristique.ToList();

            return View(itemsCaracteristique.OrderBy(i => i.nomItem).ToList());
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

            return View("/Views/Item/Modifier.cshtml", item);
        }

        public ActionResult Creation(int? caracteristiqueId, string nomCarac, string nomJeu)
        {
            _db.Caracteristiques.Find(caracteristiqueId);

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
            if (ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Caracteristique caracteristique = _db.Caracteristiques.Find(item.CaracteristiqueId);

            if (caracteristique == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            Jeu jeu = _db.Jeux.Find(caracteristique.JeuId);

            _db.Items.Add(item);
            _db.SaveChanges();

            if (jeu == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return RedirectToAction("Index", "Item",
                new {caracteristique.id, nomCarac = caracteristique.nomCaracteristique, jeu.nomJeu});
        }

        public ActionResult Modifier(int? id, string nomCarac, string nomJeu)
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

            ViewBag.nomCaracteristique = nomCarac;
            ViewBag.nomJeu = nomJeu;
            ViewBag.CaracteristiqueId =
                new SelectList(_db.Caracteristiques, "id", "nomCaracteristique", item.CaracteristiqueId);

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "id,nomItem,CaracteristiqueId")]
            Item item)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Caracteristique caracteristique = _db.Caracteristiques.Find(item.CaracteristiqueId);

            if (caracteristique == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            Jeu jeu = _db.Jeux.Find(caracteristique.JeuId);

            if (jeu == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            _db.Set<Item>().AddOrUpdate(item);
            _db.SaveChanges();

            return RedirectToAction("Index", "Item",
                new
                {
                    id = item.CaracteristiqueId,
                    nomCaracteristique = caracteristique.nomCaracteristique,
                    nomJeu = jeu.nomJeu
                });
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
            Item item = _db.Items.Find(id);

            if (item == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            Caracteristique caracteristique = _db.Caracteristiques.Find(item.CaracteristiqueId);

            if (caracteristique == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            Jeu jeu = _db.Jeux.Find(caracteristique.JeuId);

            if (jeu == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            _db.Items.Remove(item);
            _db.SaveChanges();
            return RedirectToAction("Index", "Item", new {caracteristique.id, nomCarac = caracteristique.nomCaracteristique, jeu.nomJeu});
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
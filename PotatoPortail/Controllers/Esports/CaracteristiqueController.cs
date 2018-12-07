using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers.Esports
{
    public class CaracteristiqueController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

        // GET: Caracteristique
        public ActionResult Index()
        {
            return View(_db.Caracteristiques.ToList());
        }

        // GET: Caracteristique/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caracteristique caracteristique = _db.Caracteristiques.Find(id);
            if (caracteristique == null)
            {
                return HttpNotFound();
            }
            return View(caracteristique);
        }

        // GET: Caracteristique/Create
        public ActionResult Creation(int? JeuId, string nomJeu)
        {
            Jeu jeu = _db.Jeux.Find(JeuId);
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
                Jeu jeu = _db.Jeux.Find(caracteristique.IdJeu);

                _db.Caracteristiques.Add(caracteristique);
                _db.SaveChanges();
                return RedirectToAction("Modifier", "Jeu", new { jeu.Id, jeu.NomJeu});
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

            Caracteristique caracteristique = _db.Caracteristiques.Find(id);
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Jeu jeu = _db.Jeux.Find(caracteristique.IdJeu);

            _db.Entry(caracteristique).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Modifier", "Jeu", new { jeu.Id, jeu.NomJeu});
            
        }

        // GET: Caracteristique/Delete/5
        public ActionResult Supprimer(int? id, string nomJeu)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Caracteristique caracteristique = _db.Caracteristiques.Find(id);
            if (caracteristique == null)
            {
                return HttpNotFound();
            }

            var itemsCarac = from i in _db.Items
                where i.IdCaracteristique == id
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
            Caracteristique caracteristique = _db.Caracteristiques.Find(id);

            if (caracteristique == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            Jeu jeu = _db.Jeux.Find(caracteristique.IdJeu);

            if (jeu == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            _db.Caracteristiques.Remove(caracteristique);
            _db.SaveChanges();

            _db.Caracteristiques.Remove(caracteristique);
            _db.SaveChanges();
            return RedirectToAction("Modifier", "Jeu", new { jeu.Id, jeu.NomJeu});
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
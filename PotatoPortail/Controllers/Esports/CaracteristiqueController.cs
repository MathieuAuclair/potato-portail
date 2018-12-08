using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models;
using PotatoPortail.Models.eSports;

namespace PotatoPortail.Controllers.Esports
{
    public class CaracteristiqueController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

        public ActionResult Index()
        {
            return View(_db.Caracteristique.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Caracteristique caracteristique = _db.Caracteristique.Find(id);
            if (caracteristique == null)
            {
                return HttpNotFound();
            }

            return View(caracteristique);
        }

        public ActionResult Creation(int? jeuId, string nomJeu)
        {
            Jeu jeu = _db.Jeu.Find(jeuId);
            if (jeu == null)
            {
                return HttpNotFound();
            }

            ViewBag.nomJeu = nomJeu;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "id,nomCaracteristique,JeuId")]
            Caracteristique caracteristique)
        {
            if (!ModelState.IsValid) return View(caracteristique);
            var jeu = _db.Jeu.Find(caracteristique.IdJeu);


            if (jeu == null)
            {
                return HttpNotFound();
            }

            _db.Caracteristique.Add(caracteristique);
            _db.SaveChanges();

            return RedirectToAction("Modifier", "Jeu", new {jeu.Id, jeu.NomJeu});
        }

        public ActionResult Modifier(int? id, string nomCarac, string nomJeu)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Caracteristique caracteristique = _db.Caracteristique.Find(id);
            if (caracteristique == null)
            {
                return HttpNotFound();
            }

            ViewBag.nomCarac = nomCarac;
            ViewBag.nomJeu = nomJeu;

            return View(caracteristique);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "id,nomCaracteristique,JeuId")]
            Caracteristique caracteristique)
        {
            if (ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var jeu = _db.Jeu.Find(caracteristique.IdJeu);

            if (jeu == null)
            {
                return HttpNotFound();
            }

            _db.Entry(caracteristique).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Modifier", "Jeu", new {jeu.Id, jeu.NomJeu});
        }

        public ActionResult Supprimer(int? id, string nomJeu)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Caracteristique caracteristique = _db.Caracteristique.Find(id);
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
            Caracteristique caracteristique = _db.Caracteristique.Find(id);

            if (caracteristique == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            Jeu jeu = _db.Jeu.Find(caracteristique.IdJeu);

            if (jeu == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            _db.Caracteristique.Remove(caracteristique);
            _db.SaveChanges();

            _db.Caracteristique.Remove(caracteristique);
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
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ApplicationPlanCadre.Controllers;
using PotatoPortail.Migrations;
using PotatoPortail.Models;
using PotatoPortail.Toast;

namespace PotatoPortail.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConsoleDevisMinistereController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

        public ActionResult _PartialList()
        {
            var devisMinistere = _db.DevisMinistere.Include(p => p.Departement);
            return PartialView(devisMinistere.ToList());
        }

        public ActionResult Index()
        {
            var devisMinistere = _db.DevisMinistere.Include(p => p.Departement);

            return View(devisMinistere.ToList());
        }

        public ActionResult Creation()
        {
            ViewBag.discipline = ConstruireCodeDevisMinistereSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "discipline, annee, codeSpecialisation")]
            DevisMinistere devisMinistere)
        {
            if (!DevisExiste(devisMinistere) && ModelState.IsValid)
            {
                devisMinistere.CodeSpecialisation = devisMinistere.CodeSpecialisation.ToUpper().Trim();
                _db.DevisMinistere.Add(devisMinistere);
                _db.SaveChanges();
                this.AddToastMessage("Création confirmée",
                    "Le devis " + '\u0022' + devisMinistere.Nom + '\u0022' + " a bien été créé.",
                    Toast.ToastType.Success);
                return RedirectToAction("Index");

            }

            if (DevisExiste(devisMinistere))
            {
                this.AddToastMessage("Problème pour confirmée", "Erreur, ce devis existe déjà.", Toast.ToastType.Error,
                    true);
            }

            ViewBag.discipline = ConstruireCodeDevisMinistereSelectList();
            return View(devisMinistere);
        }

        public ActionResult Modifier(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DevisMinistere devisMinistere = _db.DevisMinistere.Find(id);
            if (devisMinistere == null)
            {
                return HttpNotFound();
            }

            ViewBag.discipline = ConstruireCodeDevisMinistereSelectList(devisMinistere.Discipline);
            return View(devisMinistere);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "idDevis, discipline, annee, codeSpecialisation")]
            DevisMinistere devisMinistere)
        {
            if (!DevisExiste(devisMinistere) && ModelState.IsValid)
            {
                devisMinistere.CodeSpecialisation = devisMinistere.CodeSpecialisation.ToUpper();
                _db.Entry(devisMinistere).State = EntityState.Modified;
                _db.SaveChanges();
                this.AddToastMessage("Confirmation de la modification",
                    "Le devis " + '\u0022' + devisMinistere.Nom + '\u0022' + " a bien été modifié.",
                    ToastType.Success);
                return RedirectToAction("Index");
            }

            if (DevisExiste(devisMinistere))
            {
                this.AddToastMessage("Problème lors de la modification", "Erreur, ce devis ministeriel existe déjà.",
                    ToastType.Error, true);
            }

            ViewBag.discipline = ConstruireCodeDevisMinistereSelectList();
            return View(devisMinistere);
        }

        public SelectList ConstruireCodeDevisMinistereSelectList(string discipline = null)
        {
            var liste = _db.Departement
                .Select(e => new {discipline = e.Discipline, texte = e.Discipline + " - " + e.Nom}).ToList();
            if (discipline != null)
                return new SelectList(liste, "discipline", "texte", discipline);
            return new SelectList(liste, "discipline", "texte");
        }

        [ActionName("Supression")]
        public ActionResult SurpressionConfirmer(int id)
        {
            var devisMinistere = _db.DevisMinistere.Find(id);
            _db.DevisMinistere.Remove(devisMinistere ?? throw new InvalidOperationException());
            _db.SaveChanges();
            this.AddToastMessage("Confirmation de la suppression",
                "Le devis " + '\u0022' + devisMinistere.Nom + '\u0022' + " a bien été supprimé.",
                ToastType.Success);
            return RedirectToAction("Index");

        }

        protected override void Dispose(bool disposer)
        {
            if (disposer)
            {
                _db.Dispose();
            }

            base.Dispose(disposer);
        }

        private bool DevisExiste(DevisMinistere devisMinistere)
        {
            return _db.DevisMinistere.Any(tableDevisMinistere =>
                tableDevisMinistere.Discipline == devisMinistere.Discipline && tableDevisMinistere.Annee == devisMinistere.Annee &&
                tableDevisMinistere.CodeSpecialisation == devisMinistere.CodeSpecialisation && tableDevisMinistere.IdDevis != devisMinistere.IdDevis);
        }

    }
}
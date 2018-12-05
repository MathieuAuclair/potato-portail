using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Controllers;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConsoleDevisMinistereController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

        public ActionResult _PartialList()
        {
            //var devisMinistere = _db.DevisMinistere.Include(p => p.EnteteProgramme);
            //return PartialView(devisMinistere.ToList());
            throw new NotImplementedException();
        }

        public ActionResult Index()
        {
            //var devisMinistere = _db.DevisMinistere.Include(p => p.EnteteProgramme);

            //return View(devisMinistere.ToList());
            throw new NotImplementedException();
        }

        public ActionResult Creation()
        {
            ViewBag.codeProgramme = ConstruireCodeDevisMinistereSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "codeProgramme, annee, codeSpecialisation")]
            DevisMinistere devisMinistere)
        {
            if (!DevisExiste(devisMinistere) && ModelState.IsValid)
            {
                devisMinistere.CodeSpecialisation = devisMinistere.CodeSpecialisation.ToUpper().Trim();
                _db.DevisMinistere.Add(devisMinistere);
                _db.SaveChanges();
                this.AddToastMessage("Création confirmée",
                    "Le devis " + '\u0022' + devisMinistere.Nom + '\u0022' + " a bien été créé.",
                    PotatoPortail.Toast.ToastType.Success);
                return RedirectToAction("Index");
            }

            if (DevisExiste(devisMinistere))
            {
                this.AddToastMessage("Création confirmée",
                    "Le devis " + '\u0022' + devisMinistere.Nom + '\u0022' + " n'a pas été créé.",
                    PotatoPortail.Toast.ToastType.Error);
                ModelState.AddModelError("Duplique", @"Erreur, ce devis existe déjà.");
            }

            ViewBag.codeProgramme = ConstruireCodeDevisMinistereSelectList();
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

            ViewBag.codeProgramme = ConstruireCodeDevisMinistereSelectList(devisMinistere.codeProgramme);
            return View(devisMinistere);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "idDevis, codeProgramme, annee, codeSpecialisation")]
            DevisMinistere devisMinistere)
        {
            if (!DevisExiste(devisMinistere) && ModelState.IsValid)
            {
                devisMinistere.CodeSpecialisation = devisMinistere.CodeSpecialisation.ToUpper();
                _db.Entry(devisMinistere).State = EntityState.Modified;
                _db.SaveChanges();
                this.AddToastMessage("Confirmation de la modification",
                    "Le devis " + '\u0022' + devisMinistere.Nom + '\u0022' + " a bien été modifié.",
                    PotatoPortail.Toast.ToastType.Success);
                return RedirectToAction("Index");
            }

            if (DevisExiste(devisMinistere))
            {
                this.AddToastMessage("Confirmation de la modification",
                    "Le devis " + '\u0022' + devisMinistere.Nom + '\u0022' + " n'a pas été modifié.",
                    PotatoPortail.Toast.ToastType.Error);
                ModelState.AddModelError("Duplique", @"Erreur, ce devis ministeriel existe déjà.");
            }

            ViewBag.codeProgramme = ConstruireCodeDevisMinistereSelectList();
            return View(devisMinistere);
        }

        public SelectList ConstruireCodeDevisMinistereSelectList(string codeProgramme = null)
        {
            //var liste = _db.EnteteProgramme
            //    .Select(e => new {codeProgramme = e.codeProgramme, texte = e.codeProgramme + " - " + e.nom}).ToList();
            if (codeProgramme != null)
                return new SelectList(liste, "codeProgramme", "texte", codeProgramme);
            return new SelectList(liste, "codeProgramme", "texte");
        }

        [ActionName("Supression")]
        public ActionResult SurpressionConfirmer(int id)
        {
            DevisMinistere devisMinistere = _db.DevisMinistere.Find(id);
            _db.DevisMinistere.Remove(devisMinistere);
            _db.SaveChanges();
            this.AddToastMessage("Confirmation de la suppression",
                "Le devis " + '\u0022' + devisMinistere.nom + '\u0022' + " a bien été supprimé.",
                PotatoPortail.Toast.ToastType.Success);
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
            bool existe = _db.DevisMinistere.Any(p =>
                p.codeProgramme == devisMinistere.codeProgramme && p.annee == devisMinistere.annee &&
                p.codeSpecialisation == devisMinistere.codeSpecialisation && p.idDevis != devisMinistere.idDevis);
            return existe;
        }
    }
}
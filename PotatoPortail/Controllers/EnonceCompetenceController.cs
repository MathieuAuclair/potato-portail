using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Helpers;
using PotatoPortail.Migrations;
using PotatoPortail.Models;
using PotatoPortail.Toast;

namespace PotatoPortail.Controllers
{
    [Authorize(Roles = "RCP")]
    public class EnonceCompetenceController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

        public ActionResult Info(int? idCompetence)
        {
            if (idCompetence == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EnonceCompetence enonceCompetence = _db.EnonceCompetence.Find(idCompetence);
            if (enonceCompetence == null)
            {
                return HttpNotFound();
            }

            TempData["enonceCompetence"] = enonceCompetence;
            return View(enonceCompetence);
        }

        public ActionResult Annuler(string currentUrl)
        {
            if (currentUrl == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return Redirect(currentUrl);
        }

        
        [Authorize(Roles = "RCP")]
        public ActionResult Creation(int? idDevis)
        {
            if (idDevis == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DevisMinistere devisMinistere = _db.DevisMinistere.Find(idDevis);
            if (devisMinistere == null)
            {
                return HttpNotFound();
            }

            EnonceCompetence enonceCompetence = new EnonceCompetence
            {
                Obligatoire = true, Actif = true, IdDevis = devisMinistere.IdDevis
            };
            return View(enonceCompetence);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "RCP")]
        public ActionResult Creation([Bind(Include =
                "idCompetence,codeCompetence,description,motClef,obligatoire,actif,commentaire,idDevis")]
            EnonceCompetence enonceCompetence)
        {
            var existe = _db.EnonceCompetence.Any(ec =>
                ec.CodeCompetence == enonceCompetence.CodeCompetence && ec.IdDevis == enonceCompetence.IdDevis);
            Trim(enonceCompetence);
            if (!existe && ModelState.IsValid)
            {
                this.AddToastMessage("Confirmation de la création",
                    "L'énoncé de compétence " + '\u0022' + enonceCompetence.Description + '\u0022' +
                    ", a bien été créé.", ToastType.Success);
                enonceCompetence.CodeCompetence = enonceCompetence.CodeCompetence.ToUpper();
                _db.EnonceCompetence.Add(enonceCompetence);
                _db.SaveChanges();
                return RedirectToAction("Creation", "ContexteRealisation", new {enonceCompetence.IdCompetence});
            }

            if (existe)
            {
                ModelState.AddModelError("Duplique", "Erreur, un énoncé de compétence avec ce code existe déjà.");
                this.AddToastMessage("Problème lors de la création",
                    "L'énoncé de compétence " + '\u0022' + enonceCompetence.Description + '\u0022' +
                    ", n'a pas pus être créé.", ToastType.Error, true);
            }
            return View(enonceCompetence);
        }

        public ActionResult Modifier(int? idCompetence)
        {
            if (idCompetence == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EnonceCompetence enonceCompetence = _db.EnonceCompetence.Find(idCompetence);
            if (enonceCompetence == null)
            {
                return HttpNotFound();
            }

            return View(enonceCompetence);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include =
                "idCompetence,codeCompetence,description,motClef,obligatoire,actif,commentaire,idDevis")]
            EnonceCompetence enonceCompetence)
        {
            var existe = _db.EnonceCompetence.Any(ec =>
                ec.IdCompetence != enonceCompetence.IdCompetence &&
                ec.CodeCompetence == enonceCompetence.CodeCompetence && ec.IdDevis == enonceCompetence.IdDevis);
            Trim(enonceCompetence);
            if (!existe && ModelState.IsValid)
            {
                this.AddToastMessage("Confirmation de la modification",
                    "L'énoncé de compétence " + '\u0022' + enonceCompetence.Description + '\u0022' +
                    ", a bien été modifié.", ToastType.Success);
                _db.Entry(enonceCompetence).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Creation", "ContexteRealisation", new {enonceCompetence.IdCompetence});
            }

            if (existe)
            {
                this.AddToastMessage("Problème lors de la modification",
                    "Erreur, un énoncé de compétence avec ce code existe déjà.", ToastType.Error, true);
            }

            return View(enonceCompetence);
        }

        public ActionResult Supression(int? idCompetence)
        {
            if (idCompetence == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var enonceCompetence = _db.EnonceCompetence.Find(idCompetence);
            if (enonceCompetence == null)
            {
                return HttpNotFound();
            }

            return View(enonceCompetence);
        }

        [HttpPost, ActionName("Supression")]
        [ValidateAntiForgeryToken]
        public ActionResult SurpressionConfirmer(int idCompetence)
        {
            var planCadreEnonce = from pc in _db.PlanCadreCompetence
                where pc.IdCompetence == idCompetence
                select pc;
            var enonceCompetence = _db.EnonceCompetence.Find(idCompetence);
            if (!planCadreEnonce.Any())
            {
                if (enonceCompetence == null)
                {
                    return HttpNotFound();
                }

                foreach (var ec in enonceCompetence.ElementCompetence)
                {
                    _db.CriterePerformance.RemoveRange(ec.CriterePerformance);
                }

                _db.ElementCompetence.RemoveRange(enonceCompetence.ElementCompetence);
                _db.ContexteRealisation.RemoveRange(enonceCompetence.ContexteRealisation);
                _db.EnonceCompetence.Remove(enonceCompetence);
                _db.SaveChanges();
                this.AddToastMessage("Confirmation de la supression",
                    "L'énoncé de compétence " + '\u0022' + enonceCompetence.Description + '\u0022' +
                    ", a bien été supprimmé.", ToastType.Success);
            }
            else
            {
                if (enonceCompetence == null)
                {
                    return HttpNotFound();
                }

                this.AddToastMessage("Problème lors de la supression",
                        "L'énoncé de compétence " + '\u0022' + enonceCompetence.Description + '\u0022' +
                        ", n'a pas pus être supprimmé.", ToastType.Error, true);
            }

            return RedirectToAction("Info", "DevisMinistere", new {enonceCompetence.IdDevis});
        }

        [HttpPost]
        public void MettreAjoursOrdre(List<ElementCompetence> listeElement)
        {
            foreach (var item in listeElement)
            {
                var element = _db.ElementCompetence.Find(item.IdElement);
                if (element != null)
                {
                    element.Numero = item.Numero;
                }
            }

            _db.SaveChanges();
        }

        private static void Trim(EnonceCompetence enonceCompetence)
        {
            if (enonceCompetence.Description != null)
                enonceCompetence.Description = enonceCompetence.Description.Trim();
        }

        protected override void Dispose(bool disposer)
        {
            if (disposer)
            {
                _db.Dispose();
            }

            base.Dispose(disposer);
        }
    }
}
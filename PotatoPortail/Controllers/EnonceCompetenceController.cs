using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApplicationPlanCadre.Models;
using ApplicationPlanCadre.Helpers;

namespace ApplicationPlanCadre.Controllers
{
    [RCPEnonceCompetenceAuthorize]
    public class EnonceCompetenceController : Controller
    {
        private BDPlanCadre db = new BDPlanCadre();

        public ActionResult Info(int? idCompetence)
        {
            if (idCompetence == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EnonceCompetence enonceCompetence = db.EnonceCompetence.Find(idCompetence);
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

        [RCPDevisMinistereAuthorize]
        public ActionResult Creation(int? idDevis)
        {
            if (idDevis == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DevisMinistere devisMinistere = db.DevisMinistere.Find(idDevis);
            if (devisMinistere == null)
            {
                return HttpNotFound();
            }

            EnonceCompetence enonceCompetence = new EnonceCompetence();
            enonceCompetence.obligatoire = true;
            enonceCompetence.actif = true;
            enonceCompetence.idDevis = devisMinistere.idDevis;
            return View(enonceCompetence);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RCPDevisMinistereAuthorize]
        public ActionResult Creation([Bind(Include =
                "idCompetence,codeCompetence,description,motClef,obligatoire,actif,commentaire,idDevis")]
            EnonceCompetence enonceCompetence)
        {
            bool existe;
            existe = db.EnonceCompetence.Any(ec =>
                ec.codeCompetence == enonceCompetence.codeCompetence && ec.idDevis == enonceCompetence.idDevis);
            Trim(enonceCompetence);
            if (!existe && ModelState.IsValid)
            {
                this.AddToastMessage("Confirmation de la création",
                    "L'énoncé de compétence " + '\u0022' + enonceCompetence.description + '\u0022' +
                    ", a bien été créé.", Toast.ToastType.Success);
                enonceCompetence.codeCompetence = enonceCompetence.codeCompetence.ToUpper();
                db.EnonceCompetence.Add(enonceCompetence);
                db.SaveChanges();
                return RedirectToAction("Creation", "ContexteRealisation", new {enonceCompetence.idCompetence});
            }

            if (existe)
            {
                ModelState.AddModelError("Duplique", "Erreur, un énoncé de compétence avec ce code existe déjà.");
                this.AddToastMessage("Problème lors de la création",
                    "L'énoncé de compétence " + '\u0022' + enonceCompetence.description + '\u0022' +
                    ", n'a pas pus être créé.", Toast.ToastType.Error, true);
            }
            return View(enonceCompetence);
        }

        public ActionResult Modifier(int? idCompetence)
        {
            if (idCompetence == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EnonceCompetence enonceCompetence = db.EnonceCompetence.Find(idCompetence);
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
            bool existe;
            existe = db.EnonceCompetence.Any(ec =>
                ec.idCompetence != enonceCompetence.idCompetence &&
                ec.codeCompetence == enonceCompetence.codeCompetence && ec.idDevis == enonceCompetence.idDevis);
            Trim(enonceCompetence);
            if (!existe && ModelState.IsValid)
            {
                this.AddToastMessage("Confirmation de la modification",
                    "L'énoncé de compétence " + '\u0022' + enonceCompetence.description + '\u0022' +
                    ", a bien été modifié.", Toast.ToastType.Success);
                db.Entry(enonceCompetence).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Creation", "ContexteRealisation", new {enonceCompetence.idCompetence});
            }

            if (existe)
            {
                this.AddToastMessage("Problème lors de la modification",
                    "Erreur, un énoncé de compétence avec ce code existe déjà.", Toast.ToastType.Error, true);
            }

            return View(enonceCompetence);
        }

        public ActionResult Supression(int? idCompetence)
        {
            if (idCompetence == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EnonceCompetence enonceCompetence = db.EnonceCompetence.Find(idCompetence);
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
            var PlanCadreEnonce = from pc in db.PlanCadreCompetence
                where pc.idCompetence == idCompetence
                select pc;
            EnonceCompetence enonceCompetence = db.EnonceCompetence.Find(idCompetence);
            if (PlanCadreEnonce.Count() == 0)
            {
                foreach (ElementCompetence ec in enonceCompetence.ElementCompetence)
                {
                    db.CriterePerformance.RemoveRange(ec.CriterePerformance);
                }

                db.ElementCompetence.RemoveRange(enonceCompetence.ElementCompetence);
                db.ContexteRealisation.RemoveRange(enonceCompetence.ContexteRealisation);
                db.EnonceCompetence.Remove(enonceCompetence);
                db.SaveChanges();
                this.AddToastMessage("Confirmation de la supression",
                    "L'énoncé de compétence " + '\u0022' + enonceCompetence.description + '\u0022' +
                    ", a bien été supprimmé.", Toast.ToastType.Success);
            }
            else
            {
                this.AddToastMessage("Problème lors de la supression",
                    "L'énoncé de compétence " + '\u0022' + enonceCompetence.description + '\u0022' +
                    ", n'a pas pus être supprimmé.", Toast.ToastType.Error, true);
            }
            return RedirectToAction("Info", "DevisMinistere", new {enonceCompetence.idDevis});
        }

        [HttpPost]
        public void MettreAjoursOrdre(List<ElementCompetence> listeElement)
        {
            foreach (var item in listeElement)
            {
                var element = db.ElementCompetence.Find(item.idElement);
                if (element != null)
                {
                    element.numero = item.numero;
                }
            }

            db.SaveChanges();
        }

        private void Trim(EnonceCompetence enonceCompetence)
        {
            if (enonceCompetence.description != null)
                enonceCompetence.description = enonceCompetence.description.Trim();
        }

        protected override void Dispose(bool disposer)
        {
            if (disposer)
            {
                db.Dispose();
            }

            base.Dispose(disposer);
        }
    }
}
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ApplicationPlanCadre.Models;
using PotatoPortail.Helpers;
using PotatoPortail.Migrations;

namespace PotatoPortail.Controllers
{
    [RCPEnonceCompetenceAuthorize]
    public class EnonceCompetenceController : Controller
    {
        private readonly DatabaseContext _db = new DatabaseContext();

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

        [RCPDevisMinistereAuthorize]
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
                obligatoire = true, actif = true, idDevis = devisMinistere.idDevis
            };

            return View(enonceCompetence);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RCPDevisMinistereAuthorize]
        public ActionResult Creation([Bind(Include =
                "idCompetence,codeCompetence,description,motClef,obligatoire,actif,commentaire,idDevis")]
            EnonceCompetence enonceCompetence)
        {
            var existe = _db.EnonceCompetence.Any(ec =>
                ec.codeCompetence == enonceCompetence.codeCompetence && ec.idDevis == enonceCompetence.idDevis);
            Trim(enonceCompetence);
            if (!existe && ModelState.IsValid)
            {
                this.AddToastMessage("Confirmation de la création",
                    "L'énoncé de compétence " + '\u0022' + enonceCompetence.description + '\u0022' +
                    ", a bien été créé.", Toast.ToastType.Success);
                enonceCompetence.codeCompetence = enonceCompetence.codeCompetence.ToUpper();
                _db.EnonceCompetence.Add(enonceCompetence);
                _db.SaveChanges();
                return RedirectToAction("Creation", "ContexteRealisation",
                    new {idCompetence = enonceCompetence.idCompetence});
            }

            if (existe)
            {
                ModelState.AddModelError("Duplique", @"Erreur, un énoncé de compétence avec ce code existe déjà.");
                this.AddToastMessage("Confirmation de la création",
                    "L'énoncé de compétence " + '\u0022' + enonceCompetence.description + '\u0022' +
                    ", n'a pas pus être créé.", Toast.ToastType.Error);
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
                ec.idCompetence != enonceCompetence.idCompetence &&
                ec.codeCompetence == enonceCompetence.codeCompetence && ec.idDevis == enonceCompetence.idDevis);
            Trim(enonceCompetence);
            if (!existe && ModelState.IsValid)
            {
                this.AddToastMessage("Confirmation de la modification",
                    "L'énoncé de compétence " + '\u0022' + enonceCompetence.description + '\u0022' +
                    ", a bien été modifié.", Toast.ToastType.Success);
                _db.Entry(enonceCompetence).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Creation", "ContexteRealisation",
                    new {idCompetence = enonceCompetence.idCompetence});
            }

            if (existe)
            {
                this.AddToastMessage("Confirmation de la modification",
                    "L'énoncé de compétence " + '\u0022' + enonceCompetence.description + '\u0022' +
                    ", n'a pas pus être modifié.", Toast.ToastType.Error);
                ModelState.AddModelError("Duplique", @"Erreur, un énoncé de compétence avec ce code existe déjà.");
            }

            return View(enonceCompetence);
        }

        public ActionResult Supression(int? idCompetence)
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

        [HttpPost, ActionName("Supression")]
        [ValidateAntiForgeryToken]
        public ActionResult SurpressionConfirmer(int idCompetence)
        {
            var planCadreEnonce = from pc in _db.PlanCadreEnonce
                where pc.idCompetence == idCompetence
                select pc;
            EnonceCompetence enonceCompetence = _db.EnonceCompetence.Find(idCompetence);
            if (enonceCompetence == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (!planCadreEnonce.Any())
            {
                foreach (ElementCompetence ec in enonceCompetence.ElementCompetence)
                {
                    _db.CriterePerformance.RemoveRange(ec.CriterePerformance);
                }

                _db.ElementCompetence.RemoveRange(enonceCompetence.ElementCompetence);
                _db.ContexteRealisation.RemoveRange(enonceCompetence.ContexteRealisation);
                _db.EnonceCompetence.Remove(enonceCompetence);
                _db.SaveChanges();
                
                this.AddToastMessage("Confirmation de la supression",
                    "L'énoncé de compétence " + '\u0022' + enonceCompetence.description + '\u0022' +
                    ", a bien été supprimmé.", Toast.ToastType.Success);
            }
            else
            {
                this.AddToastMessage("Confirmation de la supression",
                    "L'énoncé de compétence " + '\u0022' + enonceCompetence.description + '\u0022' +
                    ", n'a pas pus être supprimmé.", Toast.ToastType.Error);
            }

            return RedirectToAction("Info", "DevisMinistere", new {idDevis = enonceCompetence.idDevis});
        }

        [HttpPost]
        public void MettreAjoursOrdre(List<ElementCompetence> listeElement)
        {
            foreach (var item in listeElement)
            {
                var element = _db.ElementCompetence.Find(item.idElement);
                if (element != null)
                {
                    element.numero = item.numero;
                }
            }

            _db.SaveChanges();
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
                _db.Dispose();
            }

            base.Dispose(disposer);
        }
    }
}
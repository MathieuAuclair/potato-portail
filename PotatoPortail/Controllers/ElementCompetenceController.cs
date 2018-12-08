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
    [RcpElementCompetenceAuthorize]
    public class ElementCompetenceController : Controller
    {
        private readonly BdPortail _db = new BdPortail();
        public ActionResult Info(int? idElement)
        {
            if (idElement == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ElementCompetence elementCompetence = _db.ElementCompetence.Find(idElement);
            if (elementCompetence == null)
            {
                return HttpNotFound();
            }
            return View(elementCompetence);
        }

        public ActionResult Annuler(string currentUrl)
        {
            if (currentUrl == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return Redirect(currentUrl);
        }

        [RcpEnonceCompetenceAuthorize]
        public ActionResult Creation(int? idCompetence)
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

            ElementCompetence elementCompetence = new ElementCompetence
            {
                EnonceCompetence = enonceCompetence, IdCompetence = enonceCompetence.IdCompetence
            };

            return View(elementCompetence);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RcpEnonceCompetenceAuthorize]
        public ActionResult Creation([Bind(Include = "idElement,description,numero,motClef,commentaire,idCompetence")] ElementCompetence elementCompetence)
        {
            AssignerNo(elementCompetence);
            Trim(elementCompetence);
            if (ModelState.IsValid)
            {
                this.AddToastMessage("Confirmation de la création", "L'élément de compétence "+ '\u0022' + elementCompetence.Description + '\u0022'+ ", a bien été crée.", ToastType.Success);
                _db.ElementCompetence.Add(elementCompetence);
                _db.SaveChanges();
                return RedirectToAction("Creation", "CriterePerformance", new {elementCompetence.IdElement});
            }
            else
            {
                this.AddToastMessage("Confirmation de la création", "L'élément de compétence " + '\u0022' + elementCompetence.Description + '\u0022'+ ", n'a pas pus être crée.", ToastType.Error);
            }
            return View(elementCompetence);
        }

        [HttpPost]
        public void MettreAjoursOrdre(List<CriterePerformance> listeElement)
        {
            foreach (var item in listeElement)
            {
                var element = _db.CriterePerformance.Find(item.IdElement);

                if (element != null)
                {
                    element.Numero = item.Numero;
                }
            }
            _db.SaveChanges();

        }
        public ActionResult Modifier(int? idElement)
        {
            if (idElement == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ElementCompetence elementCompetence = _db.ElementCompetence.Find(idElement);
            if (elementCompetence == null)
            {
                return HttpNotFound();
            }
            return View(elementCompetence);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "idElement,description,numero,motClef,commentaire,idCompetence")] ElementCompetence elementCompetence)
        {
            Trim(elementCompetence);
            if (ModelState.IsValid)
            {
                this.AddToastMessage("Confirmation de la modificaion", "L'élément de compétence " + '\u0022' + elementCompetence.Description + '\u0022' + ", a bien été modifié.", ToastType.Success);
                _db.Entry(elementCompetence).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Creation", "CriterePerformance", new {elementCompetence.IdElement});
            }
            else
            {
                this.AddToastMessage("Confirmation de la modificaion", "L'élément de compétence " + '\u0022'  + elementCompetence.Description + '\u0022'+ ", n'a pas pus être modifié.", ToastType.Error);
            }
            return View(elementCompetence);
        }

        public ActionResult Supression(int? idElement)
        {
            if (idElement == null)
            {
                this.AddToastMessage("Confirmation de la supression", "L'élément de compétence, n'a pas pus être supprimé.", ToastType.Error);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var elementCompetence = _db.ElementCompetence.Find(idElement);
            if (elementCompetence != null) return View(elementCompetence);
            this.AddToastMessage("Confirmation de la supression", "L'élément de compétence, n'a pas pus être supprimé.", ToastType.Error);
            return HttpNotFound();

        }

        [HttpPost, ActionName("Supression")]
        [ValidateAntiForgeryToken]
        public ActionResult SurpressionConfirmer(int idElement)
        {
            var planCadreElement = from tablePlanCadre in _db.PlanCadreElement
                                   where tablePlanCadre.IdElement == idElement
                                   select tablePlanCadre;
            var elementCompetence = _db.ElementCompetence.Find(idElement);

            if (elementCompetence == null)
            {
                return HttpNotFound();
            }

            if(!planCadreElement.Any())
            {
                _db.CriterePerformance.RemoveRange(elementCompetence.CriterePerformance);
                _db.ElementCompetence.Remove(elementCompetence);
                AjusterNo(elementCompetence);
                _db.SaveChanges();
                this.AddToastMessage("Confirmation de la supression", "L'élément de compétence : " + '\u0022' + elementCompetence.Description + '\u0022' + ", a bien été supprimé.", ToastType.Success);
            }
            else
            {
                this.AddToastMessage("Confirmation de la supression", "L'élément de compétence  " + '\u0022' + elementCompetence.Description + '\u0022' + ", n'a pas pus être supprimé.", ToastType.Error);
            }
            return RedirectToAction("Info", "EnonceCompetence", new {elementCompetence.IdCompetence});
        }

        private void AssignerNo(ElementCompetence elementCompetence)
        {
            var dernierNo = 0;

            var requete = (from tableElementCompetence in _db.ElementCompetence
                                     where tableElementCompetence.IdCompetence == elementCompetence.IdCompetence
                                     select tableElementCompetence.Numero);
            if (requete.Any())
            {
                dernierNo = requete.Max();
            }
            elementCompetence.Numero = dernierNo + 1;
        }

        private void AjusterNo(ElementCompetence elementCompetence)
        {
            var requete = (from tableElementCompetence in _db.ElementCompetence
                                                    where tableElementCompetence.IdCompetence == elementCompetence.IdCompetence && tableElementCompetence.Numero > elementCompetence.Numero
                                                    select tableElementCompetence);
            foreach (var competence in requete)
            {
                competence.Numero--;
            }
        }

        private static void Trim(ElementCompetence elementCompetence)
        {
            if (elementCompetence.Description != null)
                elementCompetence.Description = elementCompetence.Description.Trim();
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

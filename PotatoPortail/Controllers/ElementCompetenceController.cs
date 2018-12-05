using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PotatoPortail.Models;
using PotatoPortail.Helpers;
using PotatoPortail.Migrations;

namespace ApplicationPlanCadre.Controllers
{
    [RCPElementCompetenceAuthorize]
    public class ElementCompetenceController : Controller
    {
        private BDPlanCadre db = new BDPlanCadre();
        public ActionResult Info(int? idElement)
        {
            if (idElement == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ElementCompetence elementCompetence = db.ElementCompetence.Find(idElement);
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

        [RCPEnonceCompetenceAuthorize]
        public ActionResult Creation(int? idCompetence)
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
            ElementCompetence elementCompetence = new ElementCompetence();
            elementCompetence.EnonceCompetence = enonceCompetence;
            elementCompetence.idCompetence = enonceCompetence.idCompetence;

            return View(elementCompetence);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RCPEnonceCompetenceAuthorize]
        public ActionResult Creation([Bind(Include = "idElement,description,numero,motClef,commentaire,idCompetence")] ElementCompetence elementCompetence)
        {
            AssignerNo(elementCompetence);
            Trim(elementCompetence);
            if (ModelState.IsValid)
            {
                this.AddToastMessage("Confirmation de la création", "L'élément de compétence "+ '\u0022' + elementCompetence.description + '\u0022'+ ", a bien été crée.", Toast.ToastType.Success);
                db.ElementCompetence.Add(elementCompetence);
                db.SaveChanges();
                return RedirectToAction("Creation", "CriterePerformance", new {elementCompetence.idElement});
            }
            else
            {
                this.AddToastMessage("Confirmation de la création", "L'élément de compétence " + '\u0022' + elementCompetence.description + '\u0022'+ ", n'a pas pus être crée.", Toast.ToastType.Error);
            }
            return View(elementCompetence);
        }

        [HttpPost]
        public void MettreAjoursOrdre(List<CriterePerformance> listeElement)
        {
            foreach (var item in listeElement)
            {
                var element = db.CriterePerformance.Find(item.idElement);

                if (element != null)
                {
                    element.Numero = item.Numero;
                }
            }
            db.SaveChanges();

        }
        public ActionResult Modifier(int? idElement)
        {
            if (idElement == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ElementCompetence elementCompetence = db.ElementCompetence.Find(idElement);
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
                this.AddToastMessage("Confirmation de la modificaion", "L'élément de compétence " + '\u0022' + elementCompetence.description + '\u0022' + ", a bien été modifié.", Toast.ToastType.Success);
                db.Entry(elementCompetence).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Creation", "CriterePerformance", new {elementCompetence.idElement});
            }
            else
            {
                this.AddToastMessage("Confirmation de la modificaion", "L'élément de compétence " + '\u0022'  + elementCompetence.description + '\u0022'+ ", n'a pas pus être modifié.", Toast.ToastType.Error);
            }
            return View(elementCompetence);
        }

        public ActionResult Supression(int? idElement)
        {
            if (idElement == null)
            {
                this.AddToastMessage("Confirmation de la supression", "L'élément de compétence, n'a pas pus être supprimé.", Toast.ToastType.Error);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ElementCompetence elementCompetence = db.ElementCompetence.Find(idElement);
            if (elementCompetence == null)
            {
                this.AddToastMessage("Confirmation de la supression", "L'élément de compétence, n'a pas pus être supprimé.", Toast.ToastType.Error);
                return HttpNotFound();
            }
           
            return View(elementCompetence);
        }

        [HttpPost, ActionName("Supression")]
        [ValidateAntiForgeryToken]
        public ActionResult SurpressionConfirmer(int idElement)
        {
            var PlanCadreElement = from pc in db.PlanCadreElement
                                   where pc.idElement == idElement
                                   select pc;
            ElementCompetence elementCompetence = db.ElementCompetence.Find(idElement);
            if(PlanCadreElement.Count() == 0)
            {
                db.CriterePerformance.RemoveRange(elementCompetence.CriterePerformance);
                db.ElementCompetence.Remove(elementCompetence);
                AjusterNo(elementCompetence);
                db.SaveChanges();
                this.AddToastMessage("Confirmation de la supression", "L'élément de compétence : " + '\u0022' + elementCompetence.description + '\u0022' + ", a bien été supprimé.", Toast.ToastType.Success);
            }
            else
            {
                this.AddToastMessage("Confirmation de la supression", "L'élément de compétence  " + '\u0022' + elementCompetence.description + '\u0022' + ", n'a pas pus être supprimé.", Toast.ToastType.Error);
            }
            return RedirectToAction("Info", "EnonceCompetence", new {elementCompetence.idCompetence});
        }

        private void AssignerNo(ElementCompetence elementCompetence)
        {
            int dernierNo = 0;

            IQueryable<int> requete = (from ec in db.ElementCompetence
                                     where ec.idCompetence == elementCompetence.idCompetence
                                     select ec.numero);
            if (requete.Count() > 0)
            {
                dernierNo = requete.Max();
            }
            elementCompetence.numero = dernierNo + 1;
        }

        private void AjusterNo(ElementCompetence elementCompetence)
        {
            IQueryable<ElementCompetence> requete = (from ec in db.ElementCompetence
                                                    where ec.idCompetence == elementCompetence.idCompetence && ec.numero > elementCompetence.numero
                                                    select ec);
            foreach (ElementCompetence ec in requete)
            {
                ec.Numero--;
            }
        }

        private void Trim(ElementCompetence elementCompetence)
        {
            if (elementCompetence.Description != null)
                elementCompetence.Description = elementCompetence.Description.Trim();
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

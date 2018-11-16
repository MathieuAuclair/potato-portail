using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ApplicationPlanCadre.Models;
using SysInternshipManagement.Helpers;
using SysInternshipManagement.Migrations;

namespace SysInternshipManagement.Controllers
{
    [RCPElementCompetenceAuthorize]
    public class ElementCompetenceController : Controller
    {
        private readonly DatabaseContext _db = new DatabaseContext();

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

        [RCPEnonceCompetenceAuthorize]
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
                EnonceCompetence = enonceCompetence, 
                idCompetence = enonceCompetence.idCompetence
            };
            return View(elementCompetence);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RCPEnonceCompetenceAuthorize]
        public ActionResult Creation([Bind(Include = "idElement,description,numero,motClef,commentaire,idCompetence")]
            ElementCompetence elementCompetence)
        {
            AssignerNo(elementCompetence);
            Trim(elementCompetence);
            if (ModelState.IsValid)
            {
                this.AddToastMessage("Confirmation de la création",
                    "L'élément de compétence " + '\u0022' + elementCompetence.description + '\u0022' +
                    ", a bien été crée.", Toast.ToastType.Success);
                _db.ElementCompetence.Add(elementCompetence);
                _db.SaveChanges();
                return RedirectToAction("Creation", "CriterePerformance", new {elementCompetence.idElement});
            }
            else
            {
                this.AddToastMessage("Confirmation de la création",
                    "L'élément de compétence " + '\u0022' + elementCompetence.description + '\u0022' +
                    ", n'a pas pus être crée.", Toast.ToastType.Error);
            }

            return View(elementCompetence);
        }

        [HttpPost]
        public void MettreAjoursOrdre(List<CriterePerformance> listeElement)
        {
            foreach (var item in listeElement)
            {
                var element = _db.CriterePerformance.Find(item.idElement);
                if (element != null)
                {
                    element.numero = item.numero;
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
        public ActionResult Modifier([Bind(Include = "idElement,description,numero,motClef,commentaire,idCompetence")]
            ElementCompetence elementCompetence)
        {
            Trim(elementCompetence);
            if (ModelState.IsValid)
            {
                this.AddToastMessage("Confirmation de la modificaion",
                    "L'élément de compétence " + '\u0022' + elementCompetence.description + '\u0022' +
                    ", a bien été modifié.", Toast.ToastType.Success);
                _db.Entry(elementCompetence).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Creation", "CriterePerformance",
                    new {idElement = elementCompetence.idElement});
            }
            else
            {
                this.AddToastMessage("Confirmation de la modificaion",
                    "L'élément de compétence " + '\u0022' + elementCompetence.description + '\u0022' +
                    ", n'a pas pus être modifié.", Toast.ToastType.Error);
            }

            return View(elementCompetence);
        }

        public ActionResult Supression(int? idElement)
        {
            if (idElement == null)
            {
                this.AddToastMessage("Confirmation de la supression",
                    "L'élément de compétence, n'a pas pus être supprimé.", Toast.ToastType.Error);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ElementCompetence elementCompetence = _db.ElementCompetence.Find(idElement);
            if (elementCompetence == null)
            {
                this.AddToastMessage("Confirmation de la supression",
                    "L'élément de compétence, n'a pas pus être supprimé.", Toast.ToastType.Error);
                return HttpNotFound();
            }

            return View(elementCompetence);
        }

        [HttpPost, ActionName("Supression")]
        [ValidateAntiForgeryToken]
        public ActionResult SurpressionConfirmer(int idElement)
        {
            var planCadreElement = from pc in _db.PlanCadreElement
                where pc.idElement == idElement
                select pc;
            ElementCompetence elementCompetence = _db.ElementCompetence.Find(idElement);

            if (elementCompetence == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (!planCadreElement.Any())
            {
                _db.CriterePerformance.RemoveRange(elementCompetence.CriterePerformance);
                _db.ElementCompetence.Remove(elementCompetence);
                AjusterNo(elementCompetence);
                _db.SaveChanges();
                this.AddToastMessage("Confirmation de la supression",
                    "L'élément de compétence : " + '\u0022' + elementCompetence.description + '\u0022' +
                    ", a bien été supprimé.", Toast.ToastType.Success);
            }
            else
            {
                this.AddToastMessage("Confirmation de la supression",
                    "L'élément de compétence  " + '\u0022' + elementCompetence.description + '\u0022' +
                    ", n'a pas pus être supprimé.", Toast.ToastType.Error);
            }

            return RedirectToAction("Info", "EnonceCompetence", new {elementCompetence.idCompetence});
        }

        private void AssignerNo(ElementCompetence elementCompetence)
        {
            int dernierNo = 0;
            IQueryable<int> requete = (from ec in _db.ElementCompetence
                where ec.idCompetence == elementCompetence.idCompetence
                select ec.numero);
            if (requete.Any())
            {
                dernierNo = requete.Max();
            }

            elementCompetence.numero = dernierNo + 1;
        }

        private void AjusterNo(ElementCompetence elementCompetence)
        {
            IQueryable<ElementCompetence> requete = (from ec in _db.ElementCompetence
                where ec.idCompetence == elementCompetence.idCompetence && ec.numero > elementCompetence.numero
                select ec);
            foreach (ElementCompetence ec in requete)
            {
                ec.numero--;
            }
        }

        private void Trim(ElementCompetence elementCompetence)
        {
            if (elementCompetence.description != null)
                elementCompetence.description = elementCompetence.description.Trim();
        }

        protected override void Dispose(bool disposer)
        {
            if (disposer)
            {
                _db.Dispose();
            }

            base.Dispose(disposer);
        }

        public ActionResult DeplacementHaut(int idelement)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult DeplacementBas(int idelement)
        {
            throw new System.NotImplementedException();
        }
    }
}
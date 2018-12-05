using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ApplicationPlanCadre.Models;
using PotatoPortail.Helpers;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers
{
    [RCPCriterePerformanceAuthorize]
    public class CriterePerformanceController : Controller
    {
        private readonly BDPortail _db = new BDPortail();

        public ActionResult _PartialList(int? idElement)
        {
            ElementCompetence elementCompetence = _db.ElementCompetence.Find(idElement);

            if (elementCompetence == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return PartialView(elementCompetence.CriterePerformance.OrderBy(cp => cp.Numero));
        }

        [RCPElementCompetenceAuthorize]
        public ActionResult Creation(int? idElement)
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

            CriterePerformance criterePerformance = new CriterePerformance
            {
                ElementCompetence = elementCompetence,
                IdElement = elementCompetence.IdElement
            };
            return View(criterePerformance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RCPElementCompetenceAuthorize]
        public ActionResult Creation([Bind(Include = "idCritere,description,numero,commentaire,idElement")]
            CriterePerformance criterePerformance)
        {
            AssignerNo(criterePerformance);
            Trim(criterePerformance);
            if (ModelState.IsValid)
            {
                _db.CriterePerformance.Add(criterePerformance);
                _db.SaveChanges();
                this.AddToastMessage("Confirmation de la creation",
                    "Le critère de performance " + '\u0022' + criterePerformance.Description + '\u0022' +
                    " a bien été créé.", Toast.ToastType.Success);
                return RedirectToAction("Creation", new {idElement = criterePerformance.IdElement});
            }

            this.AddToastMessage("Confirmation de la creation",
                "Le critère de performance " + '\u0022' + criterePerformance.Description + '\u0022' +
                " n'a pas été créé.", Toast.ToastType.Error);

            criterePerformance.ElementCompetence = _db.ElementCompetence.Find(criterePerformance.IdElement);
            return View(criterePerformance);
        }

        public ActionResult Modifier(int? idCritere)
        {
            if (idCritere == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CriterePerformance criterePerformance = _db.CriterePerformance.Find(idCritere);
            if (criterePerformance == null)
            {
                return HttpNotFound();
            }

            return View(criterePerformance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "idCritere,description,numero,commentaire,idElement")]
            CriterePerformance criterePerformance)
        {
            Trim(criterePerformance);
            if (ModelState.IsValid)
            {
                this.AddToastMessage("Confirmation de la modification",
                    "Le critère de performance " + '\u0022' + criterePerformance.Description + '\u0022' +
                    " a bien été modifié.", Toast.ToastType.Success);
                _db.Entry(criterePerformance).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Creation", new {idElement = criterePerformance.IdElement });
            }

            this.AddToastMessage("Confirmation de la modification",
                "Le critère de performance " + '\u0022' + criterePerformance.Description + '\u0022' +
                " n'a été modifié.", Toast.ToastType.Error);

            return View(criterePerformance);
        }

        [ActionName("Supression")]
        public ActionResult SurpressionConfirmer(int idCritere)
        {
            CriterePerformance criterePerformance = _db.CriterePerformance.Find(idCritere);

            if (criterePerformance == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            _db.CriterePerformance.Remove(criterePerformance);
            AjusterNo(criterePerformance);
            
            _db.SaveChanges();
            
            this.AddToastMessage("Confirmation de la supression",
                "Le critère de performance " + '\u0022' + criterePerformance.Description + '\u0022' +
                " a bien été suprimmé.", Toast.ToastType.Success);
            
            return RedirectToAction("Creation", new {criterePerformance.IdElement});
        }

        private void AssignerNo(CriterePerformance criterePerformance)
        {
            int dernierNo = 0;
            IQueryable<int> requete = (from cp in _db.CriterePerformance
                where cp.IdElement == criterePerformance.IdElement
                                       select cp.Numero);

            if (requete.Any())
            {
                dernierNo = requete.Max();
            }

            criterePerformance.Numero = dernierNo + 1;
        }

        private void AjusterNo(CriterePerformance criterePerformance)
        {
            IQueryable<CriterePerformance> requete = (from cp in _db.CriterePerformance
                where cp.IdElement == criterePerformance.IdElement && cp.Numero > criterePerformance.Numero
                                                      select cp);
            foreach (CriterePerformance cp in requete)
            {
                cp.Numero--;
            }
        }

        private void Trim(CriterePerformance criterePerformance)
        {
            if (criterePerformance.Description != null)
                criterePerformance.Description = criterePerformance.Description.Trim();
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
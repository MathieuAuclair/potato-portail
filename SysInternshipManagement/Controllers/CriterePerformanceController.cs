using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ApplicationPlanCadre.Models;
using PotatoPortail.Helpers;
using PotatoPortail.Migrations;

namespace PotatoPortail.Controllers
{
    [RCPCriterePerformanceAuthorize]
    public class CriterePerformanceController : Controller
    {
        private readonly DatabaseContext _db = new DatabaseContext();

        public ActionResult _PartialList(int? idElement)
        {
            ElementCompetence elementCompetence = _db.ElementCompetence.Find(idElement);

            if (elementCompetence == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return PartialView(elementCompetence.CriterePerformance.OrderBy(cp => cp.numero));
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
                idElement = elementCompetence.idElement
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
                    "Le critère de performance " + '\u0022' + criterePerformance.description + '\u0022' +
                    " a bien été créé.", Toast.ToastType.Success);
                return RedirectToAction("Creation", new {idElement = criterePerformance.idElement});
            }

            this.AddToastMessage("Confirmation de la creation",
                "Le critère de performance " + '\u0022' + criterePerformance.description + '\u0022' +
                " n'a pas été créé.", Toast.ToastType.Error);

            criterePerformance.ElementCompetence = _db.ElementCompetence.Find(criterePerformance.idElement);
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
                    "Le critère de performance " + '\u0022' + criterePerformance.description + '\u0022' +
                    " a bien été modifié.", Toast.ToastType.Success);
                _db.Entry(criterePerformance).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Creation", new {idElement = criterePerformance.idElement});
            }

            this.AddToastMessage("Confirmation de la modification",
                "Le critère de performance " + '\u0022' + criterePerformance.description + '\u0022' +
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
                "Le critère de performance " + '\u0022' + criterePerformance.description + '\u0022' +
                " a bien été suprimmé.", Toast.ToastType.Success);
            
            return RedirectToAction("Creation", new {criterePerformance.idElement});
        }

        private void AssignerNo(CriterePerformance criterePerformance)
        {
            int dernierNo = 0;
            IQueryable<int> requete = (from cp in _db.CriterePerformance
                where cp.idElement == criterePerformance.idElement
                select cp.numero);

            if (requete.Any())
            {
                dernierNo = requete.Max();
            }

            criterePerformance.numero = dernierNo + 1;
        }

        private void AjusterNo(CriterePerformance criterePerformance)
        {
            IQueryable<CriterePerformance> requete = (from cp in _db.CriterePerformance
                where cp.idElement == criterePerformance.idElement && cp.numero > criterePerformance.numero
                select cp);
            foreach (CriterePerformance cp in requete)
            {
                cp.numero--;
            }
        }

        private void Trim(CriterePerformance criterePerformance)
        {
            if (criterePerformance.description != null)
                criterePerformance.description = criterePerformance.description.Trim();
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
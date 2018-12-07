using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Helpers;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers
{
    [RcpCriterePerformanceAuthorize]
    public class CriterePerformanceController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

        public ActionResult _PartialList(int? idElement)
        {
            var elementCompetence = _db.ElementCompetence.Find(idElement);

            if (elementCompetence == null)
            {
                return HttpNotFound();
            }

            return PartialView(elementCompetence.CriterePerformance.OrderBy(cp => cp.Numero));
        }

        [RcpElementCompetenceAuthorize]
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
                ElementCompetence = elementCompetence, IdElement = elementCompetence.IdElement
            };

            return View(criterePerformance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RcpElementCompetenceAuthorize]
        public ActionResult Creation([Bind(Include = "idCritere,description,numero,commentaire,idElement")]
            CriterePerformance criterePerformance)
        {
            AssignerNo(criterePerformance);
            Trim(criterePerformance);
            if (ModelState.IsValid)
            {
                _db.CriterePerformance.Add(criterePerformance);
                _db.SaveChanges();
                return RedirectToAction("Creation", new {idElement = criterePerformance.IdElement});
            }

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
                _db.Entry(criterePerformance).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Creation", new {idElement = criterePerformance.IdElement});
            }

            return View(criterePerformance);
        }

        [ActionName("Supression")]
        public ActionResult SurpressionConfirmer(int idCritere)
        {
            CriterePerformance criterePerformance = _db.CriterePerformance.Find(idCritere);
            _db.CriterePerformance.Remove(criterePerformance ?? throw new InvalidOperationException());
            AjusterNo();
            _db.SaveChanges();
            return RedirectToAction("Creation", new {idElement = criterePerformance.IdElement});
        }

        private void AssignerNo(CriterePerformance criterePerformance)
        {
            var dernierNo = 0;
            var requete = (from tableCriterePerformance in _db.CriterePerformance
                where tableCriterePerformance.IdElement == criterePerformance.IdElement
                select tableCriterePerformance.Numero);

            if (requete.Any())
            {
                dernierNo = requete.Max();
            }

            criterePerformance.Numero = dernierNo + 1;
        }

        private void AjusterNo()
        {
            var requete = (from tableCriterePerformance
                        in _db.CriterePerformance
                    select tableCriterePerformance
                );
            foreach (var cp in requete)
            {
                cp.Numero--;
            }
        }

        private static void Trim(CriterePerformance criterePerformance)
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
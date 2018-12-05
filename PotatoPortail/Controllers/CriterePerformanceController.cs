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
    [RCPCriterePerformanceAuthorize]
    public class CriterePerformanceController : Controller
    {
        private BDPlanCadre db = new BDPlanCadre();

        public ActionResult _PartialList(int? idElement)
        {
            ElementCompetence elementCompetence = db.ElementCompetence.Find(idElement);
            return PartialView(elementCompetence.CriterePerformance.OrderBy(cp => cp.numero));
        }

        [RCPElementCompetenceAuthorize]
        public ActionResult Creation(int? idElement)
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
            CriterePerformance criterePerformance = new CriterePerformance();
            criterePerformance.ElementCompetence = elementCompetence;
            criterePerformance.idElement = elementCompetence.idElement;
            
            return View(criterePerformance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RCPElementCompetenceAuthorize]
        public ActionResult Creation([Bind(Include = "idCritere,description,numero,commentaire,idElement")] CriterePerformance criterePerformance)
        {
            AssignerNo(criterePerformance);
            Trim(criterePerformance);
            if (ModelState.IsValid)
            {
                db.CriterePerformance.Add(criterePerformance);
                db.SaveChanges();
                return RedirectToAction("Creation", new { idElement = criterePerformance.idElement });
            }
            criterePerformance.ElementCompetence = db.ElementCompetence.Find(criterePerformance.idElement);
            return View(criterePerformance);
        }

        public ActionResult Modifier(int? idCritere)
        {
            if (idCritere == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CriterePerformance criterePerformance = db.CriterePerformance.Find(idCritere);
            if (criterePerformance == null)
            {
                return HttpNotFound();
            }
            return View(criterePerformance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "idCritere,description,numero,commentaire,idElement")] CriterePerformance criterePerformance)
        {
            Trim(criterePerformance);
            if (ModelState.IsValid)
            {
                db.Entry(criterePerformance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Creation", new { idElement = criterePerformance.idElement });
            }
            return View(criterePerformance);
        }

        [ActionName("Supression")]
        public ActionResult SurpressionConfirmer(int idCritere)
        {
            CriterePerformance criterePerformance = db.CriterePerformance.Find(idCritere);
            db.CriterePerformance.Remove(criterePerformance);
            AjusterNo(criterePerformance);
            db.SaveChanges();
            return RedirectToAction("Creation", new { idElement = criterePerformance.idElement });
        }

        private void AssignerNo(CriterePerformance criterePerformance)
        {
            int dernierNo = 0;
            IQueryable<int> requete = (from cp in db.CriterePerformance
                                    where cp.idElement == criterePerformance.idElement
                                    select cp.numero);

            if (requete.Count() > 0)
            {
                dernierNo = requete.Max();
            }
            criterePerformance.numero = dernierNo + 1;
        }

        private void AjusterNo(CriterePerformance criterePerformance)
        {
            IQueryable<CriterePerformance> requete = (from cp in db.CriterePerformance
                                                    where cp.idElement == criterePerformance.idElement && cp.numero > criterePerformance.numero
                                                    select cp);
            foreach(CriterePerformance cp in requete)
            {
                cp.Numero--;
            }
        }

        private void Trim(CriterePerformance criterePerformance)
        {
            if (criterePerformance.description != null) criterePerformance.description = criterePerformance.description.Trim();

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
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PotatoPortail.Models;
using PotatoPortail.Migrations;
using PotatoPortail.ViewModels.PlanCadre;
using Microsoft.Ajax.Utilities;

namespace PotatoPortail.Controllers
{
    
    public class ElementConnaissanceController : Controller
    {
        private BdPortail _db = new BdPortail();

        public ActionResult CreationModification(int? idPlanCadreElement)
        {
            if (idPlanCadreElement == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ElementConnaissanceViewModel elementConnaissanceViewModel = new ElementConnaissanceViewModel();
            elementConnaissanceViewModel.ConnaissanceSousElements = new List<ConnaissanceSousElement>();
            elementConnaissanceViewModel.IdPlanCadreElement = (int)idPlanCadreElement;

            var planCadres = from planCadre in _db.PlanCadre
                             join planCadreCompetence in _db.PlanCadreCompetence on planCadre.IdPlanCadre equals planCadreCompetence.IdPlanCadre
                             join planCadreElement in _db.PlanCadreElement on planCadreCompetence.IdPlanCadreCompetence equals
                                 planCadreElement.IdPlanCadreCompetence
                             where planCadreElement.IdPlanCadreElement == idPlanCadreElement
                             select planCadre;

            elementConnaissanceViewModel.IdPlanCadre = planCadres.First().IdPlanCadre;

            var elementConnaissances = from element in _db.ElementConnaissance
                                         where element.IdPlanCadreElement == idPlanCadreElement
                                         select element;

            foreach (var element in elementConnaissances)
            {
                var elementSousElement = new ConnaissanceSousElement();

                var sousElements = from sousElement in _db.SousElementConnaissance
                                    join elementConnaissance in _db.ElementConnaissance on sousElement.IdElementConnaissance equals elementConnaissance.IdElementConnaissance
                                    where elementConnaissance.IdElementConnaissance == element.IdElementConnaissance
                                    select sousElement;

                elementSousElement.ElementConnaissance = element;

                elementSousElement.SousElementConnaissances =
                    sousElements.Any() ? sousElements.ToList() : new List<SousElementConnaissance>();

                elementConnaissanceViewModel.ConnaissanceSousElements.Add(elementSousElement);
            }

            return View(elementConnaissanceViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreationModification(ElementConnaissanceViewModel elementConnaissanceViewModel)
        {
            if (!ModelState.IsValid) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (elementConnaissanceViewModel.ConnaissanceSousElements == null)
            {
                _db.SaveChanges();
                return RedirectToAction("Structure", "PlanCadre", new { elementConnaissanceViewModel.IdPlanCadre });
            }

            foreach (var element in elementConnaissanceViewModel.ConnaissanceSousElements)
            {
                if (element.ElementConnaissance.Description.IsNullOrWhiteSpace())
                {
                    if (element.ElementConnaissance.IdElementConnaissance != 0)
                    {
                        var dbElement = _db.ElementConnaissance.Find(element.ElementConnaissance.IdElementConnaissance);
                        if (dbElement != null) _db.ElementConnaissance.Remove(dbElement);
                    }

                    continue;
                }

                element.ElementConnaissance.IdPlanCadreElement = elementConnaissanceViewModel.IdPlanCadreElement;
                _db.ElementConnaissance.AddOrUpdate(element.ElementConnaissance);
                _db.SaveChanges();

                if (element.SousElementConnaissances == null) continue;
                foreach (var sousElementConnaissance in element.SousElementConnaissances)
                {
                    if (sousElementConnaissance.DescSousElement.IsNullOrWhiteSpace())
                    {
                        if (sousElementConnaissance.IdSousElement != 0)
                        {
                            var dbSousRessource = _db.SousElementConnaissance.Find(sousElementConnaissance.IdSousElement);
                            if (dbSousRessource != null) _db.SousElementConnaissance.Remove(dbSousRessource);
                        }
                        continue;
                    }

                    sousElementConnaissance.IdElementConnaissance = element.ElementConnaissance.IdElementConnaissance;
                    _db.SousElementConnaissance.AddOrUpdate(sousElementConnaissance);
                }
            }

            _db.SaveChanges();

            return RedirectToAction("Structure", "PlanCadre", new { elementConnaissanceViewModel.IdPlanCadre });
        }
    }
}
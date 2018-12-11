using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Models;
using Microsoft.Ajax.Utilities;
using PotatoPortail.Migrations;
using PotatoPortail.ViewModels.PlanCadre;

namespace PotatoPortail.Controllers
{
    public class ActiviteApprentissageController : Controller
    {
        private BdPortail _db = new BdPortail();

        public ActionResult CreationModification(int? idPlanCadreElement)
        {
            if (idPlanCadreElement == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ActiviteViewModel activiteViewModel = new ActiviteViewModel();
            activiteViewModel.ActiviteSousActivites = new List<ActiviteSousActivite>();
            activiteViewModel.IdPlanCadreElement = (int) idPlanCadreElement;

            var planCadres = from planCadre in _db.PlanCadre
                join planCadreCompetence in _db.PlanCadreCompetence on planCadre.IdPlanCadre equals planCadreCompetence
                    .IdPlanCadre
                join planCadreElement in _db.PlanCadreElement on planCadreCompetence.IdPlanCadreCompetence equals
                    planCadreElement.IdPlanCadreCompetence
                where planCadreElement.IdPlanCadreElement == idPlanCadreElement
                select planCadre;

            activiteViewModel.IdPlanCadre = planCadres.First().IdPlanCadre;

            var activiteApprentissages = from activite in _db.ActiviteApprentissage
                where activite.IdPlanCadreElement == idPlanCadreElement
                select activite;

            foreach (var activite in activiteApprentissages)
            {
                var activiteSousActivite = new ActiviteSousActivite();

                var sousActivites = from sousActivite in _db.SousActiviteApprentissage
                    join activiteApprentissage in _db.ActiviteApprentissage on sousActivite.IdActivite equals activite.IdActivite
                    where activiteApprentissage.IdActivite == activite.IdActivite
                    select sousActivite;

                activiteSousActivite.Activite = activite;

                activiteSousActivite.SousActivites =
                    sousActivites.Any() ? sousActivites.ToList() : new List<SousActiviteApprentissage>();

                activiteViewModel.ActiviteSousActivites.Add(activiteSousActivite);
            }

            return View(activiteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreationModification(
            [Bind(Include = "IdPlanCadre,IdPlanCadreElement,ActiviteSousActivites")]
            ActiviteViewModel activiteViewModel)
        {
            if (!ModelState.IsValid) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (activiteViewModel.ActiviteSousActivites == null)
            {
                _db.SaveChanges();
                return RedirectToAction("Structure", "PlanCadre", new {activiteViewModel.IdPlanCadre});
            }

            foreach (var activite in activiteViewModel.ActiviteSousActivites)
            {
                if (activite.Activite.DescActivite.IsNullOrWhiteSpace())
                {
                    if (activite.Activite.IdActivite != 0)
                    {
                        var dbActivite = _db.ActiviteApprentissage.Find(activite.Activite.IdActivite);
                        if(dbActivite != null) _db.ActiviteApprentissage.Remove(dbActivite);
                    }

                    continue;
                }

                activite.Activite.IdPlanCadreElement = activiteViewModel.IdPlanCadreElement;
                _db.ActiviteApprentissage.AddOrUpdate(activite.Activite);
                _db.SaveChanges();

                if (activite.SousActivites == null) continue;
                foreach (var sousActivite in activite.SousActivites)
                {
                    if (sousActivite.NomSousActivite.IsNullOrWhiteSpace())
                    {
                        if (sousActivite.IdSousActivite != 0)
                        {
                            var dbSousActivite = _db.SousActiviteApprentissage.Find(sousActivite.IdSousActivite);
                            if(dbSousActivite != null) _db.SousActiviteApprentissage.Remove(dbSousActivite);
                        }

                        continue;
                    }

                    sousActivite.IdActivite = activite.Activite.IdActivite;
                    _db.SousActiviteApprentissage.AddOrUpdate(sousActivite);
                }
            }

            _db.SaveChanges();

            return RedirectToAction("Structure", "PlanCadre", new {activiteViewModel.IdPlanCadre});
        }
    }
}
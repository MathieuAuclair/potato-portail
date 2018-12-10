using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PotatoPortail.Models;
using Microsoft.Ajax.Utilities;
using PotatoPortail.Migrations;
using PotatoPortail.ViewModels.PlanCadre;


namespace PotatoPortail.Controllers
{
    public class EnvironnementPhysiqueController : Controller
    {
        private BdPortail _db = new BdPortail();

        public ActionResult CreationModification(int? idPlanCadre)
        {
            if (idPlanCadre == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EnvironnementPhysiqueViewModel environnementViewModel = new EnvironnementPhysiqueViewModel();
            environnementViewModel.EnvironnementSousEnvironnements = new List<EnvironnementSousEnvironnement>();
            environnementViewModel.IdPlanCadre = (int)idPlanCadre;

            var planCadres = _db.PlanCadre.Find(idPlanCadre);

            environnementViewModel.IdPlanCadre = planCadres.IdPlanCadre;

            var environnementPhysiques = from environnement in _db.EnvironnementPhysique
                                         where environnement.IdPlanCadre == idPlanCadre
                                         select environnement;


            foreach (var environnement in environnementPhysiques)
            {
                var environnementSousEnvironnement = new EnvironnementSousEnvironnement();

                var sousEnvironnementsPhysique = from sousEnvironnement in _db.SousEnvironnementPhysique
                                     join EnvironnementPhysique in _db.EnvironnementPhysique on sousEnvironnement.IdEnvPhysique equals EnvironnementPhysique.IdEnvPhysique
                                     where EnvironnementPhysique.IdEnvPhysique == environnement.IdEnvPhysique
                                     select sousEnvironnement;

                environnementSousEnvironnement.EnvironnementPhysique = environnement;

                environnementSousEnvironnement.SousEnvironnementPhysiques =
                    sousEnvironnementsPhysique.Any() ? sousEnvironnementsPhysique.ToList() : new List<SousEnvironnementPhysique>();

                environnementViewModel.EnvironnementSousEnvironnements.Add(environnementSousEnvironnement);
            }

            return View(environnementViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreationModification(EnvironnementPhysiqueViewModel environnementPhysiqueViewModel)
        {
            if (!ModelState.IsValid) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (environnementPhysiqueViewModel.EnvironnementSousEnvironnements == null)
            {
                return RedirectToAction("Structure", "PlanCadre", new { environnementPhysiqueViewModel.IdPlanCadre });
            }

            foreach (var environnement in environnementPhysiqueViewModel.EnvironnementSousEnvironnements)
            {
                if (environnement.EnvironnementPhysique.NomEnvPhys.IsNullOrWhiteSpace())
                {
                    if (environnement.EnvironnementPhysique.IdEnvPhysique != 0)
                    {
                        var dbEnvironnement = _db.EnvironnementPhysique.Find(environnement.EnvironnementPhysique.IdEnvPhysique);
                        if (dbEnvironnement != null) _db.EnvironnementPhysique.Remove(dbEnvironnement);
                    }

                    continue;
                }

                environnement.EnvironnementPhysique.IdPlanCadre = environnementPhysiqueViewModel.IdPlanCadre;
                _db.EnvironnementPhysique.AddOrUpdate(environnement.EnvironnementPhysique);
                _db.SaveChanges();

                if (environnement.SousEnvironnementPhysiques == null) continue;
                foreach (var sousEnvironnement in environnement.SousEnvironnementPhysiques)
                {
                    if (sousEnvironnement.NomSousEnvPhys.IsNullOrWhiteSpace())
                    {
                        if (sousEnvironnement.IdSousEnvPhys != 0)
                        {
                            var dbSousEnvironnement = _db.SousEnvironnementPhysique.Find(sousEnvironnement.IdSousEnvPhys);
                            if (dbSousEnvironnement != null) _db.SousEnvironnementPhysique.Remove(dbSousEnvironnement);
                        }

                        continue;
                    }

                    sousEnvironnement.IdEnvPhysique = environnement.EnvironnementPhysique.IdEnvPhysique;
                    _db.SousEnvironnementPhysique.AddOrUpdate(sousEnvironnement);
                }
            }

            _db.SaveChanges();

            return RedirectToAction("Structure", "PlanCadre", new { environnementPhysiqueViewModel.IdPlanCadre });
        }
    }

}
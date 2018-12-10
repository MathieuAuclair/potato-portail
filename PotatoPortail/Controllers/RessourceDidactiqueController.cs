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
    public class RessourceDidactiqueController:Controller
    {
        private BdPortail _db = new BdPortail();

        public ActionResult CreationModification(int? idPlanCadre)
        {
            if (idPlanCadre == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DidactiqueViewModel didactiqueViewModel = new DidactiqueViewModel();
            didactiqueViewModel.DidactiqueSousDidactiques = new List<DidactiqueSousDidactique>();
            didactiqueViewModel.IdPlanCadre = (int)idPlanCadre;

            var planCadres = _db.PlanCadre.Find(idPlanCadre);

            didactiqueViewModel.IdPlanCadre = planCadres.IdPlanCadre;

            var ressourcesDidactiques = from ressource in _db.RessourceDidactique
                                         where ressource.IdPlanCadre == idPlanCadre
                                         select ressource;

            foreach (var ressource in ressourcesDidactiques)
            {
                var didactiqueSousDidactique = new DidactiqueSousDidactique();

                var sousDidactique = from sousRessource in _db.SousRessourceDidactique
                                    join RessourceDidactique in _db.RessourceDidactique on sousRessource.IdRessource equals RessourceDidactique.IdRessource
                                    where RessourceDidactique.IdRessource == ressource.IdRessource
                                    select sousRessource;

                didactiqueSousDidactique.ressourceDidactique = ressource;

                didactiqueSousDidactique.sousRessourceDidactiques =
                    sousDidactique.Any() ? sousDidactique.ToList() : new List<SousRessourceDidactique>();

                didactiqueViewModel.DidactiqueSousDidactiques.Add(didactiqueSousDidactique);
            }

            return View(didactiqueViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreationModification(DidactiqueViewModel didactiqueViewModel)
        {
            if (!ModelState.IsValid) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (didactiqueViewModel.DidactiqueSousDidactiques == null)
            {
                return RedirectToAction("Structure", "PlanCadre", new { didactiqueViewModel.IdPlanCadre });
            }

            foreach (var didactique in didactiqueViewModel.DidactiqueSousDidactiques)
            {
                if (didactique.ressourceDidactique.NomRessource.IsNullOrWhiteSpace())
                {
                    if (didactique.ressourceDidactique.IdRessource != 0)
                    {
                        var dbDidactique = _db.RessourceDidactique.Find(didactique.ressourceDidactique.IdRessource);
                        if (dbDidactique != null) _db.RessourceDidactique.Remove(dbDidactique);
                    }

                    continue;
                }

                didactique.ressourceDidactique.IdPlanCadre = didactiqueViewModel.IdPlanCadre;
                _db.RessourceDidactique.AddOrUpdate(didactique.ressourceDidactique);
                _db.SaveChanges();

                if (didactique.sousRessourceDidactiques == null) continue;
                foreach (var sousDidactique in didactique.sousRessourceDidactiques)
                {
                    if (sousDidactique.NomSousRessource.IsNullOrWhiteSpace())
                    {
                        if (sousDidactique.IdSousRessource != 0)
                        {
                            var dbSousDidactique = _db.SousRessourceDidactique.Find(sousDidactique.IdSousRessource);
                            if (dbSousDidactique != null) _db.SousRessourceDidactique.Remove(dbSousDidactique);
                        }

                        continue;
                    }

                    sousDidactique.IdRessource = didactique.ressourceDidactique.IdRessource;
                    _db.SousRessourceDidactique.AddOrUpdate(sousDidactique);
                }
            }

            _db.SaveChanges();

            return RedirectToAction("Structure", "PlanCadre", new { didactiqueViewModel.IdPlanCadre });
        }
    }
}
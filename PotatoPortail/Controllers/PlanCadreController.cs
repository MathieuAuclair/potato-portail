using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ApplicationPlanCadre.ViewModels;
using PotatoPortail.Helpers;
using PotatoPortail.Models;

/* !!!ATTENTION!!! aucune idée qui à conçus cette fameuse classe, mais pour le bien de votre CV je la nettoyerais, les nom significatifs c'est pas une blague bordel, définitivement une classe à repasser en vue! */

namespace PotatoPortail.Controllers
{
    [RCPPlanCadreAuthorize]
    public class PlanCadreController : Controller
    {
        private List<SelectListItem> _elements;
        private CompetenceViewModel _competenceViewModel;
        private readonly BdPortail _db = new BdPortail();

        public ActionResult Index()
        {
            return View(_db.PlanCadre.ToList());
        }

        public ActionResult Info(int? idPlanCadre)
        {
            if (idPlanCadre == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PlanCadre planCadre = _db.PlanCadre.Find(idPlanCadre);
            if (planCadre == null)
            {
                return HttpNotFound();
            }


            return View(planCadre);
        }
  
        public ActionResult InfoFocus(int? idPlanCadre, string idRecherche)
        {


            if (idPlanCadre == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (idRecherche != null)
            {
                ViewBag.idRecherche = idRecherche;
            }
            var planCadre = _db.PlanCadre.Find(idPlanCadre);
            if (planCadre == null)
            {
                return HttpNotFound();
            }


            return View("Info",planCadre);
        }
        
        public ActionResult Choix()
        {
            _competenceViewModel = new CompetenceViewModel();
            var typePlan = _db.TypePlanCadre.ToList();
            _competenceViewModel.TypePlanCadre = new SelectList(typePlan, "idType", "nom");
            _elements = new List<SelectListItem>();
            _competenceViewModel.EnonceCompetence = _db.EnonceCompetence.ToList();
            _competenceViewModel.ElementCompetence = _db.ElementCompetence.ToList();
            _elements = new List<SelectListItem>();
            var enonces = _db.EnonceCompetence.Select(item => new SelectListItem() {Value = item.IdCompetence.ToString(), Text = (item.CodeCompetence + " : " + item.Description)}).ToList();
            var elementEnonce = from element in _db.ElementCompetence
                                    join enonce in _db.EnonceCompetence on element.IdCompetence equals enonce.IdCompetence
                                    where enonce.IdCompetence == element.IdCompetence
                                    select new { ID = element.IdElement, Numero = element.Numero, Desc = element.Description };

                foreach (var element in elementEnonce)
                {
                _elements.Add(new SelectListItem()
                    {
                        Value = element.ID.ToString(),
                        Text = (element.Numero + " : " + element.Desc)
                    });

                }
            
            _competenceViewModel.EnonceCompetences = enonces;
            _competenceViewModel.ElementCompetences = _elements;
            return View(_competenceViewModel);
        }

        public PartialViewResult GetCompetence(CompetenceViewModel competenceViewModel, int id)
        {
            _elements = new List<SelectListItem>();
            competenceViewModel.EnonceCompetence = _db.EnonceCompetence.ToList();
            competenceViewModel.ElementCompetence = _db.ElementCompetence.ToList();
            List<SelectListItem> enonces = new List<SelectListItem>();
            
            foreach (var item in _db.EnonceCompetence)
            {
                enonces.Add(new SelectListItem()
                {
                    Value = item.IdCompetence.ToString(),
                    Text = (item.CodeCompetence + " : " + item.Description)
                });
            }
                var elementEnonce = from element in _db.ElementCompetence
                                    join enonceCompetence in _db.EnonceCompetence on element.IdCompetence equals enonceCompetence.IdCompetence
                                    where enonceCompetence.IdCompetence == id
                                    select new { ID = element.IdElement, Numero = element.Numero, Desc = element.Description };

                foreach (var ele in elementEnonce)
                {
                _elements.Add(new SelectListItem()
                    {
                        Value = ele.ID.ToString(),
                        Text = (ele.Numero + " : " + ele.Desc)
                    });

                }
            competenceViewModel.EnonceCompetences = enonces;
            competenceViewModel.ElementCompetences = _elements;
            return PartialView("GetCompetence", competenceViewModel);
        }

        //normalement, ici, on recevrait la liste des éléments de compétence sélectionnés, mais là on va tous les chercher
        [HttpPost]
        public ActionResult Choix(CompetenceViewModel competenceViewModel, string[] listeElement)
        {
            var listeElementCompetence = new List<ElementCompetence>();
            var idEnonce = Convert.ToInt32(competenceViewModel.IdEnonce);

            var elementEnonce = from element in _db.ElementCompetence
                                join enonce in _db.EnonceCompetence on element.IdCompetence equals enonce.IdCompetence
                                where enonce.IdCompetence == idEnonce
                                select new { ID = element.IdElement, Numero = element.Numero, Desc = element.Description };
            
            foreach (var element in elementEnonce)
            {
                listeElementCompetence.Add(new ElementCompetence()
                {
                    IdElement = element.ID,
                    IdCompetence = idEnonce,
                    Description = element.Desc,
                    Numero = element.Numero,
                });
            }

            var planCadre = new PlanCadre
            {
                NumeroCours = competenceViewModel.NumeroCours,
                TitreCours = competenceViewModel.TitreCours,
                IndicationPedago = competenceViewModel.IndicationPedago,
                NbHeureDevoir = competenceViewModel.NbHeureDevoir,
                NbHeurePratique = competenceViewModel.NbHeurePratique,
                NbHeureTheorie = competenceViewModel.NbHeureTheorie,
                IdType = competenceViewModel.IdType,
                IdProgramme = 1,
            };
            _db.PlanCadre.Add(planCadre);
            _db.SaveChanges();
            int planCadreId = planCadre.IdPlanCadre;
            competenceViewModel.IdPlanCadre = planCadreId;

            PlanCadreCompetence planCadreCompetence = new PlanCadreCompetence
            {
                IdPlanCadre = planCadreId,
                IdCompetence = idEnonce,
                PonderationEnHeure = 60,
            };
            _db.PlanCadreCompetence.Add(planCadreCompetence);
            _db.SaveChanges();
            var pccId = planCadreCompetence.IdPlanCadreCompetence;
            foreach (var item in listeElementCompetence)
            {
                var planCadreElement = new PlanCadreElement
                {
                    IdPlanCadreCompetence = pccId,
                    IdElement = item.IdElement,
                };
                _db.PlanCadreElement.Add(planCadreElement);
                _db.SaveChanges();
            }
            return RedirectToAction("index", competenceViewModel);
        }
    }
}

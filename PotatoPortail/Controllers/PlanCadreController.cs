using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using PotatoPortail.Models;
using PotatoPortail.Helpers;
using PotatoPortail.ViewModels;
using PotatoPortail.ViewModels.PlanCadre;
using Newtonsoft.Json;
using PotatoPortail.Migrations;

namespace PotatoPortail.Controllers
{
    [RcpPlanCadreAuthorize]
    public class PlanCadreController : Controller
    {
        List<SelectListItem> elements;
        CompetenceViewModel competenceViewModel;
        private BdPortail db = new BdPortail();

        public ActionResult Info(int? idPlanCadre)
        {
            if (idPlanCadre == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PlanCadre planCadre = db.PlanCadre.Find(idPlanCadre);
            if (planCadre == null)
            {
                return HttpNotFound();
            }

            return View(planCadre);
        }

        public ActionResult InfoFocus(int? idPlanCadre, string idRecherche)
        {
            if (idPlanCadre == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (idRecherche != null) ViewBag.idRecherche = idRecherche;

            var planCadre = db.PlanCadre.Find(idPlanCadre);

            if (planCadre == null)
            {
                return HttpNotFound();
            }

            return View("Info", planCadre);
        }

        [HttpGet]
        public ActionResult Creation(int? idProgramme, int? idPlanCadre)
        {
            var planCadre = new PlanCadre();

            if (idProgramme == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (idPlanCadre != null) planCadre = db.PlanCadre.Find(idPlanCadre);


            var typePlanCadres = db.TypePlanCadre.ToList();
            var listTypes = new List<SelectListItem>();

            foreach (var type in typePlanCadres)
            {
                listTypes.Add(new SelectListItem
                {
                    Text = type.Nom,
                    Value = type.IdType.ToString()
                });
            }

            ViewBag.Types = listTypes;

            return planCadre != null ? View(planCadre) : View();
        }

        [HttpPost]
        public ActionResult Creation(PlanCadre planCadre)
        {
            if (!ModelState.IsValid) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var planCadreTitre = from bdPlanCadre in db.PlanCadre
                where bdPlanCadre.TitreCours.Equals(planCadre.TitreCours, StringComparison.OrdinalIgnoreCase) &&
                      bdPlanCadre.IdPlanCadre != planCadre.IdPlanCadre
                select bdPlanCadre;

            if (planCadreTitre.Any())
            {
                this.AddToastMessage("Titre déjà existant.",
                    planCadreTitre.First().TitreCours + " est déjà entré dans le système.", Toast.ToastType.Error,
                    true);
                ViewBag.Types = new SelectList(db.TypePlanCadre, "idType", "nom", planCadre.IdType);
                return View(planCadre);
            }

            db.PlanCadre.AddOrUpdate(planCadre);
            db.SaveChanges();
            this.AddToastMessage("Ajout de plan cadre effectué.", "« " + planCadre.TitreCours + " » a été ajouté",
                Toast.ToastType.Success);

            return RedirectToAction("Choix", new {planCadre.IdPlanCadre});
        }

        [HttpGet]
        public ActionResult Choix(int? idPlanCadre)
        {
            if (idPlanCadre == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            competenceViewModel = new CompetenceViewModel
            {
                EnonceCompetence = db.EnonceCompetence.ToList(), ElementCompetence = db.ElementCompetence.ToList()
            };
            var enonces = db.EnonceCompetence.Select(item => new SelectListItem()
                    {Value = item.IdCompetence.ToString(), Text = (item.CodeCompetence + " : " + item.Description)})
                .ToList();

            competenceViewModel.PlanCadre = db.PlanCadre.Find(idPlanCadre);
            ViewBag.IdPlanCadre = idPlanCadre;
            competenceViewModel.EnonceCompetences = enonces;
            return View(competenceViewModel);
        }

        public PartialViewResult GetElement(CompetenceViewModel competenceViewModel, int idCompetence)
        {
            competenceViewModel.ElementCompetence = db.ElementCompetence.ToList();
            var elements = new List<SelectListItem>();

            var elementEnonce = from element in db.ElementCompetence
                join Enonc in db.EnonceCompetence on element.IdCompetence equals Enonc.IdCompetence
                where Enonc.IdCompetence == idCompetence
                orderby element.Numero
                select new
                {
                    ID = element.IdElement,
                    element.Numero,
                    Desc = element.Description
                };

            foreach (var element in elementEnonce)
            {
                elements.Add(new SelectListItem()
                {
                    Value = element.ID.ToString(),
                    Text = (element.Numero + " : " + element.Desc)
                });
            }

            ViewBag.IdCompetence = idCompetence;
            competenceViewModel.ElementCompetences = elements;
            return PartialView("GetElement", competenceViewModel);
        }


        [HttpPost]
        [WebMethod]
        public ActionResult Choix(string httpBundle, int idPlanCadre)
        {
            var listPlanCadreEnonceElement =
                JsonConvert.DeserializeObject<List<PlanCadreCompetenceElement>>(httpBundle);
            foreach (var planCadreEnonceElement in listPlanCadreEnonceElement)
            {
                var planCadreCompetence = new PlanCadreCompetence
                {
                    IdCompetence = planCadreEnonceElement.IdEnonce,
                    IdPlanCadre = idPlanCadre,
                    //PonderationEnHeure = planCadreEnonceElement.Ponderation
                };
                db.PlanCadreCompetence.Add(planCadreCompetence);
                db.SaveChanges();

                foreach (var element in planCadreEnonceElement.IdElements)
                {
                    var planCadreElement = new PlanCadreElement
                    {
                        IdPlanCadreCompetence = planCadreCompetence.IdPlanCadreCompetence,
                        IdElement = element
                    };
                    db.PlanCadreElement.Add(planCadreElement);
                    db.SaveChanges();
                }
            }

            // ReSharper disable once RedundantAnonymousTypePropertyName
            return Json(Url.Action("Structure", "PlanCadre", new {idPlanCadre = idPlanCadre}));
        }

        public ActionResult Structure(int? idPlanCadre)
        {
            if (idPlanCadre == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var structureViewModel = new StructureViewModel();
            var planCadre = db.PlanCadre.Find(idPlanCadre);
            structureViewModel.PlanCadre = planCadre;
            structureViewModel.ElementEnoncePlanCadres = new List<ElementEnoncePlanCadre>();

            var enonces = from enonce in db.EnonceCompetence
                join planCadreCompetence in db.PlanCadreCompetence on enonce.IdCompetence equals planCadreCompetence
                    .IdCompetence
                where planCadreCompetence.IdPlanCadre == planCadre.IdPlanCadre
                select enonce;

            foreach (var enonce in enonces)
            {
                var elements =
                    from element in db.ElementCompetence
                    join planCadreElement in db.PlanCadreElement
                        on element.IdElement equals planCadreElement.IdElement
                    join planCadreCompetence in db.PlanCadreCompetence
                        on planCadreElement.IdPlanCadreCompetence equals planCadreCompetence.IdPlanCadreCompetence
                    where planCadreCompetence.IdCompetence == enonce.IdCompetence &&
                          planCadreCompetence.IdPlanCadre == idPlanCadre
                    select element;
                IEnumerable<ElementCompetence> elementCompetences = elements;

                var idPlanCadreCompetences = from planCadreCompetence in db.PlanCadreCompetence
                    where planCadreCompetence.IdPlanCadre == planCadre.IdPlanCadre &&
                          planCadreCompetence.IdCompetence == enonce.IdCompetence
                    select planCadreCompetence.IdPlanCadreCompetence;

                var idPlanCadreCompetence = idPlanCadreCompetences.First();

                structureViewModel.ElementEnoncePlanCadres.Add(new ElementEnoncePlanCadre
                {
                    EnonceCompetence = enonce,
                    ElementCompetences = elementCompetences,
                    IdPlanCadreCompetence = idPlanCadreCompetence
                });
            }

            return View(structureViewModel);
        }
    }
}
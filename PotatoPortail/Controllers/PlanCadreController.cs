using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApplicationPlanCadre.Models;
using ApplicationPlanCadre.Helpers;
using ApplicationPlanCadre.ViewModels;

namespace ApplicationPlanCadre.Controllers
{
    [RCPPlanCadreAuthorize]
    public class PlanCadreController : Controller
    {
        List<SelectListItem> elements;
        CompetenceViewModel CVM;
        private BDPlanCadre db = new BDPlanCadre();

        public ActionResult Index()
        {
            return View(db.PlanCadre.ToList());
        }

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


            if (idPlanCadre == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (idRecherche != null)
            {
                ViewBag.idRecherche = idRecherche;
            }
            PlanCadre planCadre = db.PlanCadre.Find(idPlanCadre);
            if (planCadre == null)
            {
                return HttpNotFound();
            }


            return View("Info",planCadre);
        }
        
        // GET: Competence
        public ActionResult Choix()
        {
            CVM = new CompetenceViewModel();
            var typePlan = db.TypePlanCadre.ToList();
            CVM.typePlanCadre = new SelectList(typePlan, "idType", "nom");
            elements = new List<SelectListItem>();
            CVM.EnonceCompetence = db.EnonceCompetence.ToList();
            CVM.ElementCompetence = db.ElementCompetence.ToList();
            List<SelectListItem> enonces = new List<SelectListItem>();
            elements = new List<SelectListItem>();
            foreach (var item in db.EnonceCompetence)
            {
                enonces.Add(new SelectListItem()
                {
                    Value = item.idCompetence.ToString(),
                    Text = (item.codeCompetence + " : " + item.description)
                });
            }
                var elementEnonce = from element in db.ElementCompetence
                                    join Enonce in db.EnonceCompetence on element.idCompetence equals Enonce.idCompetence
                                    where Enonce.idCompetence == element.idCompetence
                                    select new { ID = element.idElement, Numero = element.numero, Desc = element.description };

                foreach (var element in elementEnonce)
                {
                elements.Add(new SelectListItem()
                    {
                        Value = element.ID.ToString(),
                        Text = (element.Numero + " : " + element.Desc)
                    });

                }
            
            CVM.EnonceCompetences = enonces;
            CVM.ElementCompetences = elements;
            return View(CVM);
        }

        public PartialViewResult GetCompetence(CompetenceViewModel CVM, int id)
        {
            elements = new List<SelectListItem>();
            CVM.EnonceCompetence = db.EnonceCompetence.ToList();
            CVM.ElementCompetence = db.ElementCompetence.ToList();
            List<SelectListItem> enonces = new List<SelectListItem>();
            
            foreach (var item in db.EnonceCompetence)
            {
                enonces.Add(new SelectListItem()
                {
                    Value = item.idCompetence.ToString(),
                    Text = (item.codeCompetence + " : " + item.description)
                });
            }
                var elementEnonce = from element in db.ElementCompetence
                                    join Enonc in db.EnonceCompetence on element.idCompetence equals Enonc.idCompetence
                                    where Enonc.idCompetence == id
                                    select new { ID = element.idElement, Numero = element.numero, Desc = element.description };

                foreach (var ele in elementEnonce)
                {
                elements.Add(new SelectListItem()
                    {
                        Value = ele.ID.ToString(),
                        Text = (ele.Numero + " : " + ele.Desc)
                    });

                }
            CVM.EnonceCompetences = enonces;
            CVM.ElementCompetences = elements;
            return PartialView("GetCompetence", CVM);
        }
        //normalement, ici, on recevrait la liste des éléments de compétence sélectionnés, mais là on va tous les chercher
        [HttpPost]
        public ActionResult Choix(CompetenceViewModel CVM, string[] listeElement)
        {
            List<ElementCompetence> ListeElement = new List<ElementCompetence>();
            int idEnonce = Convert.ToInt32(CVM.idEnonce);
            IEnumerable<EnonceCompetence> enonceComp = from enonce in db.EnonceCompetence
                                                       where enonce.idCompetence == idEnonce
                                                       select new EnonceCompetence
                                                       {
                                                           codeCompetence = enonce.codeCompetence,
                                                           description = enonce.description,
                                                           specifique = enonce.specifique,
                                                           actif = enonce.actif,
                                                           idCompetence = enonce.idCompetence,
                                                           idDevis = enonce.idDevis,
                                                           obligatoire = enonce.obligatoire,
                                                       };
            List<EnonceCompetence> competence = enonceComp as List<EnonceCompetence>;
            var elementEnonce = from element in db.ElementCompetence
                                join Enonce in db.EnonceCompetence on element.idCompetence equals Enonce.idCompetence
                                where Enonce.idCompetence == idEnonce
                                select new { ID = element.idElement, Numero = element.numero, Desc = element.description };
            
            foreach (var element in elementEnonce)
            {
                ListeElement.Add(new ElementCompetence()
                {
                    idElement = element.ID,
                    idCompetence = idEnonce,
                    description = element.Desc,
                    numero = element.Numero,
                });
            }
            var elementComp = CVM.ElementCompetences;
            PlanCadre PC = new PlanCadre
            {
                numeroCours = CVM.numeroCours,
                titreCours = CVM.titreCours,
                indicationPedago = CVM.indicationPedago,
                nbHeureDevoir = CVM.nbHeureDevoir,
                nbHeurePratique = CVM.nbHeurePratique,
                nbHeureTheorie = CVM.nbHeureTheorie,
                idType = CVM.idType,
                idProgramme = 1,
            };
            db.PlanCadre.Add(PC);
            db.SaveChanges();
            int planCadreId = PC.idPlanCadre;
            CVM.idPlanCadre = planCadreId;
            //var idPlan = from plan in db.PlanCadre
            PlanCadreCompetence PCC = new PlanCadreCompetence
            {
                idPlanCadre = planCadreId,
                idCompetence = idEnonce,
                ponderationEnHeure = 60,
            };
            db.PlanCadreCompetence.Add(PCC);
            db.SaveChanges();
            int pccId = PCC.idPlanCadreCompetence;
            foreach (var item in ListeElement)
            {
                var ele = from elem in db.ElementCompetence
                              where elem.idElement == item.idElement
                              select elem.idElement;
                PlanCadreElement PCE = new PlanCadreElement
                {
                    idPlanCadreCompetence = pccId,
                    idElement = item.idElement,
                };
                db.PlanCadreElement.Add(PCE);
                db.SaveChanges();
            }
            return RedirectToAction("index", CVM);
        }
    }
}

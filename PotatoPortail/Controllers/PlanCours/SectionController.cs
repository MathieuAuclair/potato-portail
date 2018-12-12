using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models;
using PotatoPortail.Models.Plan_Cours;
using PotatoPortail.ViewModels.PlanCours;

namespace PotatoPortail.Controllers.PlanCours
{
    public class SectionController : Controller
    {
        private readonly BdPortail _db = new BdPortail();
        private readonly ServicesAdaptesViewModel _servicesAdaptesViewModel = new ServicesAdaptesViewModel();
        
        public ActionResult Index()
        {
            return RedirectToAction("Index", "PlanCours");
        }

        public ActionResult Create(int? idPlancours)
        {
            string utilisateurDiscipline = ChercherUtilisateur();
            var discipline = from disciplineDepartement in _db.Departement
                             where disciplineDepartement.Discipline == utilisateurDiscipline
                             select disciplineDepartement.Nom;

            string departement = utilisateurDiscipline + " - "+ discipline.First();
            //On teste si l'id passé est correct
            if (!idPlancours.HasValue)
                return RedirectToAction("Index");


            Models.Plan_Cours.PlanCours planCours = _db.PlanCours.Find(idPlancours);
            if (planCours == null)
            {
                return HttpNotFound();
            }
            _servicesAdaptesViewModel.IdPlanCours = (int)idPlancours;
            _servicesAdaptesViewModel.PlanCours = planCours;
            var listeSection = from texteSection in _db.NomSection
                               join contenuSection in _db.ContenuSection on texteSection.IdNomSection equals contenuSection.IdNomSection
                               where contenuSection.Modifiable
                               select texteSection;
            var listeNom = new List<SelectListItem>();
            foreach (var nom in listeSection)
            {
                listeNom.Add(new SelectListItem
                {
                    Text = nom.TitreSection,
                    Value = nom.IdNomSection.ToString()
                });
            }
            
            _servicesAdaptesViewModel.NomSections = listeSection;
            ViewBag.nomSections = listeNom;
            ViewBag.departement = departement;
            _servicesAdaptesViewModel.IdPlanCours = (int)idPlancours;
            ViewBag.idPlanCours = idPlancours;
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(ServicesAdaptesViewModel servicesAdaptesViewModel, FormCollection collection)
        {
            string discipline = ChercherUtilisateur();

            if (discipline == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                var planCoursDepart = new PlanCoursDepart();
                var texte = servicesAdaptesViewModel.TexteContenu;
                
                var nomSection = servicesAdaptesViewModel.TitreSection;
                var idPlanCours = servicesAdaptesViewModel.IdPlanCours;

                planCoursDepart.Discipline = _db.Departement.Find(discipline)?.Discipline;
                var idNomSection = _db.NomSection.Find(Convert.ToInt32(nomSection))?.IdNomSection;
                var planCours = _db.PlanCours.Find(Convert.ToInt32(idPlanCours))?.IdPlanCours;

                if (planCours == null || idNomSection == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                planCoursDepart.IdNomSection = (int)idNomSection;
                planCoursDepart.IdPlanCours = (int)planCours;

                planCoursDepart.TexteContenu = texte;
                _db.PlanCoursDepart.Add(planCoursDepart);
                _db.SaveChanges();
                return RedirectToAction("Index","Apercu");
            }
            catch
            {
                return RedirectToAction("Index", "Apercu");
            }
        }

        public string ChercherUtilisateur()
        {
            var courrielConnexion = User.Identity.Name;
            var requete = from acces in _db.AccesProgramme
                          where acces.UserMail == courrielConnexion
                          select acces.Discipline;
            return requete.First();
        }




    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApplicationPlanCadre.Models;
using ApplicationPlanCadre.ViewModels;
using ApplicationPlanCadre.App_Code;

namespace ApplicationPlanCadre.Controllers
{
    public class SectionController : Controller
    {
        private BDPlanCadre db = new BDPlanCadre();
        private servicesAdaptesViewModel servicesAdaptesViewModel = new servicesAdaptesViewModel();
        
        // GET: Section
        public ActionResult Index()
        {
            return RedirectToAction("Index", "PlanCours");
        }

        public ActionResult Create(int? idPlancours)
        {
            string utilisateurDiscipline = chercherUtilisateur();
            var discipline = from disciplineDepartement in db.Departement
                             where disciplineDepartement.discipline == utilisateurDiscipline
                             select disciplineDepartement.nom;

            string departement = utilisateurDiscipline + " - "+ discipline.First();
            //On teste si l'id passé est correct
            if (!idPlancours.HasValue)
                return RedirectToAction("Index");


            PlanCours planCours = db.PlanCours.Find(idPlancours);
            if (planCours == null)
            {
                return HttpNotFound();
            }
            servicesAdaptesViewModel.idPlanCours = (int)idPlancours;
            servicesAdaptesViewModel.planCours = planCours;
            var listeSection = from texteSection in db.NomSection
                               join contenuSection in db.ContenuSection on texteSection.idNomSection equals contenuSection.idNomSection
                               where contenuSection.modifiable == true
                               select texteSection;
            List < SelectListItem > listeNom = new List<SelectListItem>();
            foreach (var nom in listeSection)
            {
                listeNom.Add(new SelectListItem
                {
                    Text = nom.titreSection,
                    Value = nom.idNomSection.ToString()
                });
            }
            
            servicesAdaptesViewModel.NomSections = listeSection;
            ViewBag.nomSections = listeNom;
            ViewBag.departement = departement;
            ViewBag.idPlanCours = idPlancours;
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            string discipline = chercherUtilisateur();
            try
            {
                PlanCoursDepart PCD = new PlanCoursDepart();
                string texte = Request.Form["texteContenu"];
                
                int nomSection = Convert.ToInt32(Request.Form["titreSection"]);
                int idPlanCours = Convert.ToInt32(Request.Form["idPlanCours"]);
                PCD.discipline = db.Departement.Find(discipline).discipline;
                PCD.idNomSection = db.NomSection.Find(nomSection).idNomSection;
                PCD.idPlanCours = db.PlanCours.Find(idPlanCours).idPlanCours;
                PCD.texteContenu = texte;
                db.PlanCoursDeparts.Add(PCD);
                db.SaveChanges();
                return RedirectToAction("Index","Apercu");
            }
            catch
            {
                return View();
            }
        }

        public string chercherUtilisateur()
        {
            var CourrielConnexion = User.Identity.Name;
            var requete = from acces in db.AccesProgramme
                          where acces.userMail == CourrielConnexion
                          select acces.discipline;
            return requete.First();
        }




    }
}
using ApplicationPlanCadre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ApplicationPlanCadre.Controllers
{
    public class GrilleCoursEnonceController : Controller
    {
        // GET: GrilleCoursEnonce

        public ActionResult GetEnonce()
        {
            GrilleCoursViewModel GCVM = new GrilleCoursViewModel();
            GCVM.Grille = GetGrilleCoursModel();
            GCVM.Enonces = GetEnonceModel();

            return View(GCVM);
        }

        public GrilleCours GetGrilleCoursModel() {
            GrilleCours maGrille = new GrilleCours()
            {
                idGrille = 1,
                nom = "mon cours",
            };
            return maGrille;
        }


        public List<EnonceCompetence> GetEnonceModel()
        {
            List<EnonceCompetence> mesEnonces = new List<EnonceCompetence>();
            
            mesEnonces.Add(new EnonceCompetence { idCompetence = 1, codeCompetence = "016N", description = "", obligatoire = true, actif = true });
            mesEnonces.Add(new EnonceCompetence { idCompetence = 2, codeCompetence = "016M", description = "", obligatoire = true, actif = true });
            mesEnonces.Add(new EnonceCompetence { idCompetence = 3, codeCompetence = "0160", description = "", obligatoire = true, actif = true });
            return mesEnonces;
        }
    }
}
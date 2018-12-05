using System.Collections.Generic;
using System.Web.Mvc;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers
{
    public class GrilleCoursEnonceController : Controller
    {
        public ActionResult GetEnonce()
        {
            GrilleCoursViewModel grilleCoursViewModel = new GrilleCoursViewModel
            {
                Grille = GetGrilleCoursModel(),
                Enonces = GetEnonceModel()
            };
            return View("/Views/GrilleCoursEnonce/GetEnonceModel.cshtml", grilleCoursViewModel);
        }

        public GrilleCours GetGrilleCoursModel()
        {
            GrilleCours maGrille = new GrilleCours()
            {
                IdGrille = 1,
                Nom = "mon cours",
            };
            return maGrille;
        }

        public List<EnonceCompetence> GetEnonceModel()
        {
            List<EnonceCompetence> mesEnonces = new List<EnonceCompetence>
            {
                new EnonceCompetence
                {
                    IdCompetence = 1,
                    codeCompetence = "016N",
                    description = "",
                    obligatoire = true,
                    actif = true
                },
                new EnonceCompetence
                {
                    IdCompetence = 2,
                    codeCompetence = "016M",
                    description = "",
                    obligatoire = true,
                    actif = true
                },
                new EnonceCompetence
                {
                    IdCompetence = 3,
                    codeCompetence = "0160",
                    description = "",
                    obligatoire = true,
                    actif = true
                }
            };

            return mesEnonces;
        }
    }
}
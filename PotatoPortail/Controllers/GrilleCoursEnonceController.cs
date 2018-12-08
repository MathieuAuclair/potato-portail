using System.Collections.Generic;
using System.Web.Mvc;
using PotatoPortail.Models;
using PotatoPortail.ViewModels.ProjetPrincipal;

namespace PotatoPortail.Controllers
{
    public class GrilleCoursEnonceController : Controller
    {

        public ActionResult GetEnonce()
        {
            var grilleCoursViewModel = new GrilleCoursViewModel
            {
                Grille = GetGrilleCoursModel(),
                Enonces = GetEnonceModel()
            };

            return View(grilleCoursViewModel);
        }

        public GrilleCours GetGrilleCoursModel() {
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
                    CodeCompetence = "016N",
                    Description = "",
                    Obligatoire = true,
                    Actif = true
                },
                new EnonceCompetence
                {
                    IdCompetence = 2,
                    CodeCompetence = "016M",
                    Description = "",
                    Obligatoire = true,
                    Actif = true
                },
                new EnonceCompetence
                {
                    IdCompetence = 3,
                    CodeCompetence = "0160",
                    Description = "",
                    Obligatoire = true,
                    Actif = true
                }
            };


            return mesEnonces;
        }
    }
}
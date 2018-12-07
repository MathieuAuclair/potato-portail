using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PotatoPortail.Migrations;
using PotatoPortail.Models;
using PotatoPortail.Models.Plan_Cours;
using PotatoPortail.ViewModels;

namespace PotatoPortail.Controllers.PlanCours
{
    public class ApercuController : Controller
    {
        ApercuViewModel viewModel;

        readonly BdPortail _db = new BdPortail();

        ApercuPlanCours _apercu = new ApercuPlanCours();
        
        public ActionResult Index(ApercuPlanCours apercu, int? id)
        {
            if (apercu == null) return new HttpNotFoundResult(nameof(apercu));
            viewModel = new ApercuViewModel();
            var courrielConnexion = User.Identity.Name;
            var requete = from acces in _db.AccesProgramme
                          where acces.UserMail == courrielConnexion
                          select acces.Discipline;
            var listePlanCours = new List<Models.PlanCours>();
            foreach (var planCoursId in GetPlanCours())
            {
                listePlanCours.Add(_db.PlanCours.Find(planCoursId));
            }
            foreach (var planCours in listePlanCours)
            {
                var nomSection = from nomS in _db.NomSection
                                 join cs in _db.ContenuSection on nomS.idNomSection equals cs.idNomSection
                                 join ts in _db.TexteSection on cs.idContenuSection equals ts.idContenuSection
                                 join pco in _db.PlanCours on ts.idPlanCours equals pco.idPlanCours
                                 where pco.idPlanCours == planCours.idPlanCours
                                 select nomS;

                viewModel.MainPageViewModel.NomSections = new List<List<NomSection>>
                {
                    new List<NomSection> {nomSection as NomSection}
                };
            }

            if (id == null)
            {
                id = 1;
            }
            var idPlanCadre = from planCours in _db.PlanCours
                              join cours in _db.Cours on planCours.idCours equals cours.IdCours
                              join planCadre in _db.PlanCadre on cours.IdPlanCadre equals planCadre.IdPlanCadre
                              where planCours.idPlanCours == id
                              select planCadre.IdPlanCadre;

            viewModel.MainPageViewModel.PlanCours = listePlanCours;
            viewModel.MainPageViewModel.ContenuSection = _db.ContenuSection.ToList();
            viewModel.MainPageViewModel.NomSection = _db.NomSection.ToList();
            apercu = GetUser(Convert.ToInt32(id));
            List<PlanCoursDepart> pcd = new List<PlanCoursDepart>();
            ViewBag.courrielProf = apercu.CourrielProf;
            ViewBag.imageCegep = VirtualPathUtility.ToAbsolute(apercu.ImageCegep);
            ViewBag.imageDepart = VirtualPathUtility.ToAbsolute(apercu.ImageDepart);
            ViewBag.phrase = apercu.Phrase;
            ViewBag.infosCours = CreationInfoCours(Convert.ToInt32(idPlanCadre.First()));
            ViewBag.infosProf = apercu.InfosProf;
            ViewBag.LocalProf = apercu.LocalProf;
            ViewBag.session = apercu.Session;
            viewModel.TexteContenu = new string[15];
            viewModel.TitreSection = new string[15];
            var listeSection = RetourneSection(requete.First(), Convert.ToInt32(id));
            viewModel.IndexSection = listeSection;
            foreach(var section in listeSection)
            {
                try
                {
                    var texte = CreationSectionDepart(Convert.ToInt32(id), section, requete.First());
                    viewModel.TexteContenu[section] = texte;
                    var titre = CreationTitreSection(Convert.ToInt32(id), section);
                    viewModel.TitreSection[section] = titre;
                }
                catch (Exception)
                {
                    var textecontenu = CreationSectionDefaut(Convert.ToInt32(id), section, requete.First());
                    viewModel.TexteContenu[section] = textecontenu;
                    var titreSection = CreationTitreSection(Convert.ToInt32(id), section);
                    viewModel.TitreSection[section] = titreSection;
                }

            }

                CreationEnonceCompetence(viewModel, (int)id);


            return View(viewModel);
        }
        public ActionResult Create()
        {
            List <PlanCoursDepart> pcd = new List<PlanCoursDepart>();
            viewModel = new ApercuViewModel();
            var liste = _db.Cours.ToList();

            viewModel.PlanCadre = _db.PlanCadre.ToList();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(ApercuViewModel ApercuViewModel, FormCollection collection)
        {
            try {
                string UserId = User.Identity.GetUserId();
                PlanCadre planCadre = _db.PlanCadre.Find(ApercuViewModel.IdPlanCadre);
                var Cours = from cours in _db.Cours
                            where cours.IdPlanCadre == ApercuViewModel.IdPlanCadre
                            select cours;
                int CoursId = Cours.First().IdCours;
                Models.PlanCours PC = new Models.PlanCours()
                {
                    dateCreation = DateTime.Today,
                    dateValidation = null,
                    idCours = CoursId,
                    StatutPlanCours = false,
                };
                _db.PlanCours.Add(PC);
                _db.SaveChanges();
                var idPlanCours = PC.idPlanCours;
                var PCU = new PlanCoursUtilisateur()
                {
                    idPlanCoursUtilisateur = UserId,
                    idPlanCours = idPlanCours,
                    bureauProf = ApercuViewModel.BureauProf,
                    poste = ApercuViewModel.NoPoste,
                };
                _db.PlanCoursUtilisateur.Add(PCU);
                _db.SaveChanges();
                _apercu = GetUser(idPlanCours);
                for (var i = 1; i < 15; i++)
                {
                    var texteSectionDefault = new TexteSection {idPlanCours = idPlanCours, idContenuSection = i};
                    _db.TexteSection.Add(texteSectionDefault);
                    _db.SaveChanges();
                }
                return RedirectToAction("Index", _apercu);
                    }
            catch
            {
                return View();
            }
        }

        public string CreationInfoCours(int id)
        {
            var query = from PC in _db.PlanCadre
                        where PC.IdPlanCadre == id
                        select PC;

            var planCadre = query.First();

            return _apercu.InsertionInfoCours(planCadre.NumeroCours, planCadre.TitreCours, Convert.ToInt32(planCadre.NbHeureTheorie), Convert.ToInt32(planCadre.NbHeurePratique), Convert.ToInt32(planCadre.NbHeureDevoir));
 
        }

        public List<int> RetourneSection(string discipline, int id )
        {
            var section = from nomSection in _db.NomSection
                          join contenuSection in _db.ContenuSection on nomSection.idNomSection equals contenuSection.idNomSection
                          join texteSection in _db.TexteSection on contenuSection.idContenuSection equals texteSection.idContenuSection
                          join planCours in _db.PlanCours on texteSection.idPlanCours equals planCours.idPlanCours
                          where planCours.idPlanCours == id
                          select nomSection.idNomSection;
            return section.ToList();
        }
        public string CreationSectionDepart(int id, int idSection, string discipline)
        {
            var texteSection = from texte in _db.PlanCoursDepart
                                      where texte.discipline == discipline && texte.idNomSection == idSection
                                      select texte.texteContenu;

            return texteSection.First();
        }
        public string CreationSectionDefaut(int id, int idSection, string discipline)
        {
            var texteSection = from texte in _db.ContenuSection
                               join nomSection in _db.NomSection on texte.idNomSection equals nomSection.idNomSection
                               where texte.idNomSection == idSection
                               select texte.texteContenu;

            return texteSection.First();
        }
        public string CreationTitreSection(int id, int idSection)
        {
            var texteSection = from nomSection in _db.NomSection
                               join contenuSection in _db.ContenuSection on nomSection.idNomSection equals contenuSection.idNomSection
                               where nomSection.idNomSection == idSection
                               select nomSection.titreSection;
            return texteSection.First();
        }

        public void CreationEnonceCompetence(ApercuViewModel ApercuViewModel, int id)
        {
            var listeElementCompetence = new List<ElementCompetence>();
            var listeEnonceCompetence = new List<EnonceCompetence>();
            var listeElementConnaissance = new List<ElementConnaissance>();
            var listeActiviteApprentissage = new List<ActiviteApprentissage>();
            var listePlanCadreCompetence = new List<PlanCadreCompetence>();
            var listePlanCadreElement = new List<PlanCadreElement>();
            var ponderationEnHeure = new List<int>();

            var planCours = _db.PlanCours.Find(id);

            if (planCours == null)
            {
                throw new NullReferenceException();
            }

            var cours = _db.Cours.Find(planCours.idCours);

            if (cours == null)
            {
                throw new NullReferenceException();
            }

            var planCadre = _db.PlanCadre.Find(cours.IdPlanCadre);
            
            var enonceCompetence = from enonce in _db.EnonceCompetence
                                    join planCadreCompetence in _db.PlanCadreCompetence on enonce.IdCompetence equals planCadreCompetence.IdCompetence
                                    join _planCadre in _db.PlanCadre on planCadreCompetence.IdPlanCadre equals _planCadre.IdPlanCadre
                                    where true
                                    select enonce;
            
            foreach (var enonce in enonceCompetence)
            {
                var planCadreCompetence = from tabPlanCadreCompetence in _db.PlanCadreCompetence
                                          join tabEnonceCompetence in _db.EnonceCompetence on tabPlanCadreCompetence.IdCompetence equals tabEnonceCompetence.IdCompetence
                                          where tabPlanCadreCompetence.IdCompetence == enonce.IdCompetence
                                          select tabPlanCadreCompetence;

                foreach(var planCadreComp in planCadreCompetence)
                {
                    listePlanCadreCompetence.Add(planCadreComp);
                    var planCadreElement = from tabPlanCadreElement in _db.PlanCadreElement
                                           where tabPlanCadreElement.IdPlanCadreCompetence == planCadreComp.IdPlanCadreCompetence
                                           select tabPlanCadreElement;

                    foreach(var planCadreElements in planCadreElement)
                    {
                        listePlanCadreElement.Add(planCadreElements);
                    }
                }
                var ponderation = (from tabPlanCadreCompetence in _db.PlanCadreCompetence
                                  where tabPlanCadreCompetence.IdCompetence == enonce.IdCompetence
                                  select tabPlanCadreCompetence.PonderationEnHeure);

                foreach(var ponderationHeure in ponderation)
                {
                    ponderationEnHeure.Add(Convert.ToInt32(ponderation.First()));
                }

                listeEnonceCompetence.Add(enonce);

                var _elementCompetence = from tabElement in _db.ElementCompetence
                                        join planCadreElement in _db.PlanCadreElement on tabElement.IdElement equals planCadreElement.IdElement
                                        join PlanCadreCompetence in _db.PlanCadreCompetence on planCadreElement.IdPlanCadreCompetence equals PlanCadreCompetence.IdPlanCadreCompetence
                                        join competence in _db.EnonceCompetence on PlanCadreCompetence.IdCompetence equals competence.IdCompetence
                                        where competence.IdCompetence == tabElement.IdCompetence
                                        select tabElement;

                foreach(var _element in _elementCompetence)
                {
                    listeElementCompetence.Add(_element);
                    var elementConnaissances = from tabConnaissance in _db.ElementConnaissance
                                              join planCadreElement in _db.PlanCadreElement on tabConnaissance.IdPlanCadreElement equals planCadreElement.IdPlanCadreElement
                                              join element in _db.ElementCompetence on planCadreElement.IdElement equals element.IdElement
                                              where true
                                              select tabConnaissance;

                    foreach (var connaissance in elementConnaissances)
                    {
                        listeElementConnaissance.Add(connaissance);
                    }

                    var activiteApprentissages = from tabActivite in _db.ActiviteApprentissage
                                                join planCadreElement in _db.PlanCadreElement on tabActivite.IdPlanCadreElement equals planCadreElement.IdElement
                                                join elementCompetence in _db.ElementCompetence on planCadreElement.IdElement equals elementCompetence.IdElement
                                                where elementCompetence.IdElement == _element.IdElement
                                                select tabActivite;

                    foreach (var activite in activiteApprentissages)
                    {
                        listeActiviteApprentissage.Add(activite);
                    }

                }
            }
            ApercuViewModel.ListeEnonceCompetence = listeEnonceCompetence;
            ApercuViewModel.ListeElementCompetence = listeElementCompetence;
            ApercuViewModel.ListeActiviteApprentissage = listeActiviteApprentissage;
            ApercuViewModel.ListeElementConnaissance = listeElementConnaissance;
            ApercuViewModel.PonderationEnHeure = ponderationEnHeure;
            ApercuViewModel.ListePlanCadreCompetence = listePlanCadreCompetence;
            ApercuViewModel.ListePlanCadreElement = listePlanCadreElement;
        }

        public IQueryable<int> GetPlanCours()
        {
            var userId = User.Identity.GetUserId();
            var planCours = from pcours in _db.PlanCours
                            join pcu in _db.PlanCoursUtilisateur on pcours.idPlanCours equals pcu.idPlanCours
                            where pcu.idPlanCoursUtilisateur == userId
                            select pcours.idPlanCours;
            return planCours;

        }

        public ApercuPlanCours GetUser(int id)
        {
            var apercu = new ApercuPlanCours();
            var query = from user in _db.PlanCoursUtilisateur
                        where user.idPlanCours == id
                        select user;

            var planCoursUser = query.First();
            var utilisateur = HttpContext.GetOwinContext()
            .GetUserManager<ApplicationUserManager>()
            .FindById(planCoursUser.idPlanCoursUtilisateur);
            var bureauProf = planCoursUser.bureauProf;
            var noPoste = planCoursUser.poste;
            var courrielProf = utilisateur.Email;
            var nomProf = utilisateur.nom;
            var prenomProf = utilisateur.prenom;


            apercu.CreatePageTitre(nomProf, prenomProf, bureauProf, noPoste, courrielProf);
            return apercu;
        }
    }
}
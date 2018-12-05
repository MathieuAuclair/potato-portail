using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationPlanCadre.App_Code;
using ApplicationPlanCadre.ViewModels;
using ApplicationPlanCadre.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ApplicationPlanCadre.Controllers
{
    public class ApercuController : Controller
    {
        //le viewmodel relié à l'aperçu d'un plan de cours
        ApercuViewModel viewModel;
        //database context
        BDPlanCadre db = new BDPlanCadre();

        //fonctions pour aller chercher toutes les infos d'un plan de cours
        ApercuPlanCours apercu = new ApercuPlanCours();
        
        // GET: Apercu
        public ActionResult Index(ApercuPlanCours apercu, int? id)
        {
            viewModel = new ApercuViewModel();
            var courrielConnexion = User.Identity.Name;
            var requete = from acces in db.AccesProgramme
                          where acces.userMail == courrielConnexion
                          select acces.discipline;
            List<PlanCours> listePlanCours = new List<PlanCours>();
            foreach (int PlanCoursId in GetPlanCours())
            {
                listePlanCours.Add(db.PlanCours.Find(PlanCoursId));
            }
            foreach (PlanCours pc in listePlanCours)
            {
                var nomSection = from nomS in db.NomSection
                                 join cs in db.ContenuSection on nomS.idNomSection equals cs.idNomSection
                                 join ts in db.TexteSection on cs.idContenuSection equals ts.idContenuSection
                                 join pco in db.PlanCours on ts.idPlanCours equals pco.idPlanCours
                                 where pco.idPlanCours == pc.idPlanCours
                                 select nomS;

                viewModel.mainPageViewModel.NomSections = new List<List<NomSection>>();
                viewModel.mainPageViewModel.NomSections.Add(new List<NomSection> { nomSection as NomSection });
            }

            if (id == null)
            {
                id = 1;
            }
            var idPlanCadre = from planCours in db.PlanCours
                              join cours in db.Cours on planCours.idCours equals cours.idCours
                              join planCadre in db.PlanCadre on cours.idPlanCadre equals planCadre.idPlanCadre
                              where planCours.idPlanCours == id
                              select planCadre.idPlanCadre;

            viewModel.mainPageViewModel.PlanCours = listePlanCours;
            viewModel.mainPageViewModel.contenuSection = db.ContenuSection.ToList();
            viewModel.mainPageViewModel.nomSection = db.NomSection.ToList();
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
            viewModel.texteContenu = new string[15];
            viewModel.titreSection = new string[15];
            List<int> listeSection = retourneSection(requete.First(), Convert.ToInt32(id));
            viewModel.indexSection = listeSection;
            foreach(int section in listeSection)
            {
                try
                {
                    var texte = CreationSectionDepart(Convert.ToInt32(id), section, requete.First());
                    viewModel.texteContenu[section] = texte;
                    var titre = CreationTitreSection(Convert.ToInt32(id), section);
                    viewModel.titreSection[section] = titre;
                }
                catch (Exception e)
                {
                    var textecontenu = CreationSectionDefaut(Convert.ToInt32(id), section, requete.First());
                    viewModel.texteContenu[section] = textecontenu;
                    var titreSection = CreationTitreSection(Convert.ToInt32(id), section);
                    viewModel.titreSection[section] = titreSection;
                }

            }

                CreationEnonceCompetence(viewModel, (int)id);


            return View(viewModel);
        }
        public ActionResult Create()
        {
            List <PlanCoursDepart> pcd = new List<PlanCoursDepart>();
            viewModel = new ApercuViewModel();
            var liste = db.Cours.ToList();

            viewModel.PlanCadre = db.PlanCadre.ToList();
            return View(viewModel);
        }
        //création d'un aperçu d'un plan de cours
        [HttpPost]
        public ActionResult Create(ApercuViewModel AVM, FormCollection collection)
        {
            try {
                string UserId = User.Identity.GetUserId();
                PlanCadre planCadre = db.PlanCadre.Find(AVM.idPlanCadre);
                var Cours = from cours in db.Cours
                            where cours.idPlanCadre == AVM.idPlanCadre
                            select cours;
                int CoursId = Cours.First().idCours;
                PlanCours PC = new PlanCours()
                {
                    dateCreation = DateTime.Today,
                    dateValidation = null,
                    idCours = CoursId,
                    statusPlanCours = false,
                };
                db.PlanCours.Add(PC);
                db.SaveChanges();
                int idPlanCours = PC.idPlanCours;
                PlanCoursUtilisateur PCU = new PlanCoursUtilisateur()
                {
                    idPlanCoursUtilisateur = UserId,
                    idPlanCours = idPlanCours,
                    bureauProf = AVM.BureauProf,
                    poste = AVM.NoPoste,
                };
                db.PlanCoursUtilisateurs.Add(PCU);
                db.SaveChanges();
                apercu = GetUser(idPlanCours);
                for (int i = 1; i < 15; i++)
                {
                    TexteSection texteSectionDefault = new TexteSection();
                    texteSectionDefault.idPlanCours = idPlanCours;
                    texteSectionDefault.idContenuSection = i;
                    db.TexteSection.Add(texteSectionDefault);
                    db.SaveChanges();
                }
                return RedirectToAction("Index", apercu);
                    }
            catch
            {
                return View();
            }
        }

        //Permet d'aller chercher les informations d'un plan cadre pour créer le plan cours
        public string CreationInfoCours(int id)
        {
            var query = from PC in db.PlanCadre
                        where PC.idPlanCadre == id
                        select PC;

            PlanCadre planCadre = query.First();

            return apercu.InsertionInfoCours(planCadre.numeroCours, planCadre.titreCours, Convert.ToInt32(planCadre.nbHeureTheorie), Convert.ToInt32(planCadre.nbHeurePratique), Convert.ToInt32(planCadre.nbHeureDevoir));
 
        }

        //non utilisée pour l'instant, importante pour la création des sections -- à venir
        public List<int> retourneSection(string discipline, int id )
        {
            var section = from nomSection in db.NomSection
                          join contenuSection in db.ContenuSection on nomSection.idNomSection equals contenuSection.idNomSection
                          join TexteSection in db.TexteSection on contenuSection.idContenuSection equals TexteSection.idContenuSection
                          join planCours in db.PlanCours on TexteSection.idPlanCours equals planCours.idPlanCours
                          where planCours.idPlanCours == id
                          select nomSection.idNomSection;
            return section.ToList();
        }
        public string CreationSectionDepart(int id, int idSection, string discipline)
        {
            var TexteSection = from texte in db.PlanCoursDeparts
                                      where texte.discipline == discipline && texte.idNomSection == idSection
                                      select texte.texteContenu;

            return TexteSection.First();
        }
        public string CreationSectionDefaut(int id, int idSection, string discipline)
        {
            var TexteSection = from texte in db.ContenuSection
                               join nomSection in db.NomSection on texte.idNomSection equals nomSection.idNomSection
                               where texte.idNomSection == idSection
                               select texte.texteContenu;

            return TexteSection.First();
        }
        public string CreationTitreSection(int id, int idSection)
        {
            var TexteSection = from NomSection in db.NomSection
                               join ContenuSection in db.ContenuSection on NomSection.idNomSection equals ContenuSection.idNomSection
                               where NomSection.idNomSection == idSection
                               select NomSection.titreSection;
            return TexteSection.First();
        }
        //non utilisée pour l'instant, à voir pour l'intégrer bientôt
        public void CreationEnonceCompetence(ApercuViewModel AVM, int id)
        {
            List<ElementCompetence> listeElementCompetence = new List<ElementCompetence>();
            List<EnonceCompetence> listeEnonceCompetence = new List<EnonceCompetence>();
            List<ElementConnaissance> listeElementConnaissance = new List<ElementConnaissance>();
            List<ActiviteApprentissage> listeActiviteApprentissage = new List<ActiviteApprentissage>();
            List<PlanCadreCompetence> listePlanCadreCompetence = new List<PlanCadreCompetence>();
            List<PlanCadreElement> listePlanCadreElement = new List<PlanCadreElement>();
            List<int> ponderationEnHeure = new List<int>();

            PlanCours PC = db.PlanCours.Find(id);
            Cours cours = db.Cours.Find(PC.idCours);
            PlanCadre planCadre = db.PlanCadre.Find(cours.idPlanCadre);
            
            var EnonceCompetence = from Enonce in db.EnonceCompetence
                                    join PlanCadreCompetence in db.PlanCadreCompetence on Enonce.idCompetence equals PlanCadreCompetence.idCompetence
                                    join PlanCadre in db.PlanCadre on PlanCadreCompetence.idPlanCadre equals PlanCadre.idPlanCadre
                                    where PlanCadre.idPlanCadre == planCadre.idPlanCadre
                                    select Enonce;

            foreach (Models.EnonceCompetence Enonce in EnonceCompetence)
            {
                var planCadreCompetence = from PCC in db.PlanCadreCompetence
                                          join EC in db.EnonceCompetence on PCC.idCompetence equals EC.idCompetence
                                          where PCC.idCompetence == Enonce.idCompetence
                                          select PCC;
                foreach(Models.PlanCadreCompetence planCadreComp in planCadreCompetence)
                {
                    listePlanCadreCompetence.Add(planCadreComp);
                    var planCadreElement = from PCE in db.PlanCadreElement
                                           where PCE.idPlanCadreCompetence == planCadreComp.idPlanCadreCompetence
                                           select PCE;
                    foreach(Models.PlanCadreElement planCadreElements in planCadreElement)
                    {
                        listePlanCadreElement.Add(planCadreElements);
                    }
                }
                var ponderation = (from PlanCadreCompetence in db.PlanCadreCompetence
                                  where PlanCadreCompetence.idCompetence == Enonce.idCompetence
                                  select PlanCadreCompetence.ponderationEnHeure);
                foreach(int ponderationHeure in ponderation)
                {
                    ponderationEnHeure.Add(Convert.ToInt32(ponderation.First()));
                }
                listeEnonceCompetence.Add(Enonce);
                var ElementCompetence = from Element in db.ElementCompetence
                                        join PlanCadreElement in db.PlanCadreElement on Element.idElement equals PlanCadreElement.idElement
                                        join PlanCadreCompetence in db.PlanCadreCompetence on PlanCadreElement.idPlanCadreCompetence equals PlanCadreCompetence.idPlanCadreCompetence
                                        join competence in db.EnonceCompetence on PlanCadreCompetence.idCompetence equals competence.idCompetence
                                        where competence.idCompetence == Enonce.idCompetence
                                        select Element;
                foreach(Models.ElementCompetence Element in ElementCompetence)
                {
                    listeElementCompetence.Add(Element);
                    var elementConnaissances = from Connaissance in db.ElementConnaissance
                                              join PlanCadreElement in db.PlanCadreElement on Connaissance.idPlanCadreElement equals PlanCadreElement.idPlanCadreElement
                                              join element in db.ElementCompetence on PlanCadreElement.idElement equals element.idElement
                                              where element.idElement == Element.idElement
                                              select Connaissance;

                    foreach (Models.ElementConnaissance connaissance in elementConnaissances)
                    {
                        listeElementConnaissance.Add(connaissance);
                    }
                    var activiteApprentissages = from Activite in db.ActiviteApprentissage
                                                join PlanCadreElement in db.PlanCadreElement on Activite.idPlanCadreElement equals PlanCadreElement.idElement
                                                join elementCompetence in db.ElementCompetence on PlanCadreElement.idElement equals elementCompetence.idElement
                                                where elementCompetence.idElement == Element.idElement
                                                select Activite;
                    foreach (Models.ActiviteApprentissage activite in activiteApprentissages)
                    {
                        listeActiviteApprentissage.Add(activite);
                    }

                }
            }
            AVM.listeEnonceCompetence = listeEnonceCompetence;
            AVM.listeElementCompetence = listeElementCompetence;
            AVM.listeActiviteApprentissage = listeActiviteApprentissage;
            AVM.listeElementConnaissance = listeElementConnaissance;
            AVM.ponderationEnHeure = ponderationEnHeure;
            AVM.listePlanCadreCompetence = listePlanCadreCompetence;
            AVM.listePlanCadreElement = listePlanCadreElement;
        }

        public IQueryable<int> GetPlanCours()
        {
            string UserId = User.Identity.GetUserId();
            var planCours = from pcours in db.PlanCours
                            join pcu in db.PlanCoursUtilisateurs on pcours.idPlanCours equals pcu.idPlanCours
                            where pcu.idPlanCoursUtilisateur == UserId
                            select pcours.idPlanCours;
            return planCours;

        }

        public ApercuPlanCours GetUser(int id)
        {
            string bureauProf, noPoste, CourrielProf, nomProf, prenomProf;
            ApercuPlanCours apercu = new ApercuPlanCours();
            var query = from user in db.PlanCoursUtilisateurs
                        where user.idPlanCours == id
                        select user;

            PlanCoursUtilisateur planCoursUser = query.First();
            var Utilisateur = HttpContext.GetOwinContext()
            .GetUserManager<ApplicationUserManager>()
            .FindById(planCoursUser.idPlanCoursUtilisateur);
            bureauProf = planCoursUser.bureauProf;
            noPoste = planCoursUser.poste;
            CourrielProf = Utilisateur.Email;
            nomProf = Utilisateur.nom;
            prenomProf = Utilisateur.prenom;


            apercu.CreatePageTitre(nomProf, prenomProf, bureauProf, noPoste, CourrielProf);
            return apercu;
        }
    }
}
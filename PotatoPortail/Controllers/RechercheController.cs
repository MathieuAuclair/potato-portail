using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Web.Mvc;
using ApplicationPlanCadre.Helpers;
using ApplicationPlanCadre.Models;
using ApplicationPlanCadre.Models.eSports;
using ApplicationPlanCadre.Models.Reunions;

namespace ApplicationPlanCadre.Controllers
{
    public class RechercheController : Controller
    {
        private BDPlanCadre db = new BDPlanCadre();

        //todo: la recherche ne support présentement pas la recherche avec les espaces
        [HttpGet]
        public ActionResult Recherche(string stringRechercher, string tableRecherche)
        {
            dynamic model = new ExpandoObject();
            ViewBag.txtRecherche = stringRechercher;
            ViewBag.modelActuel = tableRecherche;
            if (stringRechercher != "")
            {
                stringRechercher = stringRechercher.Trim();

                model.EnonceCompetence = getEnonceCompetence(stringRechercher);
                model.ElementCompetence = getElemCompetence(stringRechercher);
                model.DevisMinistere = getDevis(stringRechercher);
                model.Section = getSections(stringRechercher);
                model.Cours = getCours(stringRechercher);
                model.joueur = getJoueur(stringRechercher);
                model.Equipe = getEquipe(stringRechercher);
                model.jeu = getJeu(stringRechercher);
                model.entraineur = getEntraineur(stringRechercher);
                model.OrdreDuJour = getOrdreDuJour(stringRechercher);
            }
            else
            {
                model.EnonceCompetence = null;
                model.ElementCompetence = null;
                model.DevisMinistere = null;
                model.Section = null;
                model.Cours = null;
                model.joueur = null;
                model.Equipe = null;
                model.jeu = null;
                model.entraineur = null;
                model.OrdreDuJour = null;
            }

            return View("Recherche", model);
        }

        private List<RechercheDevisMinistere> getDevis(string stringRechercher)
        {
            List<RechercheDevisMinistere> devisListe = new List<RechercheDevisMinistere>();

            var devis = from tableDevisMinistere in db.DevisMinistere
                where tableDevisMinistere.specialisation.Contains(stringRechercher) || tableDevisMinistere.codeSpecialisation.Contains(stringRechercher) ||
                      tableDevisMinistere.discipline.Contains(stringRechercher) || tableDevisMinistere.annee.Contains(stringRechercher)
                select tableDevisMinistere;

            foreach (DevisMinistere tableDevisMinistere in devis)
            {
                devisListe.Add(new RechercheDevisMinistere
                {
                    idDevis = tableDevisMinistere.idDevis,
                    annee = tableDevisMinistere.annee.SurlignerMotsClee(stringRechercher, "yellow", false),
                    codeSpecialisation = tableDevisMinistere.codeSpecialisation.SurlignerMotsClee(stringRechercher, "yellow", false),
                    specialisation = tableDevisMinistere.specialisation.SurlignerMotsClee(stringRechercher, "yellow", false),
                    nbUnite = tableDevisMinistere.nbUnite,
                    nbHeureFrmGenerale = tableDevisMinistere.nbHeureFrmGenerale,
                    nbHeureFrmSpecifique = tableDevisMinistere.nbHeureFrmSpecifique,
                    condition = tableDevisMinistere.condition.SurlignerMotsClee(stringRechercher, "yellow", false),
                    sanction = tableDevisMinistere.sanction.SurlignerMotsClee(stringRechercher, "yellow", false),
                    discipline = tableDevisMinistere.discipline.SurlignerMotsClee(stringRechercher, "yellow", false),
                    total = Convert.ToInt32((tableDevisMinistere.nbHeureFrmGenerale + tableDevisMinistere.nbHeureFrmSpecifique))
                });
            }

            return devisListe;
        }

        private List<RechecheEnonceCompetence> getEnonceCompetence(string stringRechercher)
        {
            List<RechecheEnonceCompetence> enonComptListe = new List<RechecheEnonceCompetence>();

            var enonce = from tableEnonceCompetence in db.EnonceCompetence
                where tableEnonceCompetence.codeCompetence.Contains(stringRechercher) || tableEnonceCompetence.description.Contains(stringRechercher)
                select tableEnonceCompetence;

            foreach (EnonceCompetence tableEnonceCompetence in enonce)
            {
                enonComptListe.Add(new RechecheEnonceCompetence
                {
                    idCompetence = tableEnonceCompetence.idCompetence,
                    idDevis = tableEnonceCompetence.idDevis,
                    codeCompetence = tableEnonceCompetence.codeCompetence.SurlignerMotsClee(stringRechercher, "yellow", false),
                    description = tableEnonceCompetence.description.SurlignerMotsClee(stringRechercher, "yellow", false)
                });
            }

            return enonComptListe;
        }

        private List<RechecheElementCompetence> getElemCompetence(string stringRechercher)
        {
            List<RechecheElementCompetence> elementListe = new List<RechecheElementCompetence>();

            var enonce = from tableElementCompetence in db.ElementCompetence
                where tableElementCompetence.description.Contains(stringRechercher)
                orderby tableElementCompetence.numero
                select tableElementCompetence;

            foreach (ElementCompetence tableElementCompetence in enonce)
            {
                elementListe.Add(new RechecheElementCompetence
                {
                    idElement = tableElementCompetence.idElement,
                    idCompetence = tableElementCompetence.idCompetence,
                    description = tableElementCompetence.description.SurlignerMotsClee(stringRechercher, "yellow", false),
                });
            }

            return elementListe;
        }

        private List<RechecheProgramme> getProgram(string stringRechercher)
        {
            List<RechecheProgramme> programme = new List<RechecheProgramme>();

            var requete = from tableProgramme in db.Programme
                where tableProgramme.annee.Contains(stringRechercher) || tableProgramme.nom.Contains(stringRechercher)
                select tableProgramme;

            foreach (Programme tableProgramme in requete)
            {
                programme.Add(new RechecheProgramme
                {
                    idProgramme = Convert.ToInt32(tableProgramme.idProgramme),
                    annee = tableProgramme.annee.SurlignerMotsClee(stringRechercher, "yellow", false),
                    nom = tableProgramme.nom.SurlignerMotsClee(stringRechercher, "yellow", false),
                    idDevis = tableProgramme.idDevis
                });
            }
            return programme;
        }

        private List<RechercheSection> getSections(string stringRechercher)
        {
            List<RechercheSection> SectionListe = new List<RechercheSection>();

            var Section = from tableSection in db.NomSection
                          where tableSection.titreSection.Contains(stringRechercher)
                          select tableSection;
            foreach (NomSection tableSection in Section)
            {
                SectionListe.Add(new RechercheSection
                {
                    idNomSection = tableSection.idNomSection,
                    titreSection = tableSection.titreSection.SurlignerMotsClee(stringRechercher, "yellow", false)
                });
            }

            return SectionListe;
        }

        private List<RechercheCours> getCours(string stringRechercher)
        {
            List<RechercheCours> CoursListe = new List<RechercheCours>();

            var Cours = from tablePlanCadre in db.PlanCadre
                        where tablePlanCadre.titreCours.Contains(stringRechercher)
                        select tablePlanCadre;
            foreach (PlanCadre tablePlanCadre in Cours)
            {
                CoursListe.Add(new RechercheCours
                {
                    numeroCours = tablePlanCadre.numeroCours,
                    titreCours = tablePlanCadre.titreCours.SurlignerMotsClee(stringRechercher, "yellow", false)
                });
            }

            return CoursListe;
        }

        private List<RechercheJoueur> getJoueur(string stringRechercher)
        {
            List<RechercheJoueur> JoueurListe = new List<RechercheJoueur>();

            var lesJoueur = from tableJoueur in db.Joueurs
                join BDMembEsport in db.MembreESports on tableJoueur.MembreESportsId equals BDMembEsport.id into BDMembEsport2
                from tableMembre in BDMembEsport2
                where tableJoueur.pseudoJoueur.Contains(stringRechercher) || tableMembre.prenom.Contains(stringRechercher) || tableMembre.nom.Contains(stringRechercher)
                select new
                {
                    idJoueur = tableJoueur.MembreESportsId,
                    NomJoueur = tableMembre.prenom + " " + tableMembre.nom,
                    PseudoJoueur = tableJoueur.pseudoJoueur,
                    CourrielJoueur = tableJoueur.Profil.courriel
                } ;
            foreach ( var tableJoueur in lesJoueur)
            {
                JoueurListe.Add(new RechercheJoueur
                {
                    idJoueur = tableJoueur.idJoueur,
                    pseudoJoueur = tableJoueur.PseudoJoueur.SurlignerMotsClee(stringRechercher, "yellow", false),
                    NomJoueur = tableJoueur.NomJoueur.SurlignerMotsClee(stringRechercher, "yellow", false),
                    CourrielJoueur = tableJoueur.CourrielJoueur
                    
                });
            }

            return JoueurListe;
        }

        private List<RechercheEquipe> getEquipe(string stringRechercher)
        {
            List<RechercheEquipe> EquipeListe = new List<RechercheEquipe>();

            var Equipe = from tableEquipe in db.Equipes
                where tableEquipe.nomEquipe.Contains(stringRechercher) && tableEquipe.estMonojoueur==false
                select tableEquipe;
            foreach (Equipe tableEquipe in Equipe)
            {
                EquipeListe.Add(new RechercheEquipe
                {
                    idEquipe = tableEquipe.id,
                    NomEquipe = tableEquipe.nomEquipe.SurlignerMotsClee(stringRechercher, "yellow", false)
                });
            }

            return EquipeListe;
        }

        private List<RechercheJeu> getJeu(string stringRechercher)
        {
            List<RechercheJeu> JeuListe = new List<RechercheJeu>();

            var Jeux = from tableJeu in db.Jeux
                where tableJeu.nomJeu.Contains(stringRechercher)
                select tableJeu;
            foreach (Jeu tableJeu in Jeux)
            {
                JeuListe.Add(new RechercheJeu
                {
                    idJeu = tableJeu.id,
                    NomJeu = tableJeu.nomJeu.SurlignerMotsClee(stringRechercher, "yellow", false),
                    DescriptionJeu = tableJeu.description
                });
            }

            return JeuListe;
        }

        private List<RechercheEntraineur> getEntraineur(string stringRechercher)
        {
            List<RechercheEntraineur> entraineurListe = new List<RechercheEntraineur>();

            var entraineurs = from tableEntraineur in db.Entraineurs
                              where tableEntraineur.pseudoEntraineur.Contains(stringRechercher) || tableEntraineur.prenomEntraineur.Contains(stringRechercher) || tableEntraineur.prenomEntraineur.Contains(stringRechercher)
                              select tableEntraineur;

            foreach (var tableEntraineur in entraineurs)
            {  
                entraineurListe.Add(new RechercheEntraineur
                {
                    idEntraineur = tableEntraineur.id,
                    NomEntraineur = tableEntraineur.nomComplet.SurlignerMotsClee(stringRechercher, "yellow", false),
                    PseudoEntraineur = tableEntraineur.pseudoEntraineur.SurlignerMotsClee(stringRechercher, "yellow", false),
                    TelephoneEntraineur = tableEntraineur.numTel,
                    CourrielEntraineur = tableEntraineur.adresseCourriel
                });
            }

            return entraineurListe;
        }

        private List<RechercheOrdreDuJour> getOrdreDuJour(string stringRechercher)
        {
            List<RechercheOrdreDuJour> ordredujourliste = new List<RechercheOrdreDuJour>();

            var contexte = from tableOrdreDuJour in db.OrdreDuJour
                           where tableOrdreDuJour.TitreOdJ.Contains(stringRechercher)
                           select tableOrdreDuJour;
            foreach(OrdreDuJour tableOrdreDuJour in contexte)
            {
                ordredujourliste.Add(new RechercheOrdreDuJour
                {
                    IdOrdreDuJour = tableOrdreDuJour.IdOdJ,
                    titre = tableOrdreDuJour.TitreOdJ.SurlignerMotsClee(stringRechercher, "yellow", false)
                });
            }
            return ordredujourliste;
        }
    }
}

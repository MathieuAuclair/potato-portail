using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using ApplicationPlanCadre.Helpers;
using PotatoPortail.Migrations;
using PotatoPortail.Models;
using PotatoPortail.ViewModels.ProjetPrincipal;

namespace PotatoPortail.Controllers
{
    public class RechercheController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

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

                model.EnonceCompetence = GetEnonceCompetence(stringRechercher);
                model.ElementCompetence = GetElemCompetence(stringRechercher);
                model.DevisMinistere = GetDevis(stringRechercher);
                model.Section = GetSections(stringRechercher);
                model.Cours = GetCours(stringRechercher);
                model.joueur = getJoueur(stringRechercher);
                model.Equipe = GetEquipe(stringRechercher);
                model.jeu = GetJeu(stringRechercher);
                model.entraineur = getEntraineur(stringRechercher);
                model.OrdreDuJour = GetOrdreDuJour(stringRechercher);
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

        private List<RechercheDevisMinistere> GetDevis(string stringRechercher)
        {
            List<RechercheDevisMinistere> devisListe = new List<RechercheDevisMinistere>();

            var devis = from tableDevisMinistere in _db.DevisMinistere
                where tableDevisMinistere.Specialisation.Contains(stringRechercher) || tableDevisMinistere.CodeSpecialisation.Contains(stringRechercher) ||
                      tableDevisMinistere.Discipline.Contains(stringRechercher) || tableDevisMinistere.Annee.Contains(stringRechercher)
                select tableDevisMinistere;

            foreach (DevisMinistere tableDevisMinistere in devis)
            {
                devisListe.Add(new RechercheDevisMinistere
                {
                    IdDevis = tableDevisMinistere.IdDevis,
                    Annee = tableDevisMinistere.Annee.SurlignerMotsClee(stringRechercher, "yellow", false),
                    CodeSpecialisation = tableDevisMinistere.CodeSpecialisation.SurlignerMotsClee(stringRechercher, "yellow", false),
                    Specialisation = tableDevisMinistere.Specialisation.SurlignerMotsClee(stringRechercher, "yellow", false),
                    NbUnite = tableDevisMinistere.NbUnite,
                    NbHeureFrmGenerale = tableDevisMinistere.NbHeureFrmGenerale,
                    NbHeureFrmSpecifique = tableDevisMinistere.NbHeureFrmSpecifique,
                    Condition = tableDevisMinistere.Condition.SurlignerMotsClee(stringRechercher, "yellow", false),
                    Sanction = tableDevisMinistere.Sanction.SurlignerMotsClee(stringRechercher, "yellow", false),
                    Discipline = tableDevisMinistere.Discipline.SurlignerMotsClee(stringRechercher, "yellow", false),
                    Total = Convert.ToInt32((tableDevisMinistere.NbHeureFrmGenerale + tableDevisMinistere.NbHeureFrmSpecifique))
                });
            }

            return devisListe;
        }

        private List<RechecheEnonceCompetence> GetEnonceCompetence(string stringRechercher)
        {
            var enonComptListe = new List<RechecheEnonceCompetence>();

            var enonce = from tableEnonceCompetence in _db.EnonceCompetence
                where tableEnonceCompetence.CodeCompetence.Contains(stringRechercher) || tableEnonceCompetence.Description.Contains(stringRechercher)
                select tableEnonceCompetence;

            foreach (var tableEnonceCompetence in enonce)
            {
                enonComptListe.Add(new RechecheEnonceCompetence
                {
                    IdCompetence = tableEnonceCompetence.IdCompetence,
                    IdDevis = tableEnonceCompetence.IdDevis,
                    CodeCompetence = tableEnonceCompetence.CodeCompetence.SurlignerMotsClee(stringRechercher, "yellow", false),
                    Description = tableEnonceCompetence.Description.SurlignerMotsClee(stringRechercher, "yellow", false)
                });
            }

            return enonComptListe;
        }

        private List<RechecheElementCompetence> GetElemCompetence(string stringRechercher)
        {
            var enonce = from tableElementCompetence in _db.ElementCompetence
                where tableElementCompetence.Description.Contains(stringRechercher)
                orderby tableElementCompetence.Numero
                select tableElementCompetence;

            return enonce.Select(tableElementCompetence => new RechecheElementCompetence {IdElement = tableElementCompetence.IdElement, IdCompetence = tableElementCompetence.IdCompetence, Description = tableElementCompetence.Description.SurlignerMotsClee(stringRechercher, "yellow", false),}).ToList();
        }

        private List<RechecheProgramme> GetProgram(string stringRechercher)
        {
            var programme = new List<RechecheProgramme>();

            var requete = from tableProgramme in _db.Programme
                where tableProgramme.Annee.Contains(stringRechercher) || tableProgramme.Nom.Contains(stringRechercher)
                select tableProgramme;

            foreach (var tableProgramme in requete)
            {
                programme.Add(new RechecheProgramme
                {
                    IdProgramme = Convert.ToInt32(tableProgramme.IdProgramme),
                    Annee = tableProgramme.Annee.SurlignerMotsClee(stringRechercher, "yellow", false),
                    Nom = tableProgramme.Nom.SurlignerMotsClee(stringRechercher, "yellow", false),
                    IdDevis = tableProgramme.IdDevis
                });
            }
            return programme;
        }

        private List<RechercheSection> GetSections(string stringRechercher)
        {
            List<RechercheSection> sectionListe = new List<RechercheSection>();

            var section = from tableSection in _db.NomSection
                          where tableSection.titreSection.Contains(stringRechercher)
                          select tableSection;
            foreach (NomSection tableSection in section)
            {
                sectionListe.Add(new RechercheSection
                {
                    IdNomSection = tableSection.idNomSection,
                    TitreSection = tableSection.titreSection.SurlignerMotsClee(stringRechercher, "yellow", false)
                });
            }

            return sectionListe;
        }

        private List<RechercheCours> GetCours(string stringRechercher)
        {
            List<RechercheCours> coursListe = new List<RechercheCours>();

            var cours = from tablePlanCadre in _db.PlanCadre
                        where tablePlanCadre.titreCours.Contains(stringRechercher)
                        select tablePlanCadre;
            foreach (PlanCadre tablePlanCadre in cours)
            {
                coursListe.Add(new RechercheCours
                {
                    numeroCours = tablePlanCadre.numeroCours,
                    titreCours = tablePlanCadre.titreCours.SurlignerMotsClee(stringRechercher, "yellow", false)
                });
            }

            return coursListe;
        }

        private List<RechercheJoueur> getJoueur(string stringRechercher)
        {
            List<RechercheJoueur> joueurListe = new List<RechercheJoueur>();

            var lesJoueur = from tableJoueur in _db.Joueurs
                join membreESports in _db.MembreESports on tableJoueur.MembreESportsId equals membreESports.Id into BDMembEsport2
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
                joueurListe.Add(new RechercheJoueur
                {
                    idJoueur = tableJoueur.idJoueur,
                    pseudoJoueur = tableJoueur.PseudoJoueur.SurlignerMotsClee(stringRechercher, "yellow", false),
                    NomJoueur = tableJoueur.NomJoueur.SurlignerMotsClee(stringRechercher, "yellow", false),
                    CourrielJoueur = tableJoueur.CourrielJoueur
                    
                });
            }

            return joueurListe;
        }

        private List<RechercheEquipe> GetEquipe(string stringRechercher)
        {
            List<RechercheEquipe> EquipeListe = new List<RechercheEquipe>();

            var Equipe = from tableEquipe in _db.Equipes
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

        private List<RechercheJeu> GetJeu(string stringRechercher)
        {
            List<RechercheJeu> jeuListe = new List<RechercheJeu>();

            var jeux = from tableJeu in _db.Jeux
                where tableJeu.NomJeu.Contains(stringRechercher)
                select tableJeu;

            foreach (var tableJeu in jeux)
            {
                jeuListe.Add(new RechercheJeu
                {
                    IdJeu = tableJeu.Id,
                    NomJeu = tableJeu.NomJeu.SurlignerMotsClee(stringRechercher, "yellow", false),
                    DescriptionJeu = tableJeu.Description
                });
            }

            return jeuListe;
        }

        private List<RechercheEntraineur> getEntraineur(string stringRechercher)
        {
            List<RechercheEntraineur> entraineurListe = new List<RechercheEntraineur>();

            var entraineurs = from tableEntraineur in _db.Entraineurs
                              where tableEntraineur.pseudoEntraineur.Contains(stringRechercher) || tableEntraineur.prenomEntraineur.Contains(stringRechercher) || tableEntraineur.prenomEntraineur.Contains(stringRechercher)
                              select tableEntraineur;

            foreach (var tableEntraineur in entraineurs)
            {  
                entraineurListe.Add(new RechercheEntraineur
                {
                    IdEntraineur = tableEntraineur.id,
                    NomEntraineur = tableEntraineur.nomComplet.SurlignerMotsClee(stringRechercher, "yellow", false),
                    PseudoEntraineur = tableEntraineur.pseudoEntraineur.SurlignerMotsClee(stringRechercher, "yellow", false),
                    TelephoneEntraineur = tableEntraineur.numTel,
                    CourrielEntraineur = tableEntraineur.adresseCourriel
                });
            }

            return entraineurListe;
        }

        private List<RechercheOrdreDuJour> GetOrdreDuJour(string stringRechercher)
        {
            List<RechercheOrdreDuJour> ordredujourliste = new List<RechercheOrdreDuJour>();

            var contexte = from tableOrdreDuJour in _db.OrdreDuJour
                           where tableOrdreDuJour.TitreOdJ.Contains(stringRechercher)
                           select tableOrdreDuJour;
            foreach(var tableOrdreDuJour in contexte)
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

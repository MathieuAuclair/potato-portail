namespace PotatoPortail.ViewModels.ProjetPrincipal
{
    public class RechecheEnonceCompetence
    {
        public int IdCompetence { get; set; }
        public string CodeCompetence { get; set; }
        public string Description { get; set; }
        public int IdDevis { get; set; }


    }
    public class RechercheContexteRealisation
    {
        public int IdContexte { get; set; }
        public string Description { get; set; }
        public int Numero { get; set; }
        public int IdCompetence { get; set; }
     }
    public class RechecheCriterePerformance
    {
        public int IdCritere { get; set; }
        public string Description { get; set; }
        public int Numero { get; set; }
        public int IdElement { get; set; }
    }
    public class RechercheDevisMinistere
    {
        public int IdDevis { get; set; }
        public string Annee { get; set; }
        public string CodeSpecialisation { get; set; }
        public string Specialisation { get; set; }
        public string NbUnite { get; set; }
        public int? NbHeureFrmGenerale { get; set; }
        public int? NbHeureFrmSpecifique { get; set; }
        public string Condition { get; set; }
        public string Sanction { get; set; }
        public string DocMinistere { get; set; }
        public int Total { get; set; }
        public string Discipline { get; set; }
    }
    public class RechecheElementCompetence
    {
        public int IdElement { get; set; }
        public string Description { get; set; }
        public int Numeros { get; set; }
        public int IdCompetence { get; set; }
    }
    public class RechercheDepartement
    {
        public string Discipline { get; set; }
        public string DisciplineSpanned { get; set; } 
        public string Commentaire { get; set; }
    }
    public class RechecheProgramme
    {
        public int IdProgramme { get; set; }
        public string Annee { get; set; }
        public string Nom { get; set; }
        public int IdDevis { get; set; }
    }
     public class RechercheSection
    {
        public int idNomSection { get;set;}
        public string titreSection {get;set;}
    }

    public class RechercheCours
    {
        public string numeroCours { set; get; }
        public string titreCours { set; get; }
    }
    public class RechercheJoueur
    {
        public string idJoueur { get; set; }
        public string pseudoJoueur { get; set; }
        public string NomJoueur { get; set; }
        public string CourrielJoueur { get; set; }
    }

    public class RechercheEquipe
    {
        public int idEquipe { get; set; }
        public string NomEquipe { get; set; }
    }

    public class RechercheJeu
    {
        public int idJeu { get; set; }
        public string NomJeu { get; set; }
        public string DescriptionJeu { get; set; }
    }

    public class RechercheEntraineur
    {
        public int idEntraineur { get; set; }
        public string NomEntraineur{ get; set; }
        public string TelephoneEntraineur { get; set; }
        public string PseudoEntraineur { get; set; }
        public string CourrielEntraineur { get; set; }
    }

    public class RechercheOrdreDuJour
    {
        public int IdOrdreDuJour { get; set; }
        public string titre { get; set; }
        public int date { get; set; }
    }
}

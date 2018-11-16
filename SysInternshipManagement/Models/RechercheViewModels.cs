using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationPlanCadre.Models
{
    public class RechecheEnonceCompetence
    {
        public int idCompetence { get; set; }
        public string codeCompetence { get; set; }
        public string description { get; set; }
        public int idDevis { get; set; }


    }
    public class RechercheContexteRealisation
    {
        public int idContexte { get; set; }
        public string description { get; set; }
        public int numero { get; set; }
        public int idCompetence { get; set; }
     }
    public class RechecheCriterePerformance
    {
        public int idCritere { get; set; }
        public string description { get; set; }
        public int numero { get; set; }
        public int idElement { get; set; }
    }
    public class RechercheDevisMinistere
    {
        public int idDevis { get; set; }
        public string annee { get; set; }
        public string codeSpecialisation { get; set; }
        public string specialisation { get; set; }
        public string nbUnite { get; set; }
        public int? nbHeureFrmGenerale { get; set; }
        public int? nbHeureFrmSpecifique { get; set; }
        public string condition { get; set; }
        public string sanction { get; set; }
        public string docMinistere { get; set; }
        public int total { get; set; }
        public string codeProgramme { get; set; }
    }
    public class RechecheElementCompetence
    {
        public int idElement { get; set; }
        public string description { get; set; }
        public int numeros { get; set; }
        public int idCompetence { get; set; }
    }
    public class RechercheEnteteProgramme
    {
        public string codeProgramme { get; set; }//la key
        public string codeProgrammeSPANNED { get; set; } //key qui peut etre modifier avec le span
        public string commentaire { get; set; }
    }
    public class RechecheProgramme
    {
        public int idProgramme { get; set; }
        public string annee { get; set; }
        public string nom { get; set; }
        public int idDevis { get; set; }
    }
}
using System;
using PotatoPortail.Interfaces;

namespace PotatoPortail
{
    public class ApercuPlanCours: IApercu
    {
        public string ImageDepart { get; set; }
        public string NomProf { get; set; }
        public string PrenomProf { get; set; }
        public string LocalProf { get; set; }
        public string TelephoneProf { get; set; }
        public string CourrielProf { get; set; }
        public string ImageCegep { get; set; }
        public string InfosCours { get; set; }
        public string Phrase { get; set; }
        public string InfosProf { get; set; }
        public string Session { get; set; }


        public void CreatePageTitre(string nomProf, string prenomProf, string localProf, string telephoneProf, string courrielProf)
        {
            NomProf = nomProf;
            PrenomProf = prenomProf;
            LocalProf = localProf;
            TelephoneProf = telephoneProf;
            CourrielProf = courrielProf;
            Creation();
        }
        public void Creation()
        {
            ImageDepart = InsertionImageDepartement();
            Phrase = InsertionPhrase();
            InfosProf = InsertionInfoProf();
            Session = InsertionSession();
            ImageCegep = InsertionImageCegep();
        }

        public string InsertionInfoCours(string numCours, string titreCours, int hTheo, int hPratique, int hDevoir)
        {
            string Texte = titreCours + "  " + numCours + "  " + "Pondération : " + hTheo + "-" + hPratique + "-" + hDevoir;
            return Texte;
        }
        public string InsertionPhrase()
        {
            string Phrase = "Pour les étudiants(tes) de 3ième année, inscrits au programme " + "Techniques de l’informatique - 420";
            return Phrase;
        }
        public string InsertionInfoProf()
        {
            string Information = "Professeur : " + PrenomProf + " " + NomProf + " Bureau : " + LocalProf + " Téléphone : " + TelephoneProf + " Courriel : " + CourrielProf;
            return Information;
        }
        public string InsertionSession()
        {
            string Session = "Session Automne-2018";
            return Session;

        }
        public string InsertionImageCegep()
        {
            var ImageCegep = "~/Images/CEGEP.png";
            return ImageCegep;
        }
        public string InsertionImageDepartement()
        {
            var img = "~/Images/DICJ.png";
            return img;
        }
    }
}
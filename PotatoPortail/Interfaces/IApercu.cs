using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace PotatoPortail.Interfaces
{
    public interface IApercu
    {
        //détermine les méthodes qui seront définies dans aperçu plan cours
        void Creation();
        string InsertionImageDepartement();
        string InsertionPhrase();
        string InsertionInfoProf();
        string InsertionSession();
        void CreatePageTitre(string nomProf, string prenomProf, string localProf, string telephoneProf, string courrielProf);
    }
}
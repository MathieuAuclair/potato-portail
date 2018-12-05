using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApplicationPlanCadre.Models;

namespace ApplicationPlanCadre.ViewModels
{
    public class PageTitreViewModel : ViewModelBase
    {
        public virtual IEnumerable<ContenuSection> contenuSection { get; set; }
        public int Id { get; set; }
        public string texteContenu { get; set; }
        public PlanCours planCours { get; set; }
        public int idPlanCours { get; set; }
        public string courrielProf { get; set; }
        public string nomProf { get; set; }
        public string prenomProf { get; set; }
        public string bureauProf { get; set; }
        public string noPoste { get; set; }
        public string nomFichier { get; set; }
        public virtual IEnumerable<NomSection> NomSections { get; set; }
        public virtual string titreSection { get; set; }
        public virtual IEnumerable<Departement> Departements { get; set; }
        public virtual int discipline { get; set; }
        public virtual string nom { get; set; }
        public Cours cours { get; set; }
        public int coursId { get; set; }
        public PlanCadre planCadre { get; set; }
        public int planCadreId { get; set; }
        public virtual IEnumerable<ElementCompetence> ElementCompetences { get; set; }
        public virtual IEnumerable<string> descriptionElementCompetences { get; set; }
        public virtual IEnumerable<ElementConnaissance> ElementConnaissances { get; set; }
        public virtual IEnumerable<string> descriptionElementConnaissances { get; set; }
    }
}
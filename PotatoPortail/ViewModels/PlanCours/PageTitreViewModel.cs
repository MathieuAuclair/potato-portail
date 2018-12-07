using System.Collections.Generic;
using PotatoPortail.Models;

namespace PotatoPortail.ViewModels.PlanCours
{
    public class PageTitreViewModel : ViewModelBase
    {
        public virtual IEnumerable<ContenuSection> ContenuSection { get; set; }
        public int Id { get; set; }
        public string TexteContenu { get; set; }
        public Models.PlanCours PlanCours { get; set; }
        public int IdPlanCours { get; set; }
        public string CourrielProf { get; set; }
        public string NomProf { get; set; }
        public string PrenomProf { get; set; }
        public string BureauProf { get; set; }
        public string NoPoste { get; set; }
        public string NomFichier { get; set; }
        public virtual IEnumerable<NomSection> NomSections { get; set; }
        public virtual string TitreSection { get; set; }
        public virtual IEnumerable<Departement> Departements { get; set; }
        public virtual int Discipline { get; set; }
        public virtual string Nom { get; set; }
        public Cours Cours { get; set; }
        public int CoursId { get; set; }
        public Models.PlanCadre PlanCadre { get; set; }
        public int PlanCadreId { get; set; }
        public virtual IEnumerable<ElementCompetence> ElementCompetences { get; set; }
        public virtual IEnumerable<string> DescriptionElementCompetences { get; set; }
        public virtual IEnumerable<ElementConnaissance> ElementConnaissances { get; set; }
        public virtual IEnumerable<string> DescriptionElementConnaissances { get; set; }
    }
}
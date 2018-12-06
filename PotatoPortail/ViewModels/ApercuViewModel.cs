using System.Collections.Generic;
using PotatoPortail.Models;

namespace PotatoPortail.ViewModels
{
    public class ApercuViewModel:PotatoPortail.ViewModels.ViewModelBase
    {
        public MainPageViewModel MainPageViewModel = new MainPageViewModel();
        public List<int> IndexSection;
        public virtual IEnumerable<ContenuSection> ContenuSection { get; set; }
        public int Id { get; set; }
        public string[] TexteContenu { get; set; }
        public PlanCours PlanCours { get; set; }
        public int IdPlanCours { get; set; }
        public string CourrielProf { get; set; } //from aspnetuser
        public string NomProf { get; set; } //from aspnetuser
        public string PrenomProf { get; set; } //from aspnetuser
        public string BureauProf { get; set; } //ajouter à planCoursUtilisateur
        public string NoPoste { get; set; } //ajouter à planCoursUtilisateur
        public string NomFichier { get; set; }
        public virtual IEnumerable<NomSection> NomSection { get; set; }
        public virtual string[] TitreSection { get; set; }
        public virtual IEnumerable<Departement> Departement { get; set; }
        public virtual int Discipline { get; set; }
        public virtual string Nom { get; set; }
        public Cours Cours { get; set; }
        public int CoursId { get; set; }
        public List<Models.PlanCadre> PlanCadre { get; set; }
        public int IdPlanCadre { get; set; }
        public virtual IEnumerable<ElementCompetence> ElementCompetence { get; set; }
        public virtual IEnumerable<string> DescriptionElementCompetence { get; set; }
        public virtual IEnumerable<ElementConnaissance> ElementConnaissance { get; set; }
        public virtual IEnumerable<string> DescriptionElementConnaissance { get; set; }
        public List<ElementCompetence> ListeElementCompetence { get; set; }
        public List<EnonceCompetence> ListeEnonceCompetence { get; set; }
        public List<ElementConnaissance> ListeElementConnaissance { get; set; }
        public List<ActiviteApprentissage> ListeActiviteApprentissage { get; set; }
        public List<PlanCadreCompetence> ListePlanCadreCompetence { get; set; }
        public List<PlanCadreElement> ListePlanCadreElement { get; set; }
        public List<int> PonderationEnHeure { get; set; }
    }
}
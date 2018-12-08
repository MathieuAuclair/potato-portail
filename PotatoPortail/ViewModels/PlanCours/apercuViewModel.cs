using System;
using System.Collections.Generic;
using PotatoPortail.Models;

namespace PotatoPortail.ViewModels.PlanCours
{
    public class ApercuViewModel:ViewModelBase
    {
        public MainPageViewModel MainPageViewModel = new MainPageViewModel();
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
        public virtual IEnumerable<NomSection> NomSection { get; set; }
        public virtual string[] TitreSection { get; set; }
        public virtual List<int> IndexSection { get; set; }
        public virtual IEnumerable<Departement> Departement { get; set; }
        public virtual int Discipline { get; set; }
        public virtual string Nom { get; set; }
        public Cours Cours { get; set; }
        public IEnumerable<Models.PlanCadre> PlanCadre { get; set; }
        public int IdPlanCadre { get; set; }
        public virtual IEnumerable<ElementCompetence> ElementCompetence { get; set; }
        public virtual IEnumerable<ElementConnaissance> ElementConnaissance { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime? DateValidation { get; set; }
        public bool StatusPlanCours { get; set; }

        public List<ElementCompetence> ListeElementCompetence { get; set; }
        public List<EnonceCompetence> ListeEnonceCompetence { get; set; }
        public List<ElementConnaissance> ListeElementConnaissance { get; set; }
        public List<ActiviteApprentissage> ListeActiviteApprentissage { get; set; }
        public List<PlanCadreCompetence> ListePlanCadreCompetence { get; set; }
        public List<PlanCadreElement> ListePlanCadreElement { get; set; }
        public List<int> PonderationEnHeure { get; set; }
    }
}
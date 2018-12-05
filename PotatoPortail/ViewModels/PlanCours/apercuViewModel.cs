using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApplicationPlanCadre.Models;

namespace ApplicationPlanCadre.ViewModels
{
    public class ApercuViewModel:ViewModelBase
    {
        public MainPageViewModel mainPageViewModel = new MainPageViewModel();
        public virtual IEnumerable<ContenuSection> ContenuSection { get; set; }
        public int Id { get; set; }
        public string TexteContenu { get; set; }
        public PlanCours PlanCours { get; set; }
        public int idPlanCours { get; set; }
        public string CourrielProf { get; set; } //from utilisateur actif
        public string NomProf { get; set; } //from aspnetuser
        public string PrenomProf { get; set; } //from aspnetuser
        public string BureauProf { get; set; } //from plancoursutilisateur
        public string NoPoste { get; set; } //from plancoursutilisateur
        public string NomFichier { get; set; }
        public virtual IEnumerable<NomSection> NomSection { get; set; }
        public virtual string[] titreSection { get; set; }
        public virtual string[] texteContenu { get; set; }
        public virtual List<int> indexSection { get; set; }
        public virtual IEnumerable<Departement> Departement { get; set; }
        public virtual int Discipline { get; set; }
        public virtual string Nom { get; set; }
        public Cours Cours { get; set; }
        public IEnumerable<PlanCadre> PlanCadre { get; set; }
        public int idPlanCadre { get; set; }
        public virtual IEnumerable<ElementCompetence> ElementCompetence { get; set; }
        public virtual IEnumerable<ElementConnaissance> ElementConnaissance { get; set; }
        public DateTime dateCreation { get; set; }
        public DateTime? dateValidation { get; set; }
        public bool statusPlanCours { get; set; }

        public List<ElementCompetence> listeElementCompetence { get; set; }
        public List<EnonceCompetence> listeEnonceCompetence { get; set; }
        public List<ElementConnaissance> listeElementConnaissance { get; set; }
        public List<ActiviteApprentissage> listeActiviteApprentissage { get; set; }
        public List<PlanCadreCompetence> listePlanCadreCompetence { get; set; }
        public List<PlanCadreElement> listePlanCadreElement { get; set; }
        public List<int> ponderationEnHeure { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PotatoPortail.Models.Plan_Cours
{
    public class TexteSection
    {
        [Key]
        [Column(Order = 1)]
        public int IdPlanCours { get; set; }
        [Key]
        [Column(Order = 2)]
        public int IdContenuSection { get; set; }

        public PlanCours PlanCours { get; set; }

        public ContenuSection ContenuSection { get; set; }
    }
}
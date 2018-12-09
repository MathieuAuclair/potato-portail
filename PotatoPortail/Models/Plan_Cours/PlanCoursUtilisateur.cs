using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PotatoPortail.Models.Plan_Cours
{
    [Table("PlanCoursUtilisateur")]
    public partial class PlanCoursUtilisateur
    {
        [Key]
        [Column(Order = 0)]
        public string IdPlanCoursUtilisateur { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPlanCours { get; set; }

        [StringLength(5)]
        public string BureauProf { get; set; }

        [StringLength(4)]
        public string Poste { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual PlanCours PlanCours { get; set; }
    }
}

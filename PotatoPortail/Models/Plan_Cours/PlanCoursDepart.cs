using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PotatoPortail.Models.Plan_Cours
{
    [Table("PlanCoursDepart")]
    public partial class PlanCoursDepart
    {

        [Key]
        [Column(Order = 0)]
        public int IdPlanCoursDepart { get; set; }

        [Column(Order = 1)]
        [StringLength(3)]
        public string Discipline { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdPlanCours { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdNomSection { get; set; }

        [Column(Order = 4)]
        [StringLength(1000)]
        public string TexteContenu { get; set; }

        [Column(Order = 5)]
        public bool? Actif { get; set; }

        public virtual Departement Departement { get; set; }

        public virtual NomSection NomSection { get; set; }

        public virtual PlanCours PlanCours { get; set; }
    }
}

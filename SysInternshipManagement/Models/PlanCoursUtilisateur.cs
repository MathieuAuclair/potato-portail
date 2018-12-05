namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PlanCoursUtilisateur")]
    public partial class PlanCoursUtilisateur
    {
        [Key]
        [Column(Order = 0)]
        public string idPlanCoursUtilisateur { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idPlanCours { get; set; }

        [StringLength(5)]
        public string bureauProf { get; set; }

        [StringLength(4)]
        public string poste { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual PlanCours PlanCours { get; set; }
    }
}

namespace SysInternshipManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PlanCoursDepart")]
    public partial class PlanCoursDepart
    {
        [Key]
        [Column(Order = 0)]
        public int idPlanCoursDepart { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(3)]
        public string discipline { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idPlanCours { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idNomSection { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(1000)]
        public string texteContenu { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool actif { get; set; }

        public virtual Departement Departement { get; set; }

        public virtual NomSection NomSection { get; set; }

        public virtual PlanCours PlanCours { get; set; }
    }
}

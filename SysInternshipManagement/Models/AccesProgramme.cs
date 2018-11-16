namespace ApplicationPlanCadre.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AccesProgramme")]
    public partial class AccesProgramme
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idAcces { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(256)]
        public string userMail { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(3)]
        public string codeProgramme { get; set; }

        public virtual EnteteProgramme EnteteProgramme { get; set; }
    }
}

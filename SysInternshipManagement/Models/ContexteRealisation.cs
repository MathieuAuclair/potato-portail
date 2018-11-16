namespace ApplicationPlanCadre.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContexteRealisation")]
    public partial class ContexteRealisation
    {
        [Key]
        public int idContexte { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "Contexte de réalisation")]
        public string description { get; set; }

        public int numero { get; set; }

        public int idCompetence { get; set; }

        public virtual EnonceCompetence EnonceCompetence { get; set; }
    }
}

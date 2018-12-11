namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RessourceDidactique")]
    public partial class RessourceDidactique
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RessourceDidactique()
        {
            SousRessourceDidactique = new HashSet<SousRessourceDidactique>();
        }

        [Key]
        public int IdRessource { get; set; }

        [Column(TypeName = "text")]
        public string NomRessource { get; set; }

        public int IdPlanCadre { get; set; }

        public virtual PlanCadre PlanCadre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SousRessourceDidactique> SousRessourceDidactique { get; set; }
    }
}

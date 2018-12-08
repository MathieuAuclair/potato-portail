namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RessourceDIdactique")]
    public partial class RessourceDIdactique
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RessourceDIdactique()
        {
            SousRessourceDidactique = new HashSet<SousRessourceDIdactique>();
        }

        [Key]
        public int IdRessource { get; set; }

        [Column(TypeName = "text")]
        public string NomRessource { get; set; }

        public int IdPlanCadre { get; set; }

        public virtual PlanCadre PlanCadre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SousRessourceDIdactique> SousRessourceDidactique { get; set; }
    }
}

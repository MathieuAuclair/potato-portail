namespace ApplicationPlanCadre.Models.Reunions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sujetpointprincipal")]
    public partial class SujetPointPrincipal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SujetPointPrincipal()
        {
            souspointsujet = new HashSet<SousPointSujet>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idPointPrincipal { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Sujet principal")]
        public string sujetPoint { get; set; }

        public int idOrdreDuJour { get; set; }

        public List<SousPointSujet> listSousPoint { get; set; }

        public virtual OrdreDuJour ordredujour { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SousPointSujet> souspointsujet { get; set; }
    }
}

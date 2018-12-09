using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PotatoPortail.Models.Plan_Cours
{
    [Table("ContenuSection")]
    public partial class ContenuSection
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContenuSection()
        {
            PlanCours = new HashSet<PlanCours>();
        }

        [Key]
        public int IdContenuSection { get; set; }

        [Required]
        [StringLength(1000)]
        public string TexteContenu { get; set; }

        public int IdNomSection { get; set; }

        public bool Modifiable { get; set; }

        public virtual NomSection NomSection { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCours> PlanCours { get; set; }
    }
}

namespace PotatoPortail.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AccesProgramme")]
    public partial class AccesProgramme
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAcces { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(256)]
        public string UserMail { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(3)]
        public string Discipline { get; set; }

        public virtual Departement Departement { get; set; }
    }
}

namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DevisMinistere")]
    public partial class DevisMinistere
    {
        string _specialisation;
        string _nbUnite;
        string _condition;
        string _sanction;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DevisMinistere()
        {
            EnonceCompetence = new HashSet<EnonceCompetence>();
            Programme = new HashSet<Programme>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDevis { get; set; }

        [Required]
        [StringLength(4)]
        [Display(Name = "Année")]
        public string Annee { get; set; }

        [Required]
        [StringLength(3)]
        [Display(Name = "Code de spécialisation")]
        [RegularExpression("^[A-Za-z][A-Za-z|0-9]$", ErrorMessage = "Le code est invalide, il doit commencer par une lettre et Ãªtre suivis d'une autre lettre ou un chiffre.")]
        public string CodeSpecialisation { get; set; }

        [StringLength(100)]
        [Display(Name = "Spécialisation")]
        public string Specialisation
        {
            get { return _specialisation != null ? _specialisation : "N/A"; }
            set
            {
                if (_specialisation != null)
                {
                    _specialisation = value.Trim();
                }
                else
                {
                    _specialisation = value;
                }
            }
        }
                
        [StringLength(6)]
        [Display(Name = "Nombre d'unités")]
        [RegularExpression("^[0-9]*(?:\\s[0-9]\\/?[0-9])?", ErrorMessage = "Veuillez inscrire Uniquement le nombre d'unités en chiffre ou en division '2/3'")]
        public string NbUnite
        {
            get { return _nbUnite; }
            set
            {
                if (_nbUnite != null)
                {
                    _nbUnite = value.Trim();
                }
                else
                {
                    _nbUnite = value;
                }
            }
        }

        [Display(Name = "Formation générale")]
        [RegularExpression("[0-9]*", ErrorMessage = "Veuillez inscrire uniquement le nombre d'heures en chiffre")]
        public int? NbHeureFrmGenerale { get; set; }

        [Display(Name = "Formation spécifique")]
        [RegularExpression("[0-9]*", ErrorMessage = "Veuillez inscrire Uniquement le nombre d'heures en chiffre")]
        public int? NbHeureFrmSpecifique { get; set; }

        [StringLength(300)]
        [Display(Name = "Préalables d'admission")]
        public string Condition
        {
            get { return _condition; }
            set
            {
                if (_condition != null)
                {
                    _condition = value.Trim();
                }
                else
                {
                    _condition = value;
                }
            }
        }

        [StringLength(150)]
        [Display(Name = "Sanction")]
        public string Sanction
        {
            get { return _sanction; }
            set
            {
                if (_sanction != null)
                {
                    _sanction = value.Trim();
                }
                else
                {
                    _sanction = value;
                }
            }
        }

        [StringLength(250)]
        [Display(Name = "Document ministériel")]
        public string DocMinistere { get; set; }

        [Required]
        [StringLength(3)]
        [Display(Name = "Discipline")]
        public string Discipline { get; set; }

        public virtual Departement Departement { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnonceCompetence> EnonceCompetence { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Programme> Programme { get; set; }
        public string Nom
        {
            get
            {
                if (Specialisation != "N/A")
                    return CodeDevis + " • " + Specialisation;
                return CodeDevis;
            }
        }

        public string CodeDevis => Discipline + "-" + Annee + "-" + CodeSpecialisation;
    }
}

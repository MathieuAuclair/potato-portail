namespace ApplicationPlanCadre.Models
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
        public int idDevis { get; set; }

        public string nom
        {
            get
            {
                if(specialisation != "N/A")
                    return codeDevis + " • " + specialisation;
                return codeDevis;
            }
        }

        public string codeDevis
        {
            get { return codeProgramme + "-" + annee + "-" + codeSpecialisation; }
        }

        [Required]
        [StringLength(4)]
        [Display(Name = "Année")]
        public string annee { get; set; }

        [Required]
        [StringLength(3)]
        [RegularExpression("^[A-Za-z][A-Za-z|0-9]$", ErrorMessage = "Le code est invalide, il doit commencer par une lettre et Ãªtre suivis d'une autre lettre ou un chiffre.")]
        [Display(Name = "Code de spécialisation")]
        public string codeSpecialisation { get; set; }

        [StringLength(30)]
        [Display(Name = "Spécialisation")]
        public string specialisation
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

        [StringLength(8)]
        [RegularExpression("^[0-9]*(?:\\s[0-9]\\/?[0-9])?", ErrorMessage = "Veuillez inscrire Uniquement le nombre d'unités en chiffre ou en division '2/3'")]
        [Display(Name = "Nombre d'unités")]
        public string nbUnite
        {
            get { return _nbUnite; }
            set {
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
        [RegularExpression("[0-9]*", ErrorMessage = "Veuillez inscrire Uniquement le nombre d'heures en chiffre")]
        public int? nbHeureFrmGenerale { get; set; }

        [Display(Name = "Formation spécifique")]
        [RegularExpression("[0-9]*", ErrorMessage = "Veuillez inscrire Uniquement le nombre d'heures en chiffre")]
        public int? nbHeureFrmSpecifique { get; set; }

        [StringLength(300)]
        [Display(Name = "Type de condition")]
        public string condition
        {
            get { return _condition; }
            set
            {
                if(_condition != null)
                {
                    _condition = value.Trim();
                }
                else
                {
                    _condition = value;
                }
            }
        }

        [StringLength(50)]
        [Display(Name = "Sanction")]
        public string sanction
        {
            get { return _sanction; }
            set
            {
                if ( _sanction != null)
                {
                    _sanction = value.Trim();
                }
                else
                {
                    _sanction = value;
                }
            }
        }

        [StringLength(200)]
        [Display(Name = "Document ministériel")]
        public string docMinistere { get; set; }

        [Required]
        [StringLength(3)]
        [Display(Name = "Code de programme")]
        public string codeProgramme { get; set; }

        public virtual EnteteProgramme EnteteProgramme { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnonceCompetence> EnonceCompetence { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Programme> Programme { get; set; }
    }
  
    public class CustomDataAttribute : RangeAttribute
    {
        public CustomDataAttribute() : base(1967, DateTime.Now.Year)
        {
            ErrorMessage = "L'année est invalide. Le devis ne peux pas avoir été crée après " + DateTime.Now.Year.ToString() + " et avant 1967";
        }
    }

}

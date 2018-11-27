using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationPlanCadre.Models.Reunions;

namespace SysInternshipManagement.Models.Reunions
{
    [Table("souspointsujet")]
    public partial class SousPointSujet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idSousPoint { get; set; }

        public string sujetSousPoint { get; set; }

        //[Column (Name="ID",IsDbGenerated=false,DbType="int Not Null")]
        public int idSujetPointPrincipal { get; set; }

        public virtual SujetPointPrincipal sujetpointprincipal { get; set; }
    }
}

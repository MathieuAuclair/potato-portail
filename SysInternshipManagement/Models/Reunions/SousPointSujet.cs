namespace ApplicationPlanCadre.Models.Reunions
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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

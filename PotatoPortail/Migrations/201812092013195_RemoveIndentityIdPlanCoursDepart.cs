namespace PotatoPortail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIndentityIdPlanCoursDepart : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.PlanCoursDepart");
            AlterColumn("dbo.PlanCoursDepart", "IdPlanCoursDepart", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.PlanCoursDepart", new[] { "IdPlanCoursDepart", "IdPlanCours", "IdNomSection" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.PlanCoursDepart");
            AlterColumn("dbo.PlanCoursDepart", "IdPlanCoursDepart", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.PlanCoursDepart", new[] { "IdPlanCoursDepart", "IdPlanCours", "IdNomSection" });
        }
    }
}

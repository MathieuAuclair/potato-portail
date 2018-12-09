namespace PotatoPortail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncreaseSizeTexteContenu : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ContenuSection", "TexteContenu", c => c.String(nullable: false, maxLength: 3000));
            AlterColumn("dbo.PlanCoursDepart", "TexteContenu", c => c.String(maxLength: 3000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PlanCoursDepart", "TexteContenu", c => c.String(maxLength: 1000));
            AlterColumn("dbo.ContenuSection", "TexteContenu", c => c.String(nullable: false, maxLength: 1000));
        }
    }
}

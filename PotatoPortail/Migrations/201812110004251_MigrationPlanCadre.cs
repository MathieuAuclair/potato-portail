namespace PotatoPortail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationPlanCadre : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Programme", "Nom", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Programme", "Nom", c => c.String(maxLength: 50, unicode: false));
        }
    }
}

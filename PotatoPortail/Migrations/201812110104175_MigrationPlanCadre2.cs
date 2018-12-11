namespace PotatoPortail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationPlanCadre2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DevisMinistere", "Specialisation", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.DevisMinistere", "Sanction", c => c.String(maxLength: 150, unicode: false));
            AlterColumn("dbo.DevisMinistere", "DocMinistere", c => c.String(maxLength: 250, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DevisMinistere", "DocMinistere", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.DevisMinistere", "Sanction", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.DevisMinistere", "Specialisation", c => c.String(maxLength: 30, unicode: false));
        }
    }
}

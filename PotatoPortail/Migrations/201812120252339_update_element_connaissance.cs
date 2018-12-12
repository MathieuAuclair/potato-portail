namespace PotatoPortail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_element_connaissance : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ElementConnaissance", "Description", c => c.String(unicode: false, storeType: "text"));
            AlterColumn("dbo.SousElementConnaissance", "DescSousElement", c => c.String(unicode: false, storeType: "text"));
        }

        public override void Down()
        {
            AlterColumn("dbo.ElementConnaissance", "Description", c => c.String(nullable: false, unicode: false, storeType: "text"));
            AlterColumn("dbo.SousElementConnaissance", "DescSousElement", c => c.String(nullable: false, unicode: false, storeType: "text"));
        }
    }
}

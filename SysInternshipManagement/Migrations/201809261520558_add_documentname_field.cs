namespace SysInternshipManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_documentname_field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Internship", "DocumentName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Internship", "DocumentName");
        }
    }
}

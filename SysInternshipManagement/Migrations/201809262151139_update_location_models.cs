namespace SysInternshipManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_location_models : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Business", "Location_IdLocation", "dbo.Location");
            DropIndex("dbo.Business", new[] { "Location_IdLocation" });
            AddColumn("dbo.Internship", "CivicNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Internship", "CountryState", c => c.String(nullable: false));
            AddColumn("dbo.Internship", "Country", c => c.String(nullable: false));
            AddColumn("dbo.Business", "CivicNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Business", "CountryState", c => c.String(nullable: false));
            AddColumn("dbo.Business", "Country", c => c.String(nullable: false));
            AddColumn("dbo.Business", "PostalCode", c => c.String(nullable: false));
            DropColumn("dbo.Business", "Location_IdLocation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Business", "Location_IdLocation", c => c.Int());
            DropColumn("dbo.Business", "PostalCode");
            DropColumn("dbo.Business", "Country");
            DropColumn("dbo.Business", "CountryState");
            DropColumn("dbo.Business", "CivicNumber");
            DropColumn("dbo.Internship", "Country");
            DropColumn("dbo.Internship", "CountryState");
            DropColumn("dbo.Internship", "CivicNumber");
            CreateIndex("dbo.Business", "Location_IdLocation");
            AddForeignKey("dbo.Business", "Location_IdLocation", "dbo.Location", "IdLocation");
        }
    }
}

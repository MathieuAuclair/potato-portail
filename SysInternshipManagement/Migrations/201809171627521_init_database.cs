namespace SysInternshipManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init_database : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        idApplication = c.Int(nullable: false, identity: true),
                        timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.idApplication);
            
            CreateTable(
                "dbo.Businesses",
                c => new
                    {
                        idBusiness = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        address = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.idBusiness);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        idContact = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        phone = c.String(nullable: false),
                        email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.idContact);
            
            CreateTable(
                "dbo.ContactBusinesses",
                c => new
                    {
                        idContactBusiness = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.idContactBusiness);
            
            CreateTable(
                "dbo.Internships",
                c => new
                    {
                        idInternship = c.Int(nullable: false, identity: true),
                        description = c.String(),
                        address = c.String(nullable: false),
                        postalCode = c.String(nullable: false),
                        salary = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.idInternship);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        idLocation = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.idLocation);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        idPost = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.idPost);
            
            CreateTable(
                "dbo.Preferences",
                c => new
                    {
                        idPreference = c.Int(nullable: false, identity: true),
                        salary = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.idPreference);
            
            CreateTable(
                "dbo.Responsibles",
                c => new
                    {
                        idResponsible = c.Int(nullable: false, identity: true),
                        firstName = c.String(nullable: false),
                        lastName = c.String(nullable: false),
                        phone = c.String(nullable: false),
                        email = c.String(nullable: false),
                        role = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.idResponsible);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        idStatus = c.Int(nullable: false, identity: true),
                        status = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.idStatus);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        idStudent = c.Int(nullable: false, identity: true),
                        firstName = c.String(nullable: false),
                        lastName = c.String(nullable: false),
                        emailSchool = c.String(nullable: false),
                        emailPersonal = c.String(nullable: false),
                        phone = c.String(nullable: false),
                        DaNumber = c.String(nullable: false),
                        permanentCode = c.String(nullable: false),
                        role = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.idStudent);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Students");
            DropTable("dbo.Status");
            DropTable("dbo.Responsibles");
            DropTable("dbo.Preferences");
            DropTable("dbo.Posts");
            DropTable("dbo.Locations");
            DropTable("dbo.Internships");
            DropTable("dbo.ContactBusinesses");
            DropTable("dbo.Contacts");
            DropTable("dbo.Businesses");
            DropTable("dbo.Applications");
        }
    }
}

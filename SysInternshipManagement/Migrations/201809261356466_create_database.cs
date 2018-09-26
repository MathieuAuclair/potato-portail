namespace SysInternshipManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_database : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Application",
                c => new
                    {
                        IdApplication = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        Internship_IdInternship = c.Int(nullable: false),
                        Student_IdStudent = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdApplication)
                .ForeignKey("dbo.Internship", t => t.Internship_IdInternship, cascadeDelete: true)
                .ForeignKey("dbo.Student", t => t.Student_IdStudent, cascadeDelete: true)
                .Index(t => t.Internship_IdInternship)
                .Index(t => t.Student_IdStudent);
            
            CreateTable(
                "dbo.Internship",
                c => new
                    {
                        IdInternship = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Address = c.String(nullable: false),
                        PostalCode = c.String(nullable: false),
                        Salary = c.Single(nullable: false),
                        Contact_IdContact = c.Int(),
                        Location_IdLocation = c.Int(),
                        Post_IdPost = c.Int(),
                        Status_IdStatus = c.Int(),
                    })
                .PrimaryKey(t => t.IdInternship)
                .ForeignKey("dbo.Contact", t => t.Contact_IdContact)
                .ForeignKey("dbo.Location", t => t.Location_IdLocation)
                .ForeignKey("dbo.Post", t => t.Post_IdPost)
                .ForeignKey("dbo.Status", t => t.Status_IdStatus)
                .Index(t => t.Contact_IdContact)
                .Index(t => t.Location_IdLocation)
                .Index(t => t.Post_IdPost)
                .Index(t => t.Status_IdStatus);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        IdContact = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdContact);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        IdLocation = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Preference_IdPreference = c.Int(),
                    })
                .PrimaryKey(t => t.IdLocation)
                .ForeignKey("dbo.Preference", t => t.Preference_IdPreference)
                .Index(t => t.Preference_IdPreference);
            
            CreateTable(
                "dbo.Post",
                c => new
                    {
                        IdPost = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Preference_IdPreference = c.Int(),
                    })
                .PrimaryKey(t => t.IdPost)
                .ForeignKey("dbo.Preference", t => t.Preference_IdPreference)
                .Index(t => t.Preference_IdPreference);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        IdStatus = c.Int(nullable: false, identity: true),
                        StatusInternship = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdStatus);
            
            CreateTable(
                "dbo.Student",
                c => new
                    {
                        IdStudent = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        EmailSchool = c.String(nullable: false),
                        EmailPersonal = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        DaNumber = c.String(nullable: false),
                        PermanentCode = c.String(nullable: false),
                        Role = c.String(nullable: false),
                        Preference_IdPreference = c.Int(),
                    })
                .PrimaryKey(t => t.IdStudent)
                .ForeignKey("dbo.Preference", t => t.Preference_IdPreference)
                .Index(t => t.Preference_IdPreference);
            
            CreateTable(
                "dbo.Preference",
                c => new
                    {
                        IdPreference = c.Int(nullable: false, identity: true),
                        Salary = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.IdPreference);
            
            CreateTable(
                "dbo.Business",
                c => new
                    {
                        IdBusiness = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Location_IdLocation = c.Int(),
                        Preference_IdPreference = c.Int(),
                    })
                .PrimaryKey(t => t.IdBusiness)
                .ForeignKey("dbo.Location", t => t.Location_IdLocation)
                .ForeignKey("dbo.Preference", t => t.Preference_IdPreference)
                .Index(t => t.Location_IdLocation)
                .Index(t => t.Preference_IdPreference);
            
            CreateTable(
                "dbo.Responsible",
                c => new
                    {
                        IdResponsible = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Role = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdResponsible);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Application", "Student_IdStudent", "dbo.Student");
            DropForeignKey("dbo.Student", "Preference_IdPreference", "dbo.Preference");
            DropForeignKey("dbo.Post", "Preference_IdPreference", "dbo.Preference");
            DropForeignKey("dbo.Location", "Preference_IdPreference", "dbo.Preference");
            DropForeignKey("dbo.Business", "Preference_IdPreference", "dbo.Preference");
            DropForeignKey("dbo.Business", "Location_IdLocation", "dbo.Location");
            DropForeignKey("dbo.Application", "Internship_IdInternship", "dbo.Internship");
            DropForeignKey("dbo.Internship", "Status_IdStatus", "dbo.Status");
            DropForeignKey("dbo.Internship", "Post_IdPost", "dbo.Post");
            DropForeignKey("dbo.Internship", "Location_IdLocation", "dbo.Location");
            DropForeignKey("dbo.Internship", "Contact_IdContact", "dbo.Contact");
            DropIndex("dbo.Business", new[] { "Preference_IdPreference" });
            DropIndex("dbo.Business", new[] { "Location_IdLocation" });
            DropIndex("dbo.Student", new[] { "Preference_IdPreference" });
            DropIndex("dbo.Post", new[] { "Preference_IdPreference" });
            DropIndex("dbo.Location", new[] { "Preference_IdPreference" });
            DropIndex("dbo.Internship", new[] { "Status_IdStatus" });
            DropIndex("dbo.Internship", new[] { "Post_IdPost" });
            DropIndex("dbo.Internship", new[] { "Location_IdLocation" });
            DropIndex("dbo.Internship", new[] { "Contact_IdContact" });
            DropIndex("dbo.Application", new[] { "Student_IdStudent" });
            DropIndex("dbo.Application", new[] { "Internship_IdInternship" });
            DropTable("dbo.Responsible");
            DropTable("dbo.Business");
            DropTable("dbo.Preference");
            DropTable("dbo.Student");
            DropTable("dbo.Status");
            DropTable("dbo.Post");
            DropTable("dbo.Location");
            DropTable("dbo.Contact");
            DropTable("dbo.Internship");
            DropTable("dbo.Application");
        }
    }
}

namespace SysInternshipManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_database : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        IdApplication = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        Internship_IdInternship = c.Int(nullable: false),
                        Student_IdStudent = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdApplication)
                .ForeignKey("dbo.Internships", t => t.Internship_IdInternship, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Student_IdStudent, cascadeDelete: true)
                .Index(t => t.Internship_IdInternship)
                .Index(t => t.Student_IdStudent);
            
            CreateTable(
                "dbo.Internships",
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
                .ForeignKey("dbo.Contacts", t => t.Contact_IdContact)
                .ForeignKey("dbo.Locations", t => t.Location_IdLocation)
                .ForeignKey("dbo.Posts", t => t.Post_IdPost)
                .ForeignKey("dbo.Status", t => t.Status_IdStatus)
                .Index(t => t.Contact_IdContact)
                .Index(t => t.Location_IdLocation)
                .Index(t => t.Post_IdPost)
                .Index(t => t.Status_IdStatus);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        IdContact = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdContact);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        IdLocation = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Preference_IdPreference = c.Int(),
                    })
                .PrimaryKey(t => t.IdLocation)
                .ForeignKey("dbo.Preferences", t => t.Preference_IdPreference)
                .Index(t => t.Preference_IdPreference);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        IdPost = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Preference_IdPreference = c.Int(),
                    })
                .PrimaryKey(t => t.IdPost)
                .ForeignKey("dbo.Preferences", t => t.Preference_IdPreference)
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
                "dbo.Students",
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
                .ForeignKey("dbo.Preferences", t => t.Preference_IdPreference)
                .Index(t => t.Preference_IdPreference);
            
            CreateTable(
                "dbo.Preferences",
                c => new
                    {
                        IdPreference = c.Int(nullable: false, identity: true),
                        Salary = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.IdPreference);
            
            CreateTable(
                "dbo.Businesses",
                c => new
                    {
                        IdBusiness = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Location_IdLocation = c.Int(),
                        Preference_IdPreference = c.Int(),
                    })
                .PrimaryKey(t => t.IdBusiness)
                .ForeignKey("dbo.Locations", t => t.Location_IdLocation)
                .ForeignKey("dbo.Preferences", t => t.Preference_IdPreference)
                .Index(t => t.Location_IdLocation)
                .Index(t => t.Preference_IdPreference);
            
            CreateTable(
                "dbo.Responsibles",
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
            DropForeignKey("dbo.Applications", "Student_IdStudent", "dbo.Students");
            DropForeignKey("dbo.Students", "Preference_IdPreference", "dbo.Preferences");
            DropForeignKey("dbo.Posts", "Preference_IdPreference", "dbo.Preferences");
            DropForeignKey("dbo.Locations", "Preference_IdPreference", "dbo.Preferences");
            DropForeignKey("dbo.Businesses", "Preference_IdPreference", "dbo.Preferences");
            DropForeignKey("dbo.Businesses", "Location_IdLocation", "dbo.Locations");
            DropForeignKey("dbo.Applications", "Internship_IdInternship", "dbo.Internships");
            DropForeignKey("dbo.Internships", "Status_IdStatus", "dbo.Status");
            DropForeignKey("dbo.Internships", "Post_IdPost", "dbo.Posts");
            DropForeignKey("dbo.Internships", "Location_IdLocation", "dbo.Locations");
            DropForeignKey("dbo.Internships", "Contact_IdContact", "dbo.Contacts");
            DropIndex("dbo.Businesses", new[] { "Preference_IdPreference" });
            DropIndex("dbo.Businesses", new[] { "Location_IdLocation" });
            DropIndex("dbo.Students", new[] { "Preference_IdPreference" });
            DropIndex("dbo.Posts", new[] { "Preference_IdPreference" });
            DropIndex("dbo.Locations", new[] { "Preference_IdPreference" });
            DropIndex("dbo.Internships", new[] { "Status_IdStatus" });
            DropIndex("dbo.Internships", new[] { "Post_IdPost" });
            DropIndex("dbo.Internships", new[] { "Location_IdLocation" });
            DropIndex("dbo.Internships", new[] { "Contact_IdContact" });
            DropIndex("dbo.Applications", new[] { "Student_IdStudent" });
            DropIndex("dbo.Applications", new[] { "Internship_IdInternship" });
            DropTable("dbo.Responsibles");
            DropTable("dbo.Businesses");
            DropTable("dbo.Preferences");
            DropTable("dbo.Students");
            DropTable("dbo.Status");
            DropTable("dbo.Posts");
            DropTable("dbo.Locations");
            DropTable("dbo.Contacts");
            DropTable("dbo.Internships");
            DropTable("dbo.Applications");
        }
    }
}

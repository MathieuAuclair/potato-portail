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
                        Etudiant_IdEtudiant = c.Int(nullable: false),
                        Stage_IdStage = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdApplication)
                .ForeignKey("dbo.Etudiant", t => t.Etudiant_IdEtudiant, cascadeDelete: true)
                .ForeignKey("dbo.Stage", t => t.Stage_IdStage, cascadeDelete: true)
                .Index(t => t.Etudiant_IdEtudiant)
                .Index(t => t.Stage_IdStage);
            
            CreateTable(
                "dbo.Etudiant",
                c => new
                    {
                        IdEtudiant = c.Int(nullable: false, identity: true),
                        Prenom = c.String(nullable: false),
                        NomDeFamille = c.String(nullable: false),
                        CourrielEcole = c.String(nullable: false),
                        CourrielPersonnel = c.String(nullable: false),
                        Telephone = c.String(nullable: false),
                        NumeroDa = c.String(nullable: false),
                        CodePermanent = c.String(nullable: false),
                        Role = c.String(nullable: false),
                        Preference_IdPreference = c.Int(),
                    })
                .PrimaryKey(t => t.IdEtudiant)
                .ForeignKey("dbo.Preference", t => t.Preference_IdPreference)
                .Index(t => t.Preference_IdPreference);
            
            CreateTable(
                "dbo.Preference",
                c => new
                    {
                        IdPreference = c.Int(nullable: false, identity: true),
                        Salaire = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.IdPreference);
            
            CreateTable(
                "dbo.Entreprise",
                c => new
                    {
                        IdEntreprise = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false),
                        NumeroCivique = c.Int(nullable: false),
                        Rue = c.String(nullable: false),
                        Ville = c.String(nullable: false),
                        Province = c.String(nullable: false),
                        Pays = c.String(nullable: false),
                        CodePostal = c.String(nullable: false),
                        Preference_IdPreference = c.Int(),
                    })
                .PrimaryKey(t => t.IdEntreprise)
                .ForeignKey("dbo.Preference", t => t.Preference_IdPreference)
                .Index(t => t.Preference_IdPreference);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        IdLocation = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false),
                        Preference_IdPreference = c.Int(),
                    })
                .PrimaryKey(t => t.IdLocation)
                .ForeignKey("dbo.Preference", t => t.Preference_IdPreference)
                .Index(t => t.Preference_IdPreference);
            
            CreateTable(
                "dbo.Poste",
                c => new
                    {
                        IdPoste = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false),
                        Preference_IdPreference = c.Int(),
                    })
                .PrimaryKey(t => t.IdPoste)
                .ForeignKey("dbo.Preference", t => t.Preference_IdPreference)
                .Index(t => t.Preference_IdPreference);
            
            CreateTable(
                "dbo.Stage",
                c => new
                    {
                        IdStage = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        CodePostal = c.String(nullable: false),
                        CivicNumber = c.Int(nullable: false),
                        NomRue = c.String(nullable: false),
                        Ville = c.String(nullable: false),
                        Province = c.String(nullable: false),
                        Pays = c.String(nullable: false),
                        Salaire = c.Single(nullable: false),
                        NomDocument = c.String(),
                        Contact_IdContact = c.Int(),
                        Location_IdLocation = c.Int(),
                        Poste_IdPoste = c.Int(),
                        Status_IdStatus = c.Int(),
                    })
                .PrimaryKey(t => t.IdStage)
                .ForeignKey("dbo.Contact", t => t.Contact_IdContact)
                .ForeignKey("dbo.Location", t => t.Location_IdLocation)
                .ForeignKey("dbo.Poste", t => t.Poste_IdPoste)
                .ForeignKey("dbo.Status", t => t.Status_IdStatus)
                .Index(t => t.Contact_IdContact)
                .Index(t => t.Location_IdLocation)
                .Index(t => t.Poste_IdPoste)
                .Index(t => t.Status_IdStatus);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        IdContact = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false),
                        Telephone = c.String(nullable: false),
                        Courriel = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdContact);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        IdStatus = c.Int(nullable: false, identity: true),
                        StatusStage = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdStatus);
            
            CreateTable(
                "dbo.Responsable",
                c => new
                    {
                        IdResponsable = c.Int(nullable: false, identity: true),
                        Prenom = c.String(nullable: false),
                        NomDeFamille = c.String(nullable: false),
                        Telephone = c.String(nullable: false),
                        Courriel = c.String(nullable: false),
                        Role = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdResponsable);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Application", "Stage_IdStage", "dbo.Stage");
            DropForeignKey("dbo.Stage", "Status_IdStatus", "dbo.Status");
            DropForeignKey("dbo.Stage", "Poste_IdPoste", "dbo.Poste");
            DropForeignKey("dbo.Stage", "Location_IdLocation", "dbo.Location");
            DropForeignKey("dbo.Stage", "Contact_IdContact", "dbo.Contact");
            DropForeignKey("dbo.Application", "Etudiant_IdEtudiant", "dbo.Etudiant");
            DropForeignKey("dbo.Etudiant", "Preference_IdPreference", "dbo.Preference");
            DropForeignKey("dbo.Poste", "Preference_IdPreference", "dbo.Preference");
            DropForeignKey("dbo.Location", "Preference_IdPreference", "dbo.Preference");
            DropForeignKey("dbo.Entreprise", "Preference_IdPreference", "dbo.Preference");
            DropIndex("dbo.Stage", new[] { "Status_IdStatus" });
            DropIndex("dbo.Stage", new[] { "Poste_IdPoste" });
            DropIndex("dbo.Stage", new[] { "Location_IdLocation" });
            DropIndex("dbo.Stage", new[] { "Contact_IdContact" });
            DropIndex("dbo.Poste", new[] { "Preference_IdPreference" });
            DropIndex("dbo.Location", new[] { "Preference_IdPreference" });
            DropIndex("dbo.Entreprise", new[] { "Preference_IdPreference" });
            DropIndex("dbo.Etudiant", new[] { "Preference_IdPreference" });
            DropIndex("dbo.Application", new[] { "Stage_IdStage" });
            DropIndex("dbo.Application", new[] { "Etudiant_IdEtudiant" });
            DropTable("dbo.Responsable");
            DropTable("dbo.Status");
            DropTable("dbo.Contact");
            DropTable("dbo.Stage");
            DropTable("dbo.Poste");
            DropTable("dbo.Location");
            DropTable("dbo.Entreprise");
            DropTable("dbo.Preference");
            DropTable("dbo.Etudiant");
            DropTable("dbo.Application");
        }
    }
}

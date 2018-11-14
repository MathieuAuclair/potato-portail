namespace SysInternshipManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLinkContactEntreprise : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contact", "Entreprise_IdEntreprise", c => c.Int(nullable: false));
            CreateIndex("dbo.Contact", "Entreprise_IdEntreprise");
            AddForeignKey("dbo.Contact", "Entreprise_IdEntreprise", "dbo.Entreprise", "IdEntreprise", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contact", "Entreprise_IdEntreprise", "dbo.Entreprise");
            DropIndex("dbo.Contact", new[] { "Entreprise_IdEntreprise" });
            DropColumn("dbo.Contact", "Entreprise_IdEntreprise");
        }
    }
}

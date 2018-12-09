namespace PotatoPortail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifs_models_eSports : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Entraineurs", "PseudoEntraineur", c => c.String(nullable: false, maxLength: 25));
            DropColumn("dbo.Joueurs", "IdEtudiant");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Joueurs", "IdEtudiant", c => c.Int(nullable: false));
            AlterColumn("dbo.Entraineurs", "PseudoEntraineur", c => c.String(nullable: false));
        }
    }
}

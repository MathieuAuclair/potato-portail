namespace PotatoPortail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initContextBd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccesProgramme",
                c => new
                    {
                        IdAcces = c.Int(nullable: false, identity: true),
                        UserMail = c.String(nullable: false, maxLength: 256),
                        Discipline = c.String(nullable: false, maxLength: 3, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => new { t.IdAcces, t.UserMail, t.Discipline })
                .ForeignKey("dbo.Departement", t => t.Discipline)
                .Index(t => t.Discipline);
            
            CreateTable(
                "dbo.Departement",
                c => new
                    {
                        Discipline = c.String(nullable: false, maxLength: 3, fixedLength: true, unicode: false),
                        Nom = c.String(nullable: false, maxLength: 200, unicode: false),
                    })
                .PrimaryKey(t => t.Discipline);
            
            CreateTable(
                "dbo.DevisMinistere",
                c => new
                    {
                        IdDevis = c.Int(nullable: false, identity: true),
                        Annee = c.String(nullable: false, maxLength: 4, fixedLength: true, unicode: false),
                        CodeSpecialisation = c.String(nullable: false, maxLength: 3, unicode: false),
                        Specialisation = c.String(maxLength: 30, unicode: false),
                        NbUnite = c.String(maxLength: 6, unicode: false),
                        NbHeureFrmGenerale = c.Int(),
                        NbHeureFrmSpecifique = c.Int(),
                        Condition = c.String(maxLength: 300, unicode: false),
                        Sanction = c.String(maxLength: 50, unicode: false),
                        DocMinistere = c.String(maxLength: 200, unicode: false),
                        Discipline = c.String(nullable: false, maxLength: 3, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.IdDevis)
                .ForeignKey("dbo.Departement", t => t.Discipline)
                .Index(t => t.Discipline);
            
            CreateTable(
                "dbo.EnonceCompetence",
                c => new
                    {
                        IdCompetence = c.Int(nullable: false, identity: true),
                        CodeCompetence = c.String(nullable: false, maxLength: 4, unicode: false),
                        Description = c.String(nullable: false, maxLength: 300, unicode: false),
                        Obligatoire = c.Boolean(nullable: false),
                        Actif = c.Boolean(nullable: false),
                        IdDevis = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCompetence)
                .ForeignKey("dbo.DevisMinistere", t => t.IdDevis)
                .Index(t => t.IdDevis);
            
            CreateTable(
                "dbo.ContexteRealisation",
                c => new
                    {
                        IdContexte = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 300, unicode: false),
                        Numero = c.Int(nullable: false),
                        IdCompetence = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdContexte)
                .ForeignKey("dbo.EnonceCompetence", t => t.IdCompetence)
                .Index(t => t.IdCompetence);
            
            CreateTable(
                "dbo.ElementCompetence",
                c => new
                    {
                        IdElement = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 300, unicode: false),
                        Numero = c.Int(nullable: false),
                        IdCompetence = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdElement)
                .ForeignKey("dbo.EnonceCompetence", t => t.IdCompetence)
                .Index(t => t.IdCompetence);
            
            CreateTable(
                "dbo.CriterePerformance",
                c => new
                    {
                        IdCritere = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 300, unicode: false),
                        Numero = c.Int(nullable: false),
                        IdElement = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCritere)
                .ForeignKey("dbo.ElementCompetence", t => t.IdElement)
                .Index(t => t.IdElement);
            
            CreateTable(
                "dbo.PlanCadreElement",
                c => new
                    {
                        IdPlanCadreElement = c.Int(nullable: false, identity: true),
                        IdElement = c.Int(nullable: false),
                        IdPlanCadreCompetence = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPlanCadreElement)
                .ForeignKey("dbo.PlanCadreCompetence", t => t.IdPlanCadreCompetence)
                .ForeignKey("dbo.ElementCompetence", t => t.IdElement)
                .Index(t => t.IdElement)
                .Index(t => t.IdPlanCadreCompetence);
            
            CreateTable(
                "dbo.ActiviteApprentissage",
                c => new
                    {
                        IdActivite = c.Int(nullable: false, identity: true),
                        DescActivite = c.String(unicode: false, storeType: "text"),
                        IdPlanCadreElement = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdActivite)
                .ForeignKey("dbo.PlanCadreElement", t => t.IdPlanCadreElement)
                .Index(t => t.IdPlanCadreElement);
            
            CreateTable(
                "dbo.SousActiviteApprentissage",
                c => new
                    {
                        IdSousActivite = c.Int(nullable: false, identity: true),
                        NomSousActivite = c.String(unicode: false, storeType: "text"),
                        IdActivite = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdSousActivite)
                .ForeignKey("dbo.ActiviteApprentissage", t => t.IdActivite)
                .Index(t => t.IdActivite);
            
            CreateTable(
                "dbo.ElementConnaissance",
                c => new
                    {
                        IdElementConnaissance = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, unicode: false, storeType: "text"),
                        IdPlanCadreElement = c.Int(),
                    })
                .PrimaryKey(t => t.IdElementConnaissance)
                .ForeignKey("dbo.PlanCadreElement", t => t.IdPlanCadreElement)
                .Index(t => t.IdPlanCadreElement);
            
            CreateTable(
                "dbo.SousElementConnaissance",
                c => new
                    {
                        IdSousElement = c.Int(nullable: false, identity: true),
                        DescSousElement = c.String(nullable: false, unicode: false, storeType: "text"),
                        IdElementConnaissance = c.Int(),
                    })
                .PrimaryKey(t => t.IdSousElement)
                .ForeignKey("dbo.ElementConnaissance", t => t.IdElementConnaissance)
                .Index(t => t.IdElementConnaissance);
            
            CreateTable(
                "dbo.PlanCadreCompetence",
                c => new
                    {
                        IdPlanCadreCompetence = c.Int(nullable: false, identity: true),
                        PonderationEnHeure = c.Int(nullable: false),
                        IdPlanCadre = c.Int(nullable: false),
                        IdCompetence = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPlanCadreCompetence)
                .ForeignKey("dbo.PlanCadre", t => t.IdPlanCadre)
                .ForeignKey("dbo.EnonceCompetence", t => t.IdCompetence)
                .Index(t => t.IdPlanCadre)
                .Index(t => t.IdCompetence);
            
            CreateTable(
                "dbo.PlanCadre",
                c => new
                    {
                        IdPlanCadre = c.Int(nullable: false, identity: true),
                        NumeroCours = c.String(maxLength: 10, unicode: false),
                        TitreCours = c.String(nullable: false, maxLength: 150, unicode: false),
                        IndicationPedago = c.String(unicode: false, storeType: "text"),
                        NbHeureTheorie = c.Int(),
                        NbHeurePratique = c.Int(),
                        NbHeureDevoir = c.Int(),
                        IdProgramme = c.Int(nullable: false),
                        IdType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPlanCadre)
                .ForeignKey("dbo.Programme", t => t.IdProgramme)
                .ForeignKey("dbo.TypePlanCadre", t => t.IdType)
                .Index(t => t.IdProgramme)
                .Index(t => t.IdType);
            
            CreateTable(
                "dbo.Cours",
                c => new
                    {
                        IdCours = c.Int(nullable: false, identity: true),
                        IdPlanCadre = c.Int(nullable: false),
                        IdGrille = c.Int(nullable: false),
                        IdSession = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCours)
                .ForeignKey("dbo.GrilleCours", t => t.IdGrille)
                .ForeignKey("dbo.Session", t => t.IdSession)
                .ForeignKey("dbo.PlanCadre", t => t.IdPlanCadre)
                .Index(t => t.IdPlanCadre)
                .Index(t => t.IdGrille)
                .Index(t => t.IdSession);
            
            CreateTable(
                "dbo.GrilleCours",
                c => new
                    {
                        IdGrille = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.IdGrille);
            
            CreateTable(
                "dbo.PlanCours",
                c => new
                    {
                        IdPlanCours = c.Int(nullable: false, identity: true),
                        DateCreation = c.DateTime(nullable: false, storeType: "date"),
                        DateValidation = c.DateTime(storeType: "date"),
                        StatutPlanCours = c.Boolean(nullable: false),
                        IdCours = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPlanCours)
                .ForeignKey("dbo.Cours", t => t.IdCours)
                .Index(t => t.IdCours);
            
            CreateTable(
                "dbo.ContenuSection",
                c => new
                    {
                        IdContenuSection = c.Int(nullable: false, identity: true),
                        TexteContenu = c.String(nullable: false, maxLength: 3000),
                        IdNomSection = c.Int(nullable: false),
                        Modifiable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdContenuSection)
                .ForeignKey("dbo.NomSection", t => t.IdNomSection)
                .Index(t => t.IdNomSection);
            
            CreateTable(
                "dbo.NomSection",
                c => new
                    {
                        IdNomSection = c.Int(nullable: false, identity: true),
                        TitreSection = c.String(nullable: false, maxLength: 75, unicode: false),
                        Obligatoire = c.String(maxLength: 3, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.IdNomSection);
            
            CreateTable(
                "dbo.PlanCoursDepart",
                c => new
                    {
                        IdPlanCoursDepart = c.Int(nullable: false),
                        Discipline = c.String(nullable: false, maxLength: 3, fixedLength: true, unicode: false),
                        IdPlanCours = c.Int(nullable: false),
                        IdNomSection = c.Int(nullable: false),
                        TexteContenu = c.String(maxLength: 3000),
                        Actif = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.IdPlanCoursDepart, t.IdPlanCours, t.IdNomSection })
                .ForeignKey("dbo.NomSection", t => t.IdNomSection)
                .ForeignKey("dbo.PlanCours", t => t.IdPlanCours)
                .ForeignKey("dbo.Departement", t => t.Discipline)
                .Index(t => t.Discipline)
                .Index(t => t.IdPlanCours)
                .Index(t => t.IdNomSection);
            
            CreateTable(
                "dbo.PlanCoursUtilisateur",
                c => new
                    {
                        IdPlanCoursUtilisateur = c.String(nullable: false, maxLength: 128),
                        IdPlanCours = c.Int(nullable: false),
                        BureauProf = c.String(maxLength: 5, unicode: false),
                        Poste = c.String(maxLength: 4, unicode: false),
                    })
                .PrimaryKey(t => new { t.IdPlanCoursUtilisateur, t.IdPlanCours })
                .ForeignKey("dbo.AspNetUsers", t => t.IdPlanCoursUtilisateur)
                .ForeignKey("dbo.PlanCours", t => t.IdPlanCours)
                .Index(t => t.IdPlanCoursUtilisateur)
                .Index(t => t.IdPlanCours);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Nom = c.String(nullable: false, maxLength: 50),
                        Prenom = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvIder = c.String(nullable: false, maxLength: 128),
                        ProvIderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvIder, t.ProvIderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Session",
                c => new
                    {
                        IdSession = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false, maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.IdSession);
            
            CreateTable(
                "dbo.EnvironnementPhysique",
                c => new
                    {
                        IdEnvPhysique = c.Int(nullable: false, identity: true),
                        NomEnvPhys = c.String(unicode: false, storeType: "text"),
                        IdPlanCadre = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdEnvPhysique)
                .ForeignKey("dbo.PlanCadre", t => t.IdPlanCadre)
                .Index(t => t.IdPlanCadre);
            
            CreateTable(
                "dbo.SousEnvironnementPhysique",
                c => new
                    {
                        IdSousEnvPhys = c.Int(nullable: false, identity: true),
                        NomSousEnvPhys = c.String(unicode: false, storeType: "text"),
                        IdEnvPhysique = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdSousEnvPhys)
                .ForeignKey("dbo.EnvironnementPhysique", t => t.IdEnvPhysique)
                .Index(t => t.IdEnvPhysique);
            
            CreateTable(
                "dbo.PlanCadrePrealable",
                c => new
                    {
                        IdPlanCadrePrealable = c.Int(nullable: false, identity: true),
                        IdPlanCadre = c.Int(nullable: false),
                        IdPrealable = c.Int(nullable: false),
                        IdStatut = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPlanCadrePrealable)
                .ForeignKey("dbo.StatutPrealable", t => t.IdStatut)
                .ForeignKey("dbo.PlanCadre", t => t.IdPlanCadre)
                .Index(t => t.IdPlanCadre)
                .Index(t => t.IdStatut);
            
            CreateTable(
                "dbo.StatutPrealable",
                c => new
                    {
                        IdStatut = c.Int(nullable: false, identity: true),
                        Statut = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.IdStatut);
            
            CreateTable(
                "dbo.Programme",
                c => new
                    {
                        IdProgramme = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 50, unicode: false),
                        Annee = c.String(nullable: false, maxLength: 4, fixedLength: true, unicode: false),
                        DateValidation = c.DateTime(),
                        StatutStageValider = c.Boolean(),
                        NbSession = c.Int(nullable: false),
                        IdDevis = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdProgramme)
                .ForeignKey("dbo.DevisMinistere", t => t.IdDevis)
                .Index(t => t.IdDevis);
            
            CreateTable(
                "dbo.RessourceDIdactique",
                c => new
                    {
                        IdRessource = c.Int(nullable: false, identity: true),
                        NomRessource = c.String(unicode: false, storeType: "text"),
                        IdPlanCadre = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdRessource)
                .ForeignKey("dbo.PlanCadre", t => t.IdPlanCadre)
                .Index(t => t.IdPlanCadre);
            
            CreateTable(
                "dbo.SousRessourceDIdactique",
                c => new
                    {
                        IdSousRessource = c.Int(nullable: false, identity: true),
                        NomSousRessource = c.String(unicode: false, storeType: "text"),
                        IdRessource = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdSousRessource)
                .ForeignKey("dbo.RessourceDIdactique", t => t.IdRessource)
                .Index(t => t.IdRessource);
            
            CreateTable(
                "dbo.TypePlanCadre",
                c => new
                    {
                        IdType = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.IdType);
            
            CreateTable(
                "dbo.Application",
                c => new
                    {
                        IdApplication = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        Etudiant_IdEtudiant = c.Int(),
                        Stage_IdStage = c.Int(),
                    })
                .PrimaryKey(t => t.IdApplication)
                .ForeignKey("dbo.Etudiant", t => t.Etudiant_IdEtudiant)
                .ForeignKey("dbo.Stage", t => t.Stage_IdStage)
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
                        Preference_IdPreference1 = c.Int(),
                    })
                .PrimaryKey(t => t.IdEntreprise)
                .ForeignKey("dbo.Preference", t => t.Preference_IdPreference1)
                .Index(t => t.Preference_IdPreference1);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        IdContact = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false),
                        Telephone = c.String(nullable: false),
                        Courriel = c.String(nullable: false),
                        Entreprise_IdEntreprise = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdContact)
                .ForeignKey("dbo.Entreprise", t => t.Entreprise_IdEntreprise, cascadeDelete: true)
                .Index(t => t.Entreprise_IdEntreprise);
            
            CreateTable(
                "dbo.Stage",
                c => new
                    {
                        IdStage = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        CodePostal = c.String(nullable: false),
                        NumeroCivique = c.Int(nullable: false),
                        NomRue = c.String(nullable: false),
                        Ville = c.String(nullable: false),
                        Province = c.String(nullable: false),
                        Pays = c.String(nullable: false),
                        Salaire = c.Single(nullable: false),
                        NomDocument = c.String(),
                        Contact_IdContact = c.Int(),
                        Location_IdLocation = c.Int(),
                        Poste_IdPoste = c.Int(),
                        StatutStage_IdStatutStage = c.Int(),
                    })
                .PrimaryKey(t => t.IdStage)
                .ForeignKey("dbo.Location", t => t.Location_IdLocation)
                .ForeignKey("dbo.Poste", t => t.Poste_IdPoste)
                .ForeignKey("dbo.StatutStage", t => t.StatutStage_IdStatutStage)
                .ForeignKey("dbo.Contact", t => t.Contact_IdContact)
                .Index(t => t.Contact_IdContact)
                .Index(t => t.Location_IdLocation)
                .Index(t => t.Poste_IdPoste)
                .Index(t => t.StatutStage_IdStatutStage);
            
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
                        Preference_IdPreference1 = c.Int(),
                    })
                .PrimaryKey(t => t.IdPoste)
                .ForeignKey("dbo.Preference", t => t.Preference_IdPreference1)
                .Index(t => t.Preference_IdPreference1);
            
            CreateTable(
                "dbo.StatutStage",
                c => new
                    {
                        IdStatutStage = c.Int(nullable: false, identity: true),
                        StatutStage = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdStatutStage);
            
            CreateTable(
                "dbo.Caracteristiques",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NomCaracteristique = c.String(nullable: false),
                        IdJeu = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jeus", t => t.IdJeu, cascadeDelete: true)
                .Index(t => t.IdJeu);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NomItem = c.String(nullable: false),
                        IdCaracteristique = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Caracteristiques", t => t.IdCaracteristique, cascadeDelete: true)
                .Index(t => t.IdCaracteristique);
            
            CreateTable(
                "dbo.Joueurs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PseudoJoueur = c.String(nullable: false),
                        IdMembreESports = c.String(nullable: false, maxLength: 128),
                        IdProfil = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Profils", t => t.IdProfil)
                .ForeignKey("dbo.MembreESports", t => t.IdMembreESports, cascadeDelete: true)
                .Index(t => t.IdMembreESports)
                .Index(t => t.IdProfil);
            
            CreateTable(
                "dbo.Equipes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NomEquipe = c.String(nullable: false),
                        EstMonoJoueur = c.Boolean(nullable: false),
                        IdJeu = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jeus", t => t.IdJeu, cascadeDelete: true)
                .Index(t => t.IdJeu);
            
            CreateTable(
                "dbo.Entraineurs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NomEntraineur = c.String(nullable: false),
                        PrenomEntraineur = c.String(nullable: false),
                        PseudoEntraineur = c.String(nullable: false, maxLength: 25),
                        NumeroTelephone = c.String(nullable: false),
                        AdresseCourriel = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Jeus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NomJeu = c.String(nullable: false),
                        Description = c.String(),
                        UrlReference = c.String(),
                        Abreviation = c.String(nullable: false, maxLength: 6),
                        IdStatut = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Statuts", t => t.IdStatut, cascadeDelete: true)
                .Index(t => t.IdStatut);
            
            CreateTable(
                "dbo.Profils",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Pseudo = c.String(nullable: false, maxLength: 25),
                        Courriel = c.String(nullable: false),
                        Note = c.String(),
                        EstArchive = c.Boolean(nullable: false),
                        IdMembreESports = c.String(nullable: false, maxLength: 128),
                        IdJeu = c.Int(nullable: false),
                        IdJeuSecondaire = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MembreESports", t => t.IdMembreESports, cascadeDelete: true)
                .ForeignKey("dbo.Jeus", t => t.IdJeu, cascadeDelete: true)
                .Index(t => t.IdMembreESports)
                .Index(t => t.IdJeu);
            
            CreateTable(
                "dbo.MembreESports",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Nom = c.String(),
                        Prenom = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rangs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NomRang = c.String(nullable: false),
                        Abreviation = c.String(nullable: false, maxLength: 6),
                        Hierarchie = c.Int(nullable: false),
                        IdJeu = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jeus", t => t.IdJeu, cascadeDelete: true)
                .Index(t => t.IdJeu);
            
            CreateTable(
                "dbo.HistoriqueRangs",
                c => new
                    {
                        IdJoueur = c.Int(nullable: false),
                        IdRang = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdJoueur, t.IdRang })
                .ForeignKey("dbo.Rangs", t => t.IdRang, cascadeDelete: true)
                .ForeignKey("dbo.Joueurs", t => t.IdJoueur, cascadeDelete: true)
                .Index(t => t.IdJoueur)
                .Index(t => t.IdRang);
            
            CreateTable(
                "dbo.Statuts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NomStatut = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LieuDeLaReunion",
                c => new
                    {
                        IdLieu = c.Int(nullable: false, identity: true),
                        EmplacementReunion = c.String(),
                    })
                .PrimaryKey(t => t.IdLieu);
            
            CreateTable(
                "dbo.ModeleOrdreDuJour",
                c => new
                    {
                        IdModele = c.Int(nullable: false, identity: true),
                        Role = c.String(nullable: false, maxLength: 3, unicode: false),
                        NumeroProgramme = c.Int(nullable: false),
                        PointPrincipal = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.IdModele);
            
            CreateTable(
                "dbo.OrdreDuJour",
                c => new
                    {
                        IdOdJ = c.Int(nullable: false, identity: true),
                        TitreOdJ = c.String(nullable: false, maxLength: 50, unicode: false),
                        DateOdJ = c.DateTime(nullable: false, storeType: "date"),
                        HeureDebutReunion = c.String(nullable: false, maxLength: 100, unicode: false),
                        HeureFinReunion = c.String(nullable: false, maxLength: 100, unicode: false),
                        LieuReunionODJ = c.String(maxLength: 100, unicode: false),
                        IdModeleOrdreDuJour = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdOdJ);
            
            CreateTable(
                "dbo.SujetPointPrincipal",
                c => new
                    {
                        IdPointPrincipal = c.Int(nullable: false, identity: true),
                        SujetPoint = c.String(nullable: false, maxLength: 100, unicode: false),
                        PositionPP = c.Int(),
                        IdOrdreDuJour = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPointPrincipal)
                .ForeignKey("dbo.OrdreDuJour", t => t.IdOrdreDuJour)
                .Index(t => t.IdOrdreDuJour);
            
            CreateTable(
                "dbo.SousPointSujet",
                c => new
                    {
                        IdSousPoint = c.Int(nullable: false, identity: true),
                        SujetSousPoint = c.String(nullable: false, maxLength: 100, unicode: false),
                        IdSujetPointPrincipal = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdSousPoint)
                .ForeignKey("dbo.SujetPointPrincipal", t => t.IdSujetPointPrincipal)
                .Index(t => t.IdSujetPointPrincipal);
            
            CreateTable(
                "dbo.sysdiagrams",
                c => new
                    {
                        diagram_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 128),
                        principal_id = c.Int(nullable: false),
                        version = c.Int(),
                        definition = c.Binary(),
                    })
                .PrimaryKey(t => t.diagram_id);
            
            CreateTable(
                "dbo.TexteSections",
                c => new
                    {
                        IdPlanCours = c.Int(nullable: false),
                        IdContenuSection = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdPlanCours, t.IdContenuSection })
                .ForeignKey("dbo.ContenuSection", t => t.IdContenuSection, cascadeDelete: true)
                .ForeignKey("dbo.PlanCours", t => t.IdPlanCours, cascadeDelete: true)
                .Index(t => t.IdPlanCours)
                .Index(t => t.IdContenuSection);
            
            CreateTable(
                "dbo.TexteSection",
                c => new
                    {
                        idContenuSection = c.Int(nullable: false),
                        idPlanCours = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.idContenuSection, t.idPlanCours })
                .ForeignKey("dbo.ContenuSection", t => t.idContenuSection, cascadeDelete: true)
                .ForeignKey("dbo.PlanCours", t => t.idPlanCours, cascadeDelete: true)
                .Index(t => t.idContenuSection)
                .Index(t => t.idPlanCours);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.EntraineurEquipe",
                c => new
                    {
                        IdEntraineur = c.Int(nullable: false),
                        IdEquipe = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdEntraineur, t.IdEquipe })
                .ForeignKey("dbo.Entraineurs", t => t.IdEntraineur, cascadeDelete: true)
                .ForeignKey("dbo.Equipes", t => t.IdEquipe, cascadeDelete: true)
                .Index(t => t.IdEntraineur)
                .Index(t => t.IdEquipe);
            
            CreateTable(
                "dbo.EquipeJoueurs",
                c => new
                    {
                        IdEquipe = c.Int(nullable: false),
                        IdJoueur = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdEquipe, t.IdJoueur })
                .ForeignKey("dbo.Equipes", t => t.IdEquipe, cascadeDelete: true)
                .ForeignKey("dbo.Joueurs", t => t.IdJoueur, cascadeDelete: true)
                .Index(t => t.IdEquipe)
                .Index(t => t.IdJoueur);
            
            CreateTable(
                "dbo.JoueurItems",
                c => new
                    {
                        IdItem = c.Int(nullable: false),
                        IdJoueur = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdItem, t.IdJoueur })
                .ForeignKey("dbo.Items", t => t.IdItem, cascadeDelete: true)
                .ForeignKey("dbo.Joueurs", t => t.IdJoueur, cascadeDelete: true)
                .Index(t => t.IdItem)
                .Index(t => t.IdJoueur);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TexteSections", "IdPlanCours", "dbo.PlanCours");
            DropForeignKey("dbo.TexteSections", "IdContenuSection", "dbo.ContenuSection");
            DropForeignKey("dbo.SujetPointPrincipal", "IdOrdreDuJour", "dbo.OrdreDuJour");
            DropForeignKey("dbo.SousPointSujet", "IdSujetPointPrincipal", "dbo.SujetPointPrincipal");
            DropForeignKey("dbo.Items", "IdCaracteristique", "dbo.Caracteristiques");
            DropForeignKey("dbo.JoueurItems", "IdJoueur", "dbo.Joueurs");
            DropForeignKey("dbo.JoueurItems", "IdItem", "dbo.Items");
            DropForeignKey("dbo.HistoriqueRangs", "IdJoueur", "dbo.Joueurs");
            DropForeignKey("dbo.EquipeJoueurs", "IdJoueur", "dbo.Joueurs");
            DropForeignKey("dbo.EquipeJoueurs", "IdEquipe", "dbo.Equipes");
            DropForeignKey("dbo.Jeus", "IdStatut", "dbo.Statuts");
            DropForeignKey("dbo.Rangs", "IdJeu", "dbo.Jeus");
            DropForeignKey("dbo.HistoriqueRangs", "IdRang", "dbo.Rangs");
            DropForeignKey("dbo.Profils", "IdJeu", "dbo.Jeus");
            DropForeignKey("dbo.Profils", "IdMembreESports", "dbo.MembreESports");
            DropForeignKey("dbo.Joueurs", "IdMembreESports", "dbo.MembreESports");
            DropForeignKey("dbo.Joueurs", "IdProfil", "dbo.Profils");
            DropForeignKey("dbo.Equipes", "IdJeu", "dbo.Jeus");
            DropForeignKey("dbo.Caracteristiques", "IdJeu", "dbo.Jeus");
            DropForeignKey("dbo.EntraineurEquipe", "IdEquipe", "dbo.Equipes");
            DropForeignKey("dbo.EntraineurEquipe", "IdEntraineur", "dbo.Entraineurs");
            DropForeignKey("dbo.Poste", "Preference_IdPreference1", "dbo.Preference");
            DropForeignKey("dbo.Location", "Preference_IdPreference", "dbo.Preference");
            DropForeignKey("dbo.Etudiant", "Preference_IdPreference", "dbo.Preference");
            DropForeignKey("dbo.Entreprise", "Preference_IdPreference1", "dbo.Preference");
            DropForeignKey("dbo.Contact", "Entreprise_IdEntreprise", "dbo.Entreprise");
            DropForeignKey("dbo.Stage", "Contact_IdContact", "dbo.Contact");
            DropForeignKey("dbo.Stage", "StatutStage_IdStatutStage", "dbo.StatutStage");
            DropForeignKey("dbo.Stage", "Poste_IdPoste", "dbo.Poste");
            DropForeignKey("dbo.Stage", "Location_IdLocation", "dbo.Location");
            DropForeignKey("dbo.Application", "Stage_IdStage", "dbo.Stage");
            DropForeignKey("dbo.Application", "Etudiant_IdEtudiant", "dbo.Etudiant");
            DropForeignKey("dbo.PlanCoursDepart", "Discipline", "dbo.Departement");
            DropForeignKey("dbo.DevisMinistere", "Discipline", "dbo.Departement");
            DropForeignKey("dbo.Programme", "IdDevis", "dbo.DevisMinistere");
            DropForeignKey("dbo.EnonceCompetence", "IdDevis", "dbo.DevisMinistere");
            DropForeignKey("dbo.PlanCadreCompetence", "IdCompetence", "dbo.EnonceCompetence");
            DropForeignKey("dbo.ElementCompetence", "IdCompetence", "dbo.EnonceCompetence");
            DropForeignKey("dbo.PlanCadreElement", "IdElement", "dbo.ElementCompetence");
            DropForeignKey("dbo.PlanCadreElement", "IdPlanCadreCompetence", "dbo.PlanCadreCompetence");
            DropForeignKey("dbo.PlanCadre", "IdType", "dbo.TypePlanCadre");
            DropForeignKey("dbo.RessourceDIdactique", "IdPlanCadre", "dbo.PlanCadre");
            DropForeignKey("dbo.SousRessourceDIdactique", "IdRessource", "dbo.RessourceDIdactique");
            DropForeignKey("dbo.PlanCadre", "IdProgramme", "dbo.Programme");
            DropForeignKey("dbo.PlanCadrePrealable", "IdPlanCadre", "dbo.PlanCadre");
            DropForeignKey("dbo.PlanCadrePrealable", "IdStatut", "dbo.StatutPrealable");
            DropForeignKey("dbo.PlanCadreCompetence", "IdPlanCadre", "dbo.PlanCadre");
            DropForeignKey("dbo.EnvironnementPhysique", "IdPlanCadre", "dbo.PlanCadre");
            DropForeignKey("dbo.SousEnvironnementPhysique", "IdEnvPhysique", "dbo.EnvironnementPhysique");
            DropForeignKey("dbo.Cours", "IdPlanCadre", "dbo.PlanCadre");
            DropForeignKey("dbo.Cours", "IdSession", "dbo.Session");
            DropForeignKey("dbo.PlanCours", "IdCours", "dbo.Cours");
            DropForeignKey("dbo.PlanCoursUtilisateur", "IdPlanCours", "dbo.PlanCours");
            DropForeignKey("dbo.PlanCoursUtilisateur", "IdPlanCoursUtilisateur", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.PlanCoursDepart", "IdPlanCours", "dbo.PlanCours");
            DropForeignKey("dbo.TexteSection", "idPlanCours", "dbo.PlanCours");
            DropForeignKey("dbo.TexteSection", "idContenuSection", "dbo.ContenuSection");
            DropForeignKey("dbo.PlanCoursDepart", "IdNomSection", "dbo.NomSection");
            DropForeignKey("dbo.ContenuSection", "IdNomSection", "dbo.NomSection");
            DropForeignKey("dbo.Cours", "IdGrille", "dbo.GrilleCours");
            DropForeignKey("dbo.SousElementConnaissance", "IdElementConnaissance", "dbo.ElementConnaissance");
            DropForeignKey("dbo.ElementConnaissance", "IdPlanCadreElement", "dbo.PlanCadreElement");
            DropForeignKey("dbo.ActiviteApprentissage", "IdPlanCadreElement", "dbo.PlanCadreElement");
            DropForeignKey("dbo.SousActiviteApprentissage", "IdActivite", "dbo.ActiviteApprentissage");
            DropForeignKey("dbo.CriterePerformance", "IdElement", "dbo.ElementCompetence");
            DropForeignKey("dbo.ContexteRealisation", "IdCompetence", "dbo.EnonceCompetence");
            DropForeignKey("dbo.AccesProgramme", "Discipline", "dbo.Departement");
            DropIndex("dbo.JoueurItems", new[] { "IdJoueur" });
            DropIndex("dbo.JoueurItems", new[] { "IdItem" });
            DropIndex("dbo.EquipeJoueurs", new[] { "IdJoueur" });
            DropIndex("dbo.EquipeJoueurs", new[] { "IdEquipe" });
            DropIndex("dbo.EntraineurEquipe", new[] { "IdEquipe" });
            DropIndex("dbo.EntraineurEquipe", new[] { "IdEntraineur" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.TexteSection", new[] { "idPlanCours" });
            DropIndex("dbo.TexteSection", new[] { "idContenuSection" });
            DropIndex("dbo.TexteSections", new[] { "IdContenuSection" });
            DropIndex("dbo.TexteSections", new[] { "IdPlanCours" });
            DropIndex("dbo.SousPointSujet", new[] { "IdSujetPointPrincipal" });
            DropIndex("dbo.SujetPointPrincipal", new[] { "IdOrdreDuJour" });
            DropIndex("dbo.HistoriqueRangs", new[] { "IdRang" });
            DropIndex("dbo.HistoriqueRangs", new[] { "IdJoueur" });
            DropIndex("dbo.Rangs", new[] { "IdJeu" });
            DropIndex("dbo.Profils", new[] { "IdJeu" });
            DropIndex("dbo.Profils", new[] { "IdMembreESports" });
            DropIndex("dbo.Jeus", new[] { "IdStatut" });
            DropIndex("dbo.Equipes", new[] { "IdJeu" });
            DropIndex("dbo.Joueurs", new[] { "IdProfil" });
            DropIndex("dbo.Joueurs", new[] { "IdMembreESports" });
            DropIndex("dbo.Items", new[] { "IdCaracteristique" });
            DropIndex("dbo.Caracteristiques", new[] { "IdJeu" });
            DropIndex("dbo.Poste", new[] { "Preference_IdPreference1" });
            DropIndex("dbo.Location", new[] { "Preference_IdPreference" });
            DropIndex("dbo.Stage", new[] { "StatutStage_IdStatutStage" });
            DropIndex("dbo.Stage", new[] { "Poste_IdPoste" });
            DropIndex("dbo.Stage", new[] { "Location_IdLocation" });
            DropIndex("dbo.Stage", new[] { "Contact_IdContact" });
            DropIndex("dbo.Contact", new[] { "Entreprise_IdEntreprise" });
            DropIndex("dbo.Entreprise", new[] { "Preference_IdPreference1" });
            DropIndex("dbo.Etudiant", new[] { "Preference_IdPreference" });
            DropIndex("dbo.Application", new[] { "Stage_IdStage" });
            DropIndex("dbo.Application", new[] { "Etudiant_IdEtudiant" });
            DropIndex("dbo.SousRessourceDIdactique", new[] { "IdRessource" });
            DropIndex("dbo.RessourceDIdactique", new[] { "IdPlanCadre" });
            DropIndex("dbo.Programme", new[] { "IdDevis" });
            DropIndex("dbo.PlanCadrePrealable", new[] { "IdStatut" });
            DropIndex("dbo.PlanCadrePrealable", new[] { "IdPlanCadre" });
            DropIndex("dbo.SousEnvironnementPhysique", new[] { "IdEnvPhysique" });
            DropIndex("dbo.EnvironnementPhysique", new[] { "IdPlanCadre" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.PlanCoursUtilisateur", new[] { "IdPlanCours" });
            DropIndex("dbo.PlanCoursUtilisateur", new[] { "IdPlanCoursUtilisateur" });
            DropIndex("dbo.PlanCoursDepart", new[] { "IdNomSection" });
            DropIndex("dbo.PlanCoursDepart", new[] { "IdPlanCours" });
            DropIndex("dbo.PlanCoursDepart", new[] { "Discipline" });
            DropIndex("dbo.ContenuSection", new[] { "IdNomSection" });
            DropIndex("dbo.PlanCours", new[] { "IdCours" });
            DropIndex("dbo.Cours", new[] { "IdSession" });
            DropIndex("dbo.Cours", new[] { "IdGrille" });
            DropIndex("dbo.Cours", new[] { "IdPlanCadre" });
            DropIndex("dbo.PlanCadre", new[] { "IdType" });
            DropIndex("dbo.PlanCadre", new[] { "IdProgramme" });
            DropIndex("dbo.PlanCadreCompetence", new[] { "IdCompetence" });
            DropIndex("dbo.PlanCadreCompetence", new[] { "IdPlanCadre" });
            DropIndex("dbo.SousElementConnaissance", new[] { "IdElementConnaissance" });
            DropIndex("dbo.ElementConnaissance", new[] { "IdPlanCadreElement" });
            DropIndex("dbo.SousActiviteApprentissage", new[] { "IdActivite" });
            DropIndex("dbo.ActiviteApprentissage", new[] { "IdPlanCadreElement" });
            DropIndex("dbo.PlanCadreElement", new[] { "IdPlanCadreCompetence" });
            DropIndex("dbo.PlanCadreElement", new[] { "IdElement" });
            DropIndex("dbo.CriterePerformance", new[] { "IdElement" });
            DropIndex("dbo.ElementCompetence", new[] { "IdCompetence" });
            DropIndex("dbo.ContexteRealisation", new[] { "IdCompetence" });
            DropIndex("dbo.EnonceCompetence", new[] { "IdDevis" });
            DropIndex("dbo.DevisMinistere", new[] { "Discipline" });
            DropIndex("dbo.AccesProgramme", new[] { "Discipline" });
            DropTable("dbo.JoueurItems");
            DropTable("dbo.EquipeJoueurs");
            DropTable("dbo.EntraineurEquipe");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.TexteSection");
            DropTable("dbo.TexteSections");
            DropTable("dbo.sysdiagrams");
            DropTable("dbo.SousPointSujet");
            DropTable("dbo.SujetPointPrincipal");
            DropTable("dbo.OrdreDuJour");
            DropTable("dbo.ModeleOrdreDuJour");
            DropTable("dbo.LieuDeLaReunion");
            DropTable("dbo.Statuts");
            DropTable("dbo.HistoriqueRangs");
            DropTable("dbo.Rangs");
            DropTable("dbo.MembreESports");
            DropTable("dbo.Profils");
            DropTable("dbo.Jeus");
            DropTable("dbo.Entraineurs");
            DropTable("dbo.Equipes");
            DropTable("dbo.Joueurs");
            DropTable("dbo.Items");
            DropTable("dbo.Caracteristiques");
            DropTable("dbo.StatutStage");
            DropTable("dbo.Poste");
            DropTable("dbo.Location");
            DropTable("dbo.Stage");
            DropTable("dbo.Contact");
            DropTable("dbo.Entreprise");
            DropTable("dbo.Preference");
            DropTable("dbo.Etudiant");
            DropTable("dbo.Application");
            DropTable("dbo.TypePlanCadre");
            DropTable("dbo.SousRessourceDIdactique");
            DropTable("dbo.RessourceDIdactique");
            DropTable("dbo.Programme");
            DropTable("dbo.StatutPrealable");
            DropTable("dbo.PlanCadrePrealable");
            DropTable("dbo.SousEnvironnementPhysique");
            DropTable("dbo.EnvironnementPhysique");
            DropTable("dbo.Session");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.PlanCoursUtilisateur");
            DropTable("dbo.PlanCoursDepart");
            DropTable("dbo.NomSection");
            DropTable("dbo.ContenuSection");
            DropTable("dbo.PlanCours");
            DropTable("dbo.GrilleCours");
            DropTable("dbo.Cours");
            DropTable("dbo.PlanCadre");
            DropTable("dbo.PlanCadreCompetence");
            DropTable("dbo.SousElementConnaissance");
            DropTable("dbo.ElementConnaissance");
            DropTable("dbo.SousActiviteApprentissage");
            DropTable("dbo.ActiviteApprentissage");
            DropTable("dbo.PlanCadreElement");
            DropTable("dbo.CriterePerformance");
            DropTable("dbo.ElementCompetence");
            DropTable("dbo.ContexteRealisation");
            DropTable("dbo.EnonceCompetence");
            DropTable("dbo.DevisMinistere");
            DropTable("dbo.Departement");
            DropTable("dbo.AccesProgramme");
        }
    }
}

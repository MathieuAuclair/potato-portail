namespace SysInternshipManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccesProgramme",
                c => new
                    {
                        idAcces = c.Int(nullable: false, identity: true),
                        userMail = c.String(nullable: false, maxLength: 256),
                        codeProgramme = c.String(nullable: false, maxLength: 3, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => new { t.idAcces, t.userMail, t.codeProgramme })
                .ForeignKey("dbo.EnteteProgramme", t => t.codeProgramme, cascadeDelete: true)
                .Index(t => t.codeProgramme);
            
            CreateTable(
                "dbo.EnteteProgramme",
                c => new
                    {
                        codeProgramme = c.String(nullable: false, maxLength: 3, fixedLength: true, unicode: false),
                        nom = c.String(nullable: false, maxLength: 200, unicode: false),
                    })
                .PrimaryKey(t => t.codeProgramme);
            
            CreateTable(
                "dbo.DevisMinistere",
                c => new
                    {
                        idDevis = c.Int(nullable: false, identity: true),
                        annee = c.String(nullable: false, maxLength: 4, fixedLength: true, unicode: false),
                        codeSpecialisation = c.String(nullable: false, maxLength: 3, unicode: false),
                        specialisation = c.String(maxLength: 30, unicode: false),
                        nbUnite = c.String(maxLength: 8),
                        nbHeureFrmGenerale = c.Int(),
                        nbHeureFrmSpecifique = c.Int(),
                        condition = c.String(maxLength: 300, unicode: false),
                        sanction = c.String(maxLength: 50, unicode: false),
                        docMinistere = c.String(maxLength: 200, unicode: false),
                        codeProgramme = c.String(nullable: false, maxLength: 3, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.idDevis)
                .ForeignKey("dbo.EnteteProgramme", t => t.codeProgramme)
                .Index(t => t.codeProgramme);
            
            CreateTable(
                "dbo.EnonceCompetence",
                c => new
                    {
                        idCompetence = c.Int(nullable: false, identity: true),
                        codeCompetence = c.String(nullable: false, maxLength: 4, unicode: false),
                        description = c.String(nullable: false, maxLength: 300, unicode: false),
                        obligatoire = c.Boolean(nullable: false),
                        actif = c.Boolean(nullable: false),
                        specifique = c.Boolean(nullable: false),
                        idDevis = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.idCompetence)
                .ForeignKey("dbo.DevisMinistere", t => t.idDevis)
                .Index(t => t.idDevis);
            
            CreateTable(
                "dbo.ContexteRealisation",
                c => new
                    {
                        idContexte = c.Int(nullable: false, identity: true),
                        description = c.String(nullable: false, maxLength: 300, unicode: false),
                        numero = c.Int(nullable: false),
                        idCompetence = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.idContexte)
                .ForeignKey("dbo.EnonceCompetence", t => t.idCompetence)
                .Index(t => t.idCompetence);
            
            CreateTable(
                "dbo.ElementCompetence",
                c => new
                    {
                        idElement = c.Int(nullable: false, identity: true),
                        description = c.String(nullable: false, maxLength: 300, unicode: false),
                        numero = c.Int(nullable: false),
                        idCompetence = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.idElement)
                .ForeignKey("dbo.EnonceCompetence", t => t.idCompetence)
                .Index(t => t.idCompetence);
            
            CreateTable(
                "dbo.CriterePerformance",
                c => new
                    {
                        idCritere = c.Int(nullable: false, identity: true),
                        description = c.String(nullable: false, maxLength: 300, unicode: false),
                        numero = c.Int(nullable: false),
                        idElement = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.idCritere)
                .ForeignKey("dbo.ElementCompetence", t => t.idElement)
                .Index(t => t.idElement);
            
            CreateTable(
                "dbo.PlanCadreElement",
                c => new
                    {
                        idPlanCadreElement = c.Int(nullable: false),
                        idPlanCadre = c.Int(nullable: false),
                        idElement = c.Int(nullable: false),
                        idElementConnaissance = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.idPlanCadreElement, t.idPlanCadre, t.idElement, t.idElementConnaissance })
                .ForeignKey("dbo.ElementConnaissance", t => t.idElementConnaissance)
                .ForeignKey("dbo.PlanCadre", t => t.idPlanCadre)
                .ForeignKey("dbo.ElementCompetence", t => t.idElement)
                .Index(t => t.idPlanCadre)
                .Index(t => t.idElement)
                .Index(t => t.idElementConnaissance);
            
            CreateTable(
                "dbo.ElementConnaissance",
                c => new
                    {
                        idElementConnaissance = c.Int(nullable: false, identity: true),
                        description = c.String(nullable: false, unicode: false, storeType: "text"),
                        idActiviteApprentissage = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.idElementConnaissance)
                .ForeignKey("dbo.ActiviteApprentissage", t => t.idActiviteApprentissage)
                .Index(t => t.idActiviteApprentissage);
            
            CreateTable(
                "dbo.ActiviteApprentissage",
                c => new
                    {
                        idActiviteApprentissage = c.Int(nullable: false, identity: true),
                        description = c.String(nullable: false, unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.idActiviteApprentissage);
            
            CreateTable(
                "dbo.PlanCadre",
                c => new
                    {
                        idPlanCadre = c.Int(nullable: false, identity: true),
                        numeroCours = c.String(maxLength: 10, unicode: false),
                        titreCours = c.String(nullable: false, maxLength: 150, unicode: false),
                        indicationPedago = c.String(unicode: false, storeType: "text"),
                        environnementPhys = c.String(unicode: false, storeType: "text"),
                        ressource = c.String(unicode: false, storeType: "text"),
                        nbHeureTheorie = c.Int(),
                        nbHeurePratique = c.Int(),
                        nbHeureDevoir = c.Int(),
                        idProgramme = c.Int(nullable: false),
                        idType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.idPlanCadre)
                .ForeignKey("dbo.Programme", t => t.idProgramme)
                .ForeignKey("dbo.TypePlanCadre", t => t.idType)
                .Index(t => t.idProgramme)
                .Index(t => t.idType);
            
            CreateTable(
                "dbo.Cours",
                c => new
                    {
                        idCours = c.Int(nullable: false, identity: true),
                        idPlanCadre = c.Int(nullable: false),
                        idGrille = c.Int(nullable: false),
                        idSession = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.idCours)
                .ForeignKey("dbo.GrilleCours", t => t.idGrille)
                .ForeignKey("dbo.Session", t => t.idSession)
                .ForeignKey("dbo.PlanCadre", t => t.idPlanCadre)
                .Index(t => t.idPlanCadre)
                .Index(t => t.idGrille)
                .Index(t => t.idSession);
            
            CreateTable(
                "dbo.GrilleCours",
                c => new
                    {
                        idGrille = c.Int(nullable: false, identity: true),
                        nom = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.idGrille);
            
            CreateTable(
                "dbo.Session",
                c => new
                    {
                        idSession = c.Int(nullable: false, identity: true),
                        nom = c.String(nullable: false, maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.idSession);
            
            CreateTable(
                "dbo.PlanCadreEnonce",
                c => new
                    {
                        idPlanCadreEnonce = c.Int(nullable: false),
                        ponderationEnHeure = c.Int(nullable: false),
                        idPlanCadre = c.Int(nullable: false),
                        idCompetence = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.idPlanCadreEnonce, t.ponderationEnHeure, t.idPlanCadre, t.idCompetence })
                .ForeignKey("dbo.PlanCadre", t => t.idPlanCadre)
                .ForeignKey("dbo.EnonceCompetence", t => t.idCompetence)
                .Index(t => t.idPlanCadre)
                .Index(t => t.idCompetence);
            
            CreateTable(
                "dbo.PlanCadrePrealable",
                c => new
                    {
                        idPlanCadrePrealable = c.Int(nullable: false, identity: true),
                        idPlanCadre = c.Int(nullable: false),
                        idPrealable = c.Int(nullable: false),
                        idStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.idPlanCadrePrealable)
                .ForeignKey("dbo.StatusPrealable", t => t.idStatus)
                .ForeignKey("dbo.PlanCadre", t => t.idPlanCadre)
                .ForeignKey("dbo.PlanCadre", t => t.idPrealable)
                .Index(t => t.idPlanCadre)
                .Index(t => t.idPrealable)
                .Index(t => t.idStatus);
            
            CreateTable(
                "dbo.StatusPrealable",
                c => new
                    {
                        idStatus = c.Int(nullable: false, identity: true),
                        status = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.idStatus);
            
            CreateTable(
                "dbo.Programme",
                c => new
                    {
                        idProgramme = c.Int(nullable: false, identity: true),
                        nom = c.String(nullable: false, maxLength: 50, unicode: false),
                        nbSession = c.Int(nullable: false),
                        annee = c.String(nullable: false, maxLength: 4, fixedLength: true, unicode: false),
                        dateValidation = c.DateTime(),
                        statusValider = c.Boolean(nullable: false),
                        idDevis = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.idProgramme)
                .ForeignKey("dbo.DevisMinistere", t => t.idDevis)
                .Index(t => t.idDevis);
            
            CreateTable(
                "dbo.TypePlanCadre",
                c => new
                    {
                        idType = c.Int(nullable: false, identity: true),
                        nom = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.idType);
            
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
                        Entreprise_IdEntreprise = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdContact)
                .ForeignKey("dbo.Entreprise", t => t.Entreprise_IdEntreprise, cascadeDelete: true)
                .Index(t => t.Entreprise_IdEntreprise);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        IdStatus = c.Int(nullable: false, identity: true),
                        StatusStage = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdStatus);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        nom = c.String(nullable: false, maxLength: 50),
                        prenom = c.String(nullable: false, maxLength: 50),
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
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Caracteristiques",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nomCaracteristique = c.String(nullable: false),
                        JeuId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Jeux", t => t.JeuId, cascadeDelete: true)
                .Index(t => t.JeuId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nomItem = c.String(nullable: false),
                        CaracteristiqueId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Caracteristiques", t => t.CaracteristiqueId, cascadeDelete: true)
                .Index(t => t.CaracteristiqueId);
            
            CreateTable(
                "dbo.Joueurs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PseudoJoueur = c.String(nullable: false),
                        EtudiantId = c.Int(nullable: false),
                        ProfilId = c.Int(nullable: false),
                        Etudiant_IdEtudiant = c.Int(),
                        MembreEsport_IdMembreEsport = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Profils", t => t.ProfilId)
                .ForeignKey("dbo.Etudiant", t => t.Etudiant_IdEtudiant)
                .ForeignKey("dbo.Etudiants", t => t.MembreEsport_IdMembreEsport)
                .Index(t => t.ProfilId)
                .Index(t => t.Etudiant_IdEtudiant)
                .Index(t => t.MembreEsport_IdMembreEsport);
            
            CreateTable(
                "dbo.Equipes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nomEquipe = c.String(nullable: false),
                        estMonojoueur = c.Boolean(nullable: false),
                        JeuId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Jeux", t => t.JeuId, cascadeDelete: true)
                .Index(t => t.JeuId);
            
            CreateTable(
                "dbo.Entraineurs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nomEntraineur = c.String(nullable: false),
                        prenomEntraineur = c.String(nullable: false),
                        pseudoEntraineur = c.String(nullable: false),
                        numTel = c.String(nullable: false),
                        adresseCourriel = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Jeux",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nomJeu = c.String(nullable: false),
                        description = c.String(),
                        urlReference = c.String(),
                        abreviation = c.String(nullable: false, maxLength: 6),
                        StatutId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Statuts", t => t.StatutId, cascadeDelete: true)
                .Index(t => t.StatutId);
            
            CreateTable(
                "dbo.Profils",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Pseudo = c.String(nullable: false, maxLength: 25),
                        Courriel = c.String(nullable: false),
                        Note = c.String(),
                        EstArchive = c.Boolean(nullable: false),
                        EtudiantId = c.Int(nullable: false),
                        JeuId = c.Int(nullable: false),
                        JeuSecondaireId = c.Int(),
                        Etudiant_IdEtudiant = c.Int(),
                        MembreEsport_IdMembreEsport = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Etudiant", t => t.Etudiant_IdEtudiant)
                .ForeignKey("dbo.Jeux", t => t.JeuId, cascadeDelete: true)
                .ForeignKey("dbo.Etudiants", t => t.MembreEsport_IdMembreEsport)
                .Index(t => t.JeuId)
                .Index(t => t.Etudiant_IdEtudiant)
                .Index(t => t.MembreEsport_IdMembreEsport);
            
            CreateTable(
                "dbo.Rangs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nomRang = c.String(nullable: false),
                        abreviation = c.String(nullable: false, maxLength: 6),
                        hierarchie = c.Int(nullable: false),
                        JeuId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Jeux", t => t.JeuId, cascadeDelete: true)
                .Index(t => t.JeuId);
            
            CreateTable(
                "dbo.HistoriqueRang",
                c => new
                    {
                        JoueurId = c.Int(nullable: false),
                        RangId = c.Int(nullable: false),
                        date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.JoueurId, t.RangId })
                .ForeignKey("dbo.Joueurs", t => t.JoueurId, cascadeDelete: true)
                .ForeignKey("dbo.Rangs", t => t.RangId, cascadeDelete: true)
                .Index(t => t.JoueurId)
                .Index(t => t.RangId);
            
            CreateTable(
                "dbo.Statuts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nomStatut = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.lieuDeLaReunion",
                c => new
                    {
                        idLieu = c.Int(nullable: false, identity: true),
                        emplacementReunion = c.String(),
                    })
                .PrimaryKey(t => t.idLieu);
            
            CreateTable(
                "dbo.Etudiants",
                c => new
                    {
                        IdMembreEsport = c.Int(nullable: false, identity: true),
                        Etudiant_IdEtudiant = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdMembreEsport)
                .ForeignKey("dbo.Etudiant", t => t.Etudiant_IdEtudiant, cascadeDelete: true)
                .Index(t => t.Etudiant_IdEtudiant);
            
            CreateTable(
                "dbo.OrdreDuJour",
                c => new
                    {
                        idOdJ = c.Int(nullable: false, identity: true),
                        titreOdJ = c.String(nullable: false, maxLength: 50, unicode: false),
                        dateOdJ = c.DateTime(nullable: false, storeType: "date"),
                        heureDebutReunion = c.String(nullable: false, maxLength: 10, unicode: false),
                        heureFinReunion = c.String(nullable: false, maxLength: 10, unicode: false),
                        lieuReunionODJ = c.String(maxLength: 10, unicode: false),
                        idCreateurOdJ = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.idOdJ);
            
            CreateTable(
                "dbo.sujetpointprincipal",
                c => new
                    {
                        idPointPrincipal = c.Int(nullable: false, identity: true),
                        sujetPoint = c.String(nullable: false, maxLength: 50, unicode: false),
                        idOrdreDuJour = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.idPointPrincipal)
                .ForeignKey("dbo.OrdreDuJour", t => t.idOrdreDuJour, cascadeDelete: true)
                .Index(t => t.idOrdreDuJour);
            
            CreateTable(
                "dbo.souspointsujet",
                c => new
                    {
                        idSousPoint = c.Int(nullable: false),
                        sujetSousPoint = c.String(),
                        idSujetPointPrincipal = c.Int(nullable: false),
                        SujetPointPrincipal_idPointPrincipal = c.Int(),
                    })
                .PrimaryKey(t => t.idSousPoint)
                .ForeignKey("dbo.sujetpointprincipal", t => t.SujetPointPrincipal_idPointPrincipal)
                .ForeignKey("dbo.sujetpointprincipal", t => t.idSujetPointPrincipal, cascadeDelete: true)
                .Index(t => t.idSujetPointPrincipal)
                .Index(t => t.SujetPointPrincipal_idPointPrincipal);
            
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
                        Entraineur_id = c.Int(nullable: false),
                        Equipe_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Entraineur_id, t.Equipe_id })
                .ForeignKey("dbo.Entraineurs", t => t.Entraineur_id, cascadeDelete: true)
                .ForeignKey("dbo.Equipes", t => t.Equipe_id, cascadeDelete: true)
                .Index(t => t.Entraineur_id)
                .Index(t => t.Equipe_id);
            
            CreateTable(
                "dbo.EquipeJoueur",
                c => new
                    {
                        Equipe_id = c.Int(nullable: false),
                        Joueur_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Equipe_id, t.Joueur_Id })
                .ForeignKey("dbo.Equipes", t => t.Equipe_id, cascadeDelete: true)
                .ForeignKey("dbo.Joueurs", t => t.Joueur_Id, cascadeDelete: true)
                .Index(t => t.Equipe_id)
                .Index(t => t.Joueur_Id);
            
            CreateTable(
                "dbo.JoueurItem",
                c => new
                    {
                        Joueur_Id = c.Int(nullable: false),
                        Item_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Joueur_Id, t.Item_id })
                .ForeignKey("dbo.Joueurs", t => t.Joueur_Id, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.Item_id, cascadeDelete: true)
                .Index(t => t.Joueur_Id)
                .Index(t => t.Item_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.sujetpointprincipal", "idOrdreDuJour", "dbo.OrdreDuJour");
            DropForeignKey("dbo.souspointsujet", "idSujetPointPrincipal", "dbo.sujetpointprincipal");
            DropForeignKey("dbo.souspointsujet", "SujetPointPrincipal_idPointPrincipal", "dbo.sujetpointprincipal");
            DropForeignKey("dbo.Profils", "MembreEsport_IdMembreEsport", "dbo.Etudiants");
            DropForeignKey("dbo.Joueurs", "MembreEsport_IdMembreEsport", "dbo.Etudiants");
            DropForeignKey("dbo.Etudiants", "Etudiant_IdEtudiant", "dbo.Etudiant");
            DropForeignKey("dbo.JoueurItem", "Item_id", "dbo.Items");
            DropForeignKey("dbo.JoueurItem", "Joueur_Id", "dbo.Joueurs");
            DropForeignKey("dbo.Joueurs", "Etudiant_IdEtudiant", "dbo.Etudiant");
            DropForeignKey("dbo.EquipeJoueur", "Joueur_Id", "dbo.Joueurs");
            DropForeignKey("dbo.EquipeJoueur", "Equipe_id", "dbo.Equipes");
            DropForeignKey("dbo.Jeux", "StatutId", "dbo.Statuts");
            DropForeignKey("dbo.Rangs", "JeuId", "dbo.Jeux");
            DropForeignKey("dbo.HistoriqueRang", "RangId", "dbo.Rangs");
            DropForeignKey("dbo.HistoriqueRang", "JoueurId", "dbo.Joueurs");
            DropForeignKey("dbo.Joueurs", "ProfilId", "dbo.Profils");
            DropForeignKey("dbo.Profils", "JeuId", "dbo.Jeux");
            DropForeignKey("dbo.Profils", "Etudiant_IdEtudiant", "dbo.Etudiant");
            DropForeignKey("dbo.Equipes", "JeuId", "dbo.Jeux");
            DropForeignKey("dbo.Caracteristiques", "JeuId", "dbo.Jeux");
            DropForeignKey("dbo.EntraineurEquipe", "Equipe_id", "dbo.Equipes");
            DropForeignKey("dbo.EntraineurEquipe", "Entraineur_id", "dbo.Entraineurs");
            DropForeignKey("dbo.Items", "CaracteristiqueId", "dbo.Caracteristiques");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Application", "Stage_IdStage", "dbo.Stage");
            DropForeignKey("dbo.Stage", "Status_IdStatus", "dbo.Status");
            DropForeignKey("dbo.Stage", "Poste_IdPoste", "dbo.Poste");
            DropForeignKey("dbo.Stage", "Location_IdLocation", "dbo.Location");
            DropForeignKey("dbo.Stage", "Contact_IdContact", "dbo.Contact");
            DropForeignKey("dbo.Contact", "Entreprise_IdEntreprise", "dbo.Entreprise");
            DropForeignKey("dbo.Application", "Etudiant_IdEtudiant", "dbo.Etudiant");
            DropForeignKey("dbo.Etudiant", "Preference_IdPreference", "dbo.Preference");
            DropForeignKey("dbo.Poste", "Preference_IdPreference", "dbo.Preference");
            DropForeignKey("dbo.Location", "Preference_IdPreference", "dbo.Preference");
            DropForeignKey("dbo.Entreprise", "Preference_IdPreference", "dbo.Preference");
            DropForeignKey("dbo.AccesProgramme", "codeProgramme", "dbo.EnteteProgramme");
            DropForeignKey("dbo.DevisMinistere", "codeProgramme", "dbo.EnteteProgramme");
            DropForeignKey("dbo.Programme", "idDevis", "dbo.DevisMinistere");
            DropForeignKey("dbo.EnonceCompetence", "idDevis", "dbo.DevisMinistere");
            DropForeignKey("dbo.PlanCadreEnonce", "idCompetence", "dbo.EnonceCompetence");
            DropForeignKey("dbo.ElementCompetence", "idCompetence", "dbo.EnonceCompetence");
            DropForeignKey("dbo.PlanCadreElement", "idElement", "dbo.ElementCompetence");
            DropForeignKey("dbo.PlanCadre", "idType", "dbo.TypePlanCadre");
            DropForeignKey("dbo.PlanCadre", "idProgramme", "dbo.Programme");
            DropForeignKey("dbo.PlanCadrePrealable", "idPrealable", "dbo.PlanCadre");
            DropForeignKey("dbo.PlanCadrePrealable", "idPlanCadre", "dbo.PlanCadre");
            DropForeignKey("dbo.PlanCadrePrealable", "idStatus", "dbo.StatusPrealable");
            DropForeignKey("dbo.PlanCadreEnonce", "idPlanCadre", "dbo.PlanCadre");
            DropForeignKey("dbo.PlanCadreElement", "idPlanCadre", "dbo.PlanCadre");
            DropForeignKey("dbo.Cours", "idPlanCadre", "dbo.PlanCadre");
            DropForeignKey("dbo.Cours", "idSession", "dbo.Session");
            DropForeignKey("dbo.Cours", "idGrille", "dbo.GrilleCours");
            DropForeignKey("dbo.PlanCadreElement", "idElementConnaissance", "dbo.ElementConnaissance");
            DropForeignKey("dbo.ElementConnaissance", "idActiviteApprentissage", "dbo.ActiviteApprentissage");
            DropForeignKey("dbo.CriterePerformance", "idElement", "dbo.ElementCompetence");
            DropForeignKey("dbo.ContexteRealisation", "idCompetence", "dbo.EnonceCompetence");
            DropIndex("dbo.JoueurItem", new[] { "Item_id" });
            DropIndex("dbo.JoueurItem", new[] { "Joueur_Id" });
            DropIndex("dbo.EquipeJoueur", new[] { "Joueur_Id" });
            DropIndex("dbo.EquipeJoueur", new[] { "Equipe_id" });
            DropIndex("dbo.EntraineurEquipe", new[] { "Equipe_id" });
            DropIndex("dbo.EntraineurEquipe", new[] { "Entraineur_id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.souspointsujet", new[] { "SujetPointPrincipal_idPointPrincipal" });
            DropIndex("dbo.souspointsujet", new[] { "idSujetPointPrincipal" });
            DropIndex("dbo.sujetpointprincipal", new[] { "idOrdreDuJour" });
            DropIndex("dbo.Etudiants", new[] { "Etudiant_IdEtudiant" });
            DropIndex("dbo.HistoriqueRang", new[] { "RangId" });
            DropIndex("dbo.HistoriqueRang", new[] { "JoueurId" });
            DropIndex("dbo.Rangs", new[] { "JeuId" });
            DropIndex("dbo.Profils", new[] { "MembreEsport_IdMembreEsport" });
            DropIndex("dbo.Profils", new[] { "Etudiant_IdEtudiant" });
            DropIndex("dbo.Profils", new[] { "JeuId" });
            DropIndex("dbo.Jeux", new[] { "StatutId" });
            DropIndex("dbo.Equipes", new[] { "JeuId" });
            DropIndex("dbo.Joueurs", new[] { "MembreEsport_IdMembreEsport" });
            DropIndex("dbo.Joueurs", new[] { "Etudiant_IdEtudiant" });
            DropIndex("dbo.Joueurs", new[] { "ProfilId" });
            DropIndex("dbo.Items", new[] { "CaracteristiqueId" });
            DropIndex("dbo.Caracteristiques", new[] { "JeuId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Contact", new[] { "Entreprise_IdEntreprise" });
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
            DropIndex("dbo.Programme", new[] { "idDevis" });
            DropIndex("dbo.PlanCadrePrealable", new[] { "idStatus" });
            DropIndex("dbo.PlanCadrePrealable", new[] { "idPrealable" });
            DropIndex("dbo.PlanCadrePrealable", new[] { "idPlanCadre" });
            DropIndex("dbo.PlanCadreEnonce", new[] { "idCompetence" });
            DropIndex("dbo.PlanCadreEnonce", new[] { "idPlanCadre" });
            DropIndex("dbo.Cours", new[] { "idSession" });
            DropIndex("dbo.Cours", new[] { "idGrille" });
            DropIndex("dbo.Cours", new[] { "idPlanCadre" });
            DropIndex("dbo.PlanCadre", new[] { "idType" });
            DropIndex("dbo.PlanCadre", new[] { "idProgramme" });
            DropIndex("dbo.ElementConnaissance", new[] { "idActiviteApprentissage" });
            DropIndex("dbo.PlanCadreElement", new[] { "idElementConnaissance" });
            DropIndex("dbo.PlanCadreElement", new[] { "idElement" });
            DropIndex("dbo.PlanCadreElement", new[] { "idPlanCadre" });
            DropIndex("dbo.CriterePerformance", new[] { "idElement" });
            DropIndex("dbo.ElementCompetence", new[] { "idCompetence" });
            DropIndex("dbo.ContexteRealisation", new[] { "idCompetence" });
            DropIndex("dbo.EnonceCompetence", new[] { "idDevis" });
            DropIndex("dbo.DevisMinistere", new[] { "codeProgramme" });
            DropIndex("dbo.AccesProgramme", new[] { "codeProgramme" });
            DropTable("dbo.JoueurItem");
            DropTable("dbo.EquipeJoueur");
            DropTable("dbo.EntraineurEquipe");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Responsable");
            DropTable("dbo.souspointsujet");
            DropTable("dbo.sujetpointprincipal");
            DropTable("dbo.OrdreDuJour");
            DropTable("dbo.Etudiants");
            DropTable("dbo.lieuDeLaReunion");
            DropTable("dbo.Statuts");
            DropTable("dbo.HistoriqueRang");
            DropTable("dbo.Rangs");
            DropTable("dbo.Profils");
            DropTable("dbo.Jeux");
            DropTable("dbo.Entraineurs");
            DropTable("dbo.Equipes");
            DropTable("dbo.Joueurs");
            DropTable("dbo.Items");
            DropTable("dbo.Caracteristiques");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Status");
            DropTable("dbo.Contact");
            DropTable("dbo.Stage");
            DropTable("dbo.Poste");
            DropTable("dbo.Location");
            DropTable("dbo.Entreprise");
            DropTable("dbo.Preference");
            DropTable("dbo.Etudiant");
            DropTable("dbo.Application");
            DropTable("dbo.TypePlanCadre");
            DropTable("dbo.Programme");
            DropTable("dbo.StatusPrealable");
            DropTable("dbo.PlanCadrePrealable");
            DropTable("dbo.PlanCadreEnonce");
            DropTable("dbo.Session");
            DropTable("dbo.GrilleCours");
            DropTable("dbo.Cours");
            DropTable("dbo.PlanCadre");
            DropTable("dbo.ActiviteApprentissage");
            DropTable("dbo.ElementConnaissance");
            DropTable("dbo.PlanCadreElement");
            DropTable("dbo.CriterePerformance");
            DropTable("dbo.ElementCompetence");
            DropTable("dbo.ContexteRealisation");
            DropTable("dbo.EnonceCompetence");
            DropTable("dbo.DevisMinistere");
            DropTable("dbo.EnteteProgramme");
            DropTable("dbo.AccesProgramme");
        }
    }
}

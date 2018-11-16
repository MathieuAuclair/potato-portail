using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ApplicationPlanCadre.Models;
using ApplicationPlanCadre.Models.eSports;
using ApplicationPlanCadre.Models.Reunions;
using Microsoft.AspNet.Identity.EntityFramework;
using SysInternshipManagement.Models;
using SysInternshipManagement.Models.eSports;
using SysInternshipManagement.Models.SystemeStage;
using Etudiant = SysInternshipManagement.Models.Etudiant;

namespace SysInternshipManagement.Migrations
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=GestionStageConnectionString")
        {
        }

        public DbSet<Application> Application { get; set; }
        public DbSet<Entreprise> Entreprise { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Stage> Stage { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Poste> Poste { get; set; }
        public DbSet<Preference> Preference { get; set; }
        public DbSet<Responsable> Responsible { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Etudiant> Etudiant { get; set; }

        public virtual DbSet<ContexteRealisation> ContexteRealisation { get; set; }
        public virtual DbSet<Cours> Cours { get; set; }
        public virtual DbSet<CriterePerformance> CriterePerformance { get; set; }
        public virtual DbSet<DevisMinistere> DevisMinistere { get; set; }
        public virtual DbSet<ElementCompetence> ElementCompetence { get; set; }
        public virtual DbSet<EnonceCompetence> EnonceCompetence { get; set; }
        public virtual DbSet<EnteteProgramme> EnteteProgramme { get; set; }
        public virtual DbSet<GrilleCours> GrilleCours { get; set; }
        public virtual DbSet<PlanCadre> PlanCadre { get; set; }
        public virtual DbSet<PlanCadrePrealable> PlanCadrePrealable { get; set; }
        public virtual DbSet<Programme> Programme { get; set; }
        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<StatusPrealable> StatusPrealable { get; set; }
        public virtual DbSet<TypePlanCadre> TypePlanCadre { get; set; }
        public virtual DbSet<PlanCadreElement> PlanCadreElement { get; set; }
        public virtual DbSet<PlanCadreEnonce> PlanCadreEnonce { get; set; }
        public virtual DbSet<ActiviteApprentissage> ActiviteApprentissage { get; set; }
        public virtual DbSet<ElementConnaissance> ElementConnaissance { get; set; }
        public virtual DbSet<AccesProgramme> AccesProgramme { get; set; }
        public virtual DbSet<OrdreDuJour> OrdreDuJour { get; set; }
        public virtual DbSet<SousPointSujet> SousPointProjet { get; set; }
        public virtual DbSet<SujetPointPrincipal> SujetPointPrincipal { get; set; }
        public virtual DbSet<lieuDeLaReunion> LieuDeLaReunion { get; set; }

        public virtual DbSet<Caracteristique> Caracteristiques { get; set; }
        public virtual DbSet<Equipe> Equipes { get; set; }
        public virtual DbSet<MembreEsport> MembreEsports { get; set; }
        public virtual DbSet<HistoriqueRang> HistoriquesRang { get; set; }
        public virtual DbSet<Profil> Profils { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Jeu> Jeux { get; set; }
        public virtual DbSet<Joueur> Joueurs { get; set; }
        public virtual DbSet<Rang> Rangs { get; set; }
        public virtual DbSet<Statut> Statuts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContexteRealisation>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<CriterePerformance>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<DevisMinistere>()
                .Property(e => e.annee)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DevisMinistere>()
                .Property(e => e.codeSpecialisation)
                .IsUnicode(false);

            modelBuilder.Entity<DevisMinistere>()
                .Property(e => e.specialisation)
                .IsUnicode(false);

            modelBuilder.Entity<DevisMinistere>()
                .Property(e => e.nbUnite);


            modelBuilder.Entity<DevisMinistere>()
                .Property(e => e.condition)
                .IsUnicode(false);

            modelBuilder.Entity<DevisMinistere>()
                .Property(e => e.sanction)
                .IsUnicode(false);

            modelBuilder.Entity<DevisMinistere>()
                .Property(e => e.docMinistere)
                .IsUnicode(false);

            modelBuilder.Entity<DevisMinistere>()
                .Property(e => e.codeProgramme)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DevisMinistere>()
                .HasMany(e => e.EnonceCompetence)
                .WithRequired(e => e.DevisMinistere)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DevisMinistere>()
                .HasMany(e => e.Programme)
                .WithRequired(e => e.DevisMinistere)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ElementCompetence>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<ElementCompetence>()
                .HasMany(e => e.CriterePerformance)
                .WithRequired(e => e.ElementCompetence)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ElementCompetence>()
                .HasMany(e => e.PlanCadreElement)
                .WithRequired(e => e.ElementCompetence)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EnonceCompetence>()
                .Property(e => e.codeCompetence)
                .IsUnicode(false);

            modelBuilder.Entity<EnonceCompetence>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<EnonceCompetence>()
                .HasMany(e => e.ContexteRealisation)
                .WithRequired(e => e.EnonceCompetence)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EnonceCompetence>()
                .HasMany(e => e.ElementCompetence)
                .WithRequired(e => e.EnonceCompetence)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EnonceCompetence>()
                .HasMany(e => e.PlanCadreEnonce)
                .WithRequired(e => e.EnonceCompetence)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EnteteProgramme>()
                .Property(e => e.codeProgramme)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EnteteProgramme>()
                .Property(e => e.nom)
                .IsUnicode(false);

            modelBuilder.Entity<EnteteProgramme>()
                .HasMany(e => e.DevisMinistere)
                .WithRequired(e => e.EnteteProgramme)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GrilleCours>()
                .Property(e => e.nom)
                .IsUnicode(false);

            modelBuilder.Entity<GrilleCours>()
                .HasMany(e => e.Cours)
                .WithRequired(e => e.GrilleCours)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlanCadre>()
                .Property(e => e.numeroCours)
                .IsUnicode(false);

            modelBuilder.Entity<PlanCadre>()
                .Property(e => e.titreCours)
                .IsUnicode(false);

            modelBuilder.Entity<PlanCadre>()
                .Property(e => e.indicationPedago)
                .IsUnicode(false);

            modelBuilder.Entity<PlanCadre>()
                .Property(e => e.environnementPhys)
                .IsUnicode(false);

            modelBuilder.Entity<PlanCadre>()
                .Property(e => e.ressource)
                .IsUnicode(false);

            modelBuilder.Entity<PlanCadre>()
                .HasMany(e => e.Cours)
                .WithRequired(e => e.PlanCadre)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlanCadre>()
                .HasMany(e => e.PlanCadrePrealable)
                .WithRequired(e => e.PlanCadre)
                .HasForeignKey(e => e.idPlanCadre)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlanCadre>()
                .HasMany(e => e.PlanCadreElement)
                .WithRequired(e => e.PlanCadre)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlanCadre>()
                .HasMany(e => e.PlanCadreEnonce)
                .WithRequired(e => e.PlanCadre)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlanCadre>()
                .HasMany(e => e.Prealable)
                .WithRequired(e => e.Prealable)
                .HasForeignKey(e => e.idPrealable)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Programme>()
                .Property(e => e.nom)
                .IsUnicode(false);

            modelBuilder.Entity<Programme>()
                .Property(e => e.annee)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Programme>()
                .HasMany(e => e.PlanCadre)
                .WithRequired(e => e.Programme)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Session>()
                .Property(e => e.nom)
                .IsUnicode(false);

            modelBuilder.Entity<Session>()
                .HasMany(e => e.Cours)
                .WithRequired(e => e.Session)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StatusPrealable>()
                .Property(e => e.status)
                .IsUnicode(false);

            modelBuilder.Entity<StatusPrealable>()
                .HasMany(e => e.PlanCadrePrealable)
                .WithRequired(e => e.StatusPrealable)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TypePlanCadre>()
                .Property(e => e.nom)
                .IsUnicode(false);

            modelBuilder.Entity<TypePlanCadre>()
                .HasMany(e => e.PlanCadre)
                .WithRequired(e => e.TypePlanCadre)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ActiviteApprentissage>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<ActiviteApprentissage>()
                .HasMany(e => e.ElementConnaissance)
                .WithRequired(e => e.ActiviteApprentissage)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ElementConnaissance>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<ElementConnaissance>()
                .HasMany(e => e.PlanCadreElement)
                .WithRequired(e => e.ElementConnaissance)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AccesProgramme>()
                .Property(e => e.codeProgramme)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OrdreDuJour>()
                .Property(e => e.titreOdJ)
                .IsUnicode(false);

            modelBuilder.Entity<OrdreDuJour>()
                .Property(e => e.heureDebutReunion)
                .IsUnicode(false);

            modelBuilder.Entity<OrdreDuJour>()
                .Property(e => e.heureFinReunion)
                .IsUnicode(false);

            modelBuilder.Entity<OrdreDuJour>()
                .Property(e => e.lieuReunionODJ)
                .IsUnicode(false);

            modelBuilder.Entity<OrdreDuJour>()
                .HasMany(e => e.SujetPointPrincipal)
                .WithRequired(e => e.ordredujour)
                .HasForeignKey(e => e.idOrdreDuJour);

            modelBuilder.Entity<SujetPointPrincipal>()
                .Property(e => e.sujetPoint)
                .IsUnicode(false);

            modelBuilder.Entity<SujetPointPrincipal>()
                .HasMany(e => e.souspointsujet)
                .WithRequired(e => e.sujetpointprincipal)
                .HasForeignKey(e => e.idSujetPointPrincipal);
            modelBuilder.Entity<Profil>()
                .HasOptional(p => p.Joueur)
                .WithRequired(j => j.Profil)
                .Map(c => c.MapKey("ProfilId"));

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
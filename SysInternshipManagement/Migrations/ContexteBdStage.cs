using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Migrations
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=GestionStageConnectionString")
        {
        }

        public DbSet<Application> application { get; set; }
        public DbSet<Entreprise> entreprise { get; set; }
        public DbSet<Contact> contact { get; set; }
        public DbSet<Stage> stage { get; set; }
        public DbSet<Location> location { get; set; }
        public DbSet<Poste> poste { get; set; }
        public DbSet<Preference> preference { get; set; }
        public DbSet<Responsable> responsible { get; set; }
        public DbSet<Status> status { get; set; }
        public DbSet<Etudiant> etudiant { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
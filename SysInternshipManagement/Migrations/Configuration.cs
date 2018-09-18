namespace SysInternshipManagement.Migrations
{
    using SysInternshipManagement.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SysInternshipManagement.Migrations.InitialCreate>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SysInternshipManagement.Migrations.InitialCreate context)
        {
            context.internship.AddOrUpdate(new Internship { });
            context.business.AddOrUpdate(new Business { });
        }
    }
}

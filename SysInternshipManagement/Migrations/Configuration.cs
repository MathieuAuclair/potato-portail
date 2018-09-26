namespace SysInternshipManagement.Migrations
{
    using SysInternshipManagement.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SysInternshipManagement.Migrations.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(SysInternshipManagement.Migrations.DatabaseContext context)
        {
            var db = context;

            var post = new Post();
            post.Name = "Programmeur";
            db.post.Add(post);

            db.SaveChanges();

            var status = new Status();
            status.StatusInternship = "Occupé";
            db.status.Add(status);

            db.SaveChanges();

            var location = new Location();
            location.Name = "Montréal";
            db.location.Add(location);

            db.SaveChanges();

            var contact = new Contact();
            contact.Name = "Aymen";
            contact.Email = "AymenSioud@beep.boop";
            contact.Phone = "+1-(123)-456-7890";
            db.contact.Add(contact);

            db.SaveChanges();

            var post2 = new Post();
            post.Name = "Technicien";
            db.post.Add(post);

            db.SaveChanges();

            var status2 = new Status();
            status.StatusInternship = "Disponible";
            db.status.Add(status);

            db.SaveChanges();

            var location2 = new Location();
            location.Name = "Saguenay";
            db.location.Add(location);

            db.SaveChanges();

            var contact2 = new Contact();
            contact.Name = "Éric Dallaire";
            contact.Email = "EricChose@Microsoft.xyz";
            contact.Phone = "+1-(098)-765-4321";
            db.contact.Add(contact);

            
            db.SaveChanges();
        }
    }
}

using SysInternshipManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace SysInternshipManagement.Migrations
{
    public class InternshipInitializer: System.Data.Entity.DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {

            context.SaveChanges();
            var statuts = new List<Status>
            {
            new Status{IdStatus=1,StatusInternship="Occupé"},

            };
            statuts.ForEach(s => context.status.Add(s));
            context.SaveChanges();
        }
    }
}
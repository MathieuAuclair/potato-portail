using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SysInternshipManagement.Models
{
    public class DB : DbContext
    {
        public DbSet<Application> application { get; set; }
        public DbSet<Business> business { get; set; }
        public DbSet<Contact> contact { get; set; }
        public DbSet<ContactBusiness> contactBusiness { get; set; }
        public DbSet<Internship> internship { get; set; }
        public DbSet<Location> location { get; set; }
        public DbSet<Post> post { get; set; }
        public DbSet<Preference> preference { get; set; }
        public DbSet<Responsible> responsible { get; set; }
        public DbSet<Status> status { get; set; }
        public DbSet<Student> student { get; set; }

    }
}
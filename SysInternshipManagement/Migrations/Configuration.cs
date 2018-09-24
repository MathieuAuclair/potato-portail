namespace SysInternshipManagement.Migrations
{
    using SysInternshipManagement.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DatabaseContext context)
        {
            Location location = new Location();
            location.Name = "test";

            Status status = new Status();
            status.StatusInternship = "test";

            Post post = new Post();
            post.Name = "test";

            Internship internship = new Internship();
            internship.Status = status;
            internship.Post = post;
            internship.Location = location;

            context.status.Add(status);
            /*context.application.Add(
                new Application
                {
                    Timestamp = DateTime.UtcNow,
                    Internship =
                    {
                        new Internship
                        {
                            Address = "legit street",
                            Description = "legit development internship",
                            PostalCode = "A0B 1C3",
                            Salary = 20.0f,
                            Status =
                            {
                                new Status
                                {
                                    StatusInternship = "Invalid",
                                }
                            },
                            Contact =
                            {
                                new Contact
                                {
                                    Email = "beepboopboop@beep.boop",
                                    Name = "Mathieu Auclair",
                                    Phone = "1-(800)-666-BEEP",
                                },
                            },
                            Location =
                            {
                                new Location
                                {
                                    Name = "Tunis"
                                }
                            },
                            Post =
                            {
                                new Post
                                {
                                    Name = "Architecte Système",
                                },
                            },
                        },
                    },
                    Student =
                    {
                        new Student
                        {
                            IdPreference =
                            {
                                new Preference
                                {
                                    Salary = 20.0f,
                                    Location =
                                    {
                                        new Location
                                        {
                                            Name = "Silicon Valley"
                                        },
                                    },
                                    Post =
                                    {
                                        new Post
                                        {
                                            Name = "Développeur `Full Stack`",
                                        }
                                    },
                                    Business =
                                    {
                                        new Business
                                        {
                                            Name = "Indian SCC (scam call center)",
                                            Address = "110 rue Principale",
                                            Location =
                                            {
                                                new Location
                                                {
                                                    Name = "Chicoutimi"
                                                },
                                            },
                                        },
                                    },
                                },
                            },
                            DaNumber = "1633485",
                            EmailPersonal = "mathieuauclair@aol.com",
                            EmailSchool = "AucMa1633485@etu.cegepjonquiere.ca",
                            FirstName = "mathieu",
                            LastName = "auclair",
                            Phone = "1-(123)-456-7890",
                            PermanentCode = "AucMa12099709",
                            Role = "étudiant",
                        }
                    },
                });*/

            base.Seed(context);
        }
    }
}
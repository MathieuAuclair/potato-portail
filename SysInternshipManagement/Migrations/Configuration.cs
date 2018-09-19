namespace SysInternshipManagement.Migrations
{
    using SysInternshipManagement.Models;
    using System;
    using System.Collections.Generic;
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
            context.post.AddOrUpdate(
                new Post
                {
                    idPost = 1,
                    name = "analyste",
                },
                new Post
                {
                    idPost = 2,
                    name = "programmeur",
                });
            context.location.AddOrUpdate(
                new Location
                {
                    idLocation = 1,
                    name = "Quebec",
                });
            context.status.AddOrUpdate(
                new Status
                {
                    idStatus = 1,
                    status = "occupé",
                },
                new Status
                {
                    idStatus = 2,
                    status = "disponible",
                },
                new Status
                {
                    idStatus = 3,
                    status = "suspendu",
                });
            context.business.AddOrUpdate(
                new Business
                {
                    idBusiness = 1,
                    address = "110 legit street",
                    name = "CGI",
                    idLocation = { 1 },
                });
            context.responsible.AddOrUpdate(
                new Responsible
                {
                    idResponsible = 1,
                    email = "aymensioud@cegepjonquiere.ca",
                    firstName = "Aymen",
                    lastName = "Sioud",
                    phone = "1-(234)-456-7890",
                    role = "Administrateur",
                },
                new Responsible
                {
                    idResponsible = 2,
                    email = "ericdallaire@cegepjonquiere.ca",
                    firstName = "eric",
                    lastName = "dallaire",
                    phone = "1-(234)-567-7890",
                    role = "Administrateur"
                });
            context.preference.AddOrUpdate(
                new Preference
                {
                    idPreference = 1,
                    idBusiness = { 1 },
                    idLocation = { 1 },
                    idPost = { 1 },
                    salary = 0,
                });
            context.student.AddOrUpdate(
                new Student
                {
                    idStudent = 1,
                    idPreference = { 1 },
                    DaNumber = "1633485",
                    emailPersonal = "mathieuauclair@aol.com",
                    emailSchool = "AucMa1633485@etu.cegepjonquiere.ca",
                    firstName = "mathieu",
                    lastName = "auclair",
                    phone = "1-(123)-456-7890",
                    permanentCode = "AucMa12099709",
                    role = "Étudiant",
                });
            context.contact.AddOrUpdate(
                new Contact
                {
                    idContact = 1,
                    email = "john@legitbusiness.xyz",
                    name = "John",
                    phone = "1-(234)-456-7890",
                });
            context.contactBusiness.AddOrUpdate(
                new ContactBusiness
                {
                    idContactBusiness = 1,
                    idBusiness = { 1 },
                    idContact = { 1 },
                });
            context.internship.AddOrUpdate(
                new Internship
                {
                    idInternship = 1,
                    address = "legit street",
                    description = "legit development internship",
                    postalCode = "A0B 1C3",
                    salary = 20.0f,
                    idStatus = { 1 },
                    idContact = { 1 },
                    idLocation = { 1 },
                    idPost = { 1 },
                });
            context.application.AddOrUpdate(
                new Application
                {
                    idApplication = 1,
                    idInternship = { 1 },
                    student = { 1 },
                    timestamp = DateTime.UtcNow,
                });
        }
    }
}

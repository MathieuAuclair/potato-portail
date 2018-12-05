using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PotatoPortail.Models;
using Microsoft.AspNet.Identity;
using PotatoPortail.Helpers;
using PotatoPortail.Migrations;

namespace PotatoPortail.Controllers
{
    [RCPDevisMinistereAuthorize]
    public class DevisMinistereController : Controller
    {
        private readonly BDPortail _db = new BDPortail();

        private IQueryable<DevisMinistere> GetRcpDevisMinistere()
        {
            string username = User.Identity.GetUserName();
            return from devisMinistere in _db.DevisMinistere
                join enteteProgramme in _db.Departement on devisMinistere.Discipline equals enteteProgramme
                    .Discipline
                join accesProgramme in _db.AccesProgramme on enteteProgramme.Discipline equals accesProgramme
                    .Discipline
                   where accesProgramme.UserMail == username
                select devisMinistere;
        }

        public ActionResult ListeDevis()
        {
            return PartialView(GetRcpDevisMinistere().ToList());
        }

        public ActionResult Index()
        {
            return View(GetRcpDevisMinistere().ToList());
        }

        public ActionResult Info(int? idDevis)
        {
            if (idDevis == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DevisMinistere devisMinistere = _db.DevisMinistere.Find(idDevis);
            if (devisMinistere == null)
            {
                return HttpNotFound();
            }

            ViewBag.total = devisMinistere.NbHeureFrmGenerale + devisMinistere.NbHeureFrmSpecifique;
            return View(devisMinistere);
        }

        public ActionResult Modifier(int? idDevis)
        {
            if (idDevis == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DevisMinistere devisMinistere = _db.DevisMinistere.Find(idDevis);
            if (devisMinistere == null)
            {
                return HttpNotFound();
            }

            return View(devisMinistere);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include =
                "idDevis, codeProgramme, annee, codeSpecialisation, nom, dateValidation, docMinistere, specialisation, sanction, nbUnite, condition, nbHeurefrmGenerale,nbHeurefrmSpecifique")]
            DevisMinistere devisMinistere, HttpPostedFileBase docMinistere)
        {
            devisMinistere.Departement = _db.Departement.Find(devisMinistere.Discipline);
            if (docMinistere != null)
            {
                if (!TeleverserFichier(docMinistere, devisMinistere))
                    ModelState.AddModelError("PDF", @"Le fichier doit être de type PDF.");
            }

            if (ModelState.IsValid)
            {
                _db.Entry(devisMinistere).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Info", "DevisMinistere", new {idDevis = devisMinistere.IdDevis});
            }

            return View(devisMinistere);
        }

        public bool TeleverserFichier(HttpPostedFileBase fichier, DevisMinistere devisMinistere)
        {
            try
            {
                string nomFichier = Path.GetFileName(fichier.FileName);
                string chemin = Path.Combine(Server.MapPath("~/Files/Document ministériel"),
                    nomFichier ?? throw new NullReferenceException());
                string extension = nomFichier.Substring(nomFichier.Length - 4, 4);
                string ancienChemin = devisMinistere.DocMinistere;
                devisMinistere.DocMinistere = nomFichier;
                if (extension == ".pdf")
                {
                    fichier.SaveAs(chemin);
                    if (ancienChemin != null)
                        SupressionFichier(ancienChemin);
                    return true;
                }

                return false;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public bool SupressionFichier(string nomFichier)
        {
            try
            {
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Files/Document ministériel"), nomFichier));
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        protected override void Dispose(bool disposer)
        {
            if (disposer)
            {
                _db.Dispose();
            }

            base.Dispose(disposer);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApplicationPlanCadre.Models;
using ApplicationPlanCadre.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ApplicationPlanCadre.Controllers
{
    [RCPDevisMinistereAuthorize]
    public class DevisMinistereController : Controller
    {
        private BDPlanCadre db = new BDPlanCadre();

        private IQueryable<DevisMinistere> getRCPDevisMinistere()
        {
            string username = User.Identity.GetUserName();
            return from devisMinistere in db.DevisMinistere
                   join departement in db.Departement on devisMinistere.discipline equals departement.discipline
                   join accesProgramme in db.AccesProgramme on departement.discipline equals accesProgramme.discipline
                   where accesProgramme.userMail == username
                   select devisMinistere;
           
        }

        public ActionResult ListeDevis()
        {
            return PartialView(getRCPDevisMinistere().ToList());
            
        }

        public ActionResult Index()
        {
            return View(getRCPDevisMinistere().ToList());
        }

        public ActionResult Info(int? idDevis)
        {
            if (idDevis == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DevisMinistere devisMinistere = db.DevisMinistere.Find(idDevis);
            if (devisMinistere == null)
            {
                return HttpNotFound();
            }
            ViewBag.total = devisMinistere.nbHeureFrmGenerale + devisMinistere.nbHeureFrmSpecifique;
            //ViewBag.dateValidation = checkValidation(devisMinistere);
            return View(devisMinistere);
        }

        public ActionResult Modifier(int? idDevis)
        {
            if (idDevis == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DevisMinistere devisMinistere = db.DevisMinistere.Find(idDevis);
            if (devisMinistere == null)
            {
                return HttpNotFound();
            }
            return View(devisMinistere);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "idDevis, discipline, annee, codeSpecialisation, nom, dateValidation, docMinistere, specialisation, sanction, nbUnite, condition, nbHeurefrmGenerale,nbHeurefrmSpecifique")] DevisMinistere devisMinistere, HttpPostedFileBase docMinistere)
        {
            devisMinistere.Departement = db.Departement.Find(devisMinistere.discipline);
            if (docMinistere != null)
            {
                if(!TeleverserFichier(docMinistere, devisMinistere))
                    ModelState.AddModelError("PDF", "Le fichier doit être de type PDF.");
            }
            //Trim(devisMinistere);
            if (ModelState.IsValid)
            {
                db.Entry(devisMinistere).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Info", "DevisMinistere", new { idDevis = devisMinistere.idDevis });
            }
            return View(devisMinistere);
        }

        public bool TeleverserFichier(HttpPostedFileBase fichier, DevisMinistere devisMinistere)
        {
            try
            {
                string nomFichier = Path.GetFileName(fichier.FileName);
                string chemin = Path.Combine(Server.MapPath("~/Files/Document ministériel"), nomFichier);
                string extension = nomFichier.Substring(nomFichier.Length - 4, 4);
                string ancienChemin = devisMinistere.docMinistere;
                devisMinistere.docMinistere = nomFichier;
                if (extension == ".pdf")
                {
                    fichier.SaveAs(chemin);
                    if (ancienChemin != null)
                        SupressionFichier(ancienChemin);
                    return true;
                }
                return false;
            }
            catch(IOException)
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
            catch(IOException)
            {
                return false;
            }
        }

        protected override void Dispose(bool disposer)
        {
            if (disposer)
            {
                db.Dispose();
            }
            base.Dispose(disposer);
        }
    }
}

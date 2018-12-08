using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PotatoPortail.Helpers;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers
{
    [RcpDevisMinistereAuthorize]
    public class DevisMinistereController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

        private IEnumerable<DevisMinistere> GetRcpDevisMinistere()
        {
            string username = User.Identity.GetUserName();
            return from devisMinistere in _db.DevisMinistere
                   join departement in _db.Departement on devisMinistere.Discipline equals departement.Discipline
                   join accesProgramme in _db.AccesProgramme on departement.Discipline equals accesProgramme.Discipline
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
        public ActionResult Modifier([Bind(Include = "idDevis, discipline, annee, codeSpecialisation, nom, dateValidation, docMinistere, specialisation, sanction, nbUnite, condition, nbHeurefrmGenerale,nbHeurefrmSpecifique")] DevisMinistere devisMinistere, HttpPostedFileBase docMinistere)
        {
            devisMinistere.Departement = _db.Departement.Find(devisMinistere.Discipline);
            
            if (docMinistere != null)
            {
                if(!TeleverserFichier(docMinistere, devisMinistere))
                    ModelState.AddModelError("PDF", "Le fichier doit être de type PDF.");
            }

            if (!ModelState.IsValid) return View(devisMinistere);
            _db.Entry(devisMinistere).State = EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("Info", "DevisMinistere", new { idDevis = devisMinistere.IdDevis });
        }

        public bool TeleverserFichier(HttpPostedFileBase fichier, DevisMinistere devisMinistere)
        {
            try //Sérieux cette fonction là c'est une vrai fucking pile of shit!!! Reparez moi ça!
            {
                var nomFichier = Path.GetFileName(fichier.FileName);

                if (nomFichier == null)
                {
                    return false;
                }

                var chemin = Path.Combine(Server.MapPath("~/Files/Document ministériel"), nomFichier);
                var extension = nomFichier.Substring(nomFichier.Length - 4, 4);
                var ancienChemin = devisMinistere.DocMinistere;
                devisMinistere.DocMinistere = nomFichier;
                if (extension != ".pdf") return false;
                fichier.SaveAs(chemin);
                if (ancienChemin != null)
                    SupressionFichier(ancienChemin);
                return true;
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
                _db.Dispose();
            }
            base.Dispose(disposer);
        }
    }
}

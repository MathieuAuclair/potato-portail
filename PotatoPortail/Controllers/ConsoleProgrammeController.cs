using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApplicationPlanCadre.Models;
using ApplicationPlanCadre.Helpers;

namespace ApplicationPlanCadre.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConsoleProgrammeController : Controller
    {
        private BDPlanCadre db = new BDPlanCadre();

        public ActionResult _PartialList()
        {
            return PartialView(db.Programme.ToList());
        }

        public ActionResult Creation()
        {
            ViewBag.idDevis = ConstruireDevisSelectList();
            return View();
        }

        public ActionResult Index()
        {
            return View(db.Programme.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "idProgramme,nom,annee,dateValidation,idDevis, nbSession")] Programme programme)
        {
            if (!validationDate(programme))
            {
                this.AddToastMessage("Problème lors de la création", "Veuillez entrer une année entre celle du devis et l'année courante", Toast.ToastType.Error, true);
            }
            else
            {
                if (!programmeExiste(programme) && ModelState.IsValid)
                {
                    this.AddToastMessage("Confirmation de la création", "Le programme " + '\u0022' + programme.nom + '\u0022' + ", a bien été crée.", Toast.ToastType.Success);
                    db.Programme.Add(programme);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    this.AddToastMessage("Problème lors de la création", "Un programme contenant le même nom et le même devis ministériel existe déjà.", Toast.ToastType.Error, true);
                }
            }

            ViewBag.idDevis = ConstruireDevisSelectList();
            return View(programme);
        }

        public ActionResult Modifier(int? idProgramme)
        {
            if (idProgramme == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programme programme = db.Programme.Find(idProgramme);
            if (programme == null)
            {
                return HttpNotFound();
            }
            ViewBag.idDevis = ConstruireDevisSelectList(programme.idDevis);
            return View(programme);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "idProgramme,nom,annee,dateValidation,idDevis, nbSession")] Programme programme)
        {
            if (!validationDate(programme))
            {               
                ModelState.AddModelError("Duplique", "Veuillez entrer une année entre celle du devis et l'année courante.");
            }
            if (ModelState.IsValid)
            {
                this.AddToastMessage("Confirmation de la modification", "Le programme " + '\u0022' + programme.nom + '\u0022' + ", a bien été modifié.", Toast.ToastType.Success);
                db.Entry(programme).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (!validationDate(programme))
            {
                this.AddToastMessage("Problème lors de la modification", "Veuillez entrer une année entre celle du devis et l'année courante.", Toast.ToastType.Error,true);
            }
            else
            {
                if (!programmeExiste(programme) && ModelState.IsValid)
                {
                    this.AddToastMessage("Confirmation de la modification", "Le programme " + '\u0022' + programme.nom + '\u0022' + ", a bien été modifié.", Toast.ToastType.Success);
                    db.Programme.Add(programme);
                    db.SaveChanges();
                }
            }
   
            ViewBag.idDevis = ConstruireDevisSelectList(programme.idDevis);
            return View(programme);
        }
		
        public ActionResult Valider(int idProgramme)
        {
            Programme programme = db.Programme.Find(idProgramme);
            programme.statusValider = true;
            db.Entry(programme).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private SelectList ConstruireDevisSelectList(int? idDevis = null)
        {
            var devis = db.DevisMinistere;
            List<SelectListItem> liste = new List<SelectListItem>();
            foreach (DevisMinistere e in devis)
            {
                liste.Add(new SelectListItem { Value = e.idDevis.ToString(), Text = e.nom });
            }
            if (idDevis != null)
                return new SelectList(liste, "Value", "Text", idDevis);
            return new SelectList(liste, "Value", "Text");
        }

        [ActionName("Supression")]
        public ActionResult SurpressionConfirmer(int idProgramme)
        {
            Programme Programme = db.Programme.Find(idProgramme);
            if (Programme == null)
            {
                this.AddToastMessage("Problème lors de la supression", "Le programme "+ '\u0022' + Programme.nom + '\u0022' + ", n'a pus être supprimé", Toast.ToastType.Error,true);
            }
            else
            {
                db.Programme.Remove(Programme);
                db.SaveChanges();
                this.AddToastMessage("Confirmation de la supression", "Le programme " + '\u0022' + Programme.nom + '\u0022' + ", a bien été supprimé", Toast.ToastType.Success);
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposer)
        {
            if (disposer)
            {
                db.Dispose();
            }
            base.Dispose(disposer);
        }

        private bool programmeExiste(Programme programme)
        {
            bool existe = db.Programme.Any(p => p.idDevis == programme.idDevis && p.nom == programme.nom && p.annee == programme.annee);
            return existe;
        }

        private bool validationDate(Programme programme)
        {
            bool dateValide = false;
            int anneeProgramme;
            int anneeCourrante = DateTime.Now.Year;
            int anneeDevis;

            IQueryable<DevisMinistere> DevisMin =
                from dev in db.DevisMinistere
                where dev.idDevis == programme.idDevis
                select dev;

            anneeCourrante += 2;
            anneeDevis = Convert.ToInt32(DevisMin.First().annee);
            anneeProgramme = Convert.ToInt32(programme.annee);

            if (anneeProgramme >= anneeDevis && anneeProgramme <= anneeCourrante)
            {
                dateValide = true;
            }
            return dateValide;
        }
    }
}

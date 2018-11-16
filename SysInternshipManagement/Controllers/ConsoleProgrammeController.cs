using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ApplicationPlanCadre.Models;
using SysInternshipManagement.Migrations;

namespace SysInternshipManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConsoleProgrammeController : Controller
    {
        private readonly DatabaseContext _db = new DatabaseContext();

        public ActionResult _PartialList()
        {
            return PartialView(_db.Programme.ToList());
        }

        public ActionResult Creation()
        {
            ViewBag.idDevis = ConstruireDevisSelectList();
            return View();
        }

        public ActionResult Index()
        {
            return View(_db.Programme.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "idProgramme,nom,annee,dateValidation,idDevis, nbSession")]
            Programme programme)
        {
            if (!ValidationDate(programme))
            {
                ModelState.AddModelError("Duplique",
                    @"Veuillez entrer une année entre celle du devis et l'année courante.");
                this.AddToastMessage("Confirmation de la création",
                    "Le programme " + '\u0022' + programme.nom + '\u0022' + ", n'a pas pus être crée.",
                    Toast.ToastType.Error);
            }
            else
            {
                if (!ProgrammeExiste(programme) && ModelState.IsValid)
                {
                    this.AddToastMessage("Confirmation de la création",
                        "Le programme " + '\u0022' + programme.nom + '\u0022' + ", a bien été crée.",
                        Toast.ToastType.Success);
                    _db.Programme.Add(programme);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }

                this.AddToastMessage("Confirmation de la création",
                    "Le programme " + programme.nom + ", n'a pas pus être crée.", Toast.ToastType.Error);
                ModelState.AddModelError("Duplique",
                    @"Un programme contenant le même nom et le même devis ministériel existe déjà.");
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

            Programme programme = _db.Programme.Find(idProgramme);
            if (programme == null)
            {
                return HttpNotFound();
            }

            ViewBag.idDevis = ConstruireDevisSelectList(programme.idDevis);
            return View(programme);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "idProgramme,nom,annee,dateValidation,idDevis, nbSession")]
            Programme programme)
        {
            if (!ValidationDate(programme))
            {
                ModelState.AddModelError("Duplique",
                    @"Veuillez entrer une année entre celle du devis et l'année courante.");
            }

            if (ModelState.IsValid)
            {
                this.AddToastMessage("Confirmation de la modification",
                    "Le programme " + '\u0022' + programme.nom + '\u0022' + ", a bien été modifié.",
                    Toast.ToastType.Success);
                _db.Entry(programme).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (!ValidationDate(programme))
            {
                this.AddToastMessage("Confirmation de la modification",
                    "Le programme " + '\u0022' + programme.nom + '\u0022' + ", n'a pas pus être modifié.",
                    Toast.ToastType.Error);
                ModelState.AddModelError("Duplique",
                    @"Veuillez entrer une année entre celle du devis et l'année courante.");
            }
            else
            {
                if (!ProgrammeExiste(programme) && ModelState.IsValid)
                {
                    this.AddToastMessage("Confirmation de la modification",
                        "Le programme " + '\u0022' + programme.nom + '\u0022' + ", a bien été modifié.",
                        Toast.ToastType.Success);
                    _db.Programme.Add(programme);
                    _db.SaveChanges();
                }
            }

            ViewBag.idDevis = ConstruireDevisSelectList(programme.idDevis);
            return View(programme);
        }

        public ActionResult Valider(int idProgramme)
        {
            Programme programme = _db.Programme.Find(idProgramme);

            if (programme == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            programme.statusValider = true;

            _db.Entry(programme).State = EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        private SelectList ConstruireDevisSelectList(int? idDevis = null)
        {
            var devis = _db.DevisMinistere;
            List<SelectListItem> liste = new List<SelectListItem>();
            foreach (DevisMinistere e in devis)
            {
                liste.Add(new SelectListItem {Value = e.idDevis.ToString(), Text = e.nom});
            }

            if (idDevis != null)
                return new SelectList(liste, "Value", "Text", idDevis);
            return new SelectList(liste, "Value", "Text");
        }

        [ActionName("Supression")]
        public ActionResult SurpressionConfirmer(int idProgramme)
        {
            Programme programme = _db.Programme.Find(idProgramme);

            if (programme == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            _db.Programme.Remove(programme);
            _db.SaveChanges();
            this.AddToastMessage("Confirmation de la supression",
                "Le programme " + '\u0022' + programme.nom + '\u0022' + ", a bien été supprimé",
                Toast.ToastType.Success);


            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposer)
        {
            if (disposer)
            {
                _db.Dispose();
            }

            base.Dispose(disposer);
        }

        private bool ProgrammeExiste(Programme programme)
        {
            bool existe = _db.Programme.Any(p =>
                p.idDevis == programme.idDevis && p.nom == programme.nom && p.annee == programme.annee);
            return existe;
        }

        private bool ValidationDate(Programme programme)
        {
            bool dateValide = false;
            int anneeCourrante = DateTime.Now.Year;

            IQueryable<DevisMinistere> devisMin =
                from dev in _db.DevisMinistere
                where dev.idDevis == programme.idDevis
                select dev;

            anneeCourrante += 2;
            var anneeDevis = Convert.ToInt32(devisMin.First().annee);
            var anneeProgramme = Convert.ToInt32(programme.annee);

            if (anneeProgramme >= anneeDevis && anneeProgramme <= anneeCourrante)
            {
                dateValide = true;
            }

            return dateValide;
        }
    }
}
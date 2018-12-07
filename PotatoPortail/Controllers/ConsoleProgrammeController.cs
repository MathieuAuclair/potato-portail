using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ApplicationPlanCadre.Controllers;
using PotatoPortail.Migrations;
using PotatoPortail.Models;
using PotatoPortail.Toast;

namespace PotatoPortail.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConsoleProgrammeController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

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
        public ActionResult Creation([Bind(Include = "idProgramme,nom,annee,dateValidation,idDevis, nbSession")] Programme programme)
        {
            if (!ValidationDate(programme))
            {
                this.AddToastMessage("Problème lors de la création", "Veuillez entrer une année entre celle du devis et l'année courante", ToastType.Error, true);
            }
            else
            {
                if (!ProgrammeExiste(programme) && ModelState.IsValid)
                {
                    this.AddToastMessage("Confirmation de la création", "Le programme " + '\u0022' + programme.Nom + '\u0022' + ", a bien été crée.", ToastType.Success);
                    _db.Programme.Add(programme);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    this.AddToastMessage("Problème lors de la création", "Un programme contenant le même nom et le même devis ministériel existe déjà.", ToastType.Error, true);
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
            Programme programme = _db.Programme.Find(idProgramme);
            if (programme == null)
            {
                return HttpNotFound();
            }
            ViewBag.idDevis = ConstruireDevisSelectList(programme.IdDevis);
            return View(programme);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "idProgramme,nom,annee,dateValidation,idDevis, nbSession")] Programme programme)
        {
            if (!ValidationDate(programme))
            {               
                ModelState.AddModelError("Duplique", "Veuillez entrer une année entre celle du devis et l'année courante.");
            }
            if (ModelState.IsValid)
            {
                this.AddToastMessage("Confirmation de la modification", "Le programme " + '\u0022' + programme.Nom + '\u0022' + ", a bien été modifié.", ToastType.Success);
                _db.Entry(programme).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (!ValidationDate(programme))
            {
                this.AddToastMessage("Problème lors de la modification", "Veuillez entrer une année entre celle du devis et l'année courante.", ToastType.Error,true);
            }
            else
            {
                if (!ProgrammeExiste(programme) && ModelState.IsValid)
                {
                    this.AddToastMessage("Confirmation de la modification", "Le programme " + '\u0022' + programme.Nom + '\u0022' + ", a bien été modifié.", ToastType.Success);
                    _db.Programme.Add(programme);
                    _db.SaveChanges();
                }
            }
   
            ViewBag.idDevis = ConstruireDevisSelectList(programme.IdDevis);
            return View(programme);
        }
		
        public ActionResult Valider(int idProgramme)
        {
            var programme = _db.Programme.Find(idProgramme);

            if (programme == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            programme.StatutStageValider = true;
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
                liste.Add(new SelectListItem { Value = e.IdDevis.ToString(), Text = e.Nom });
            }
            if (idDevis != null)
                return new SelectList(liste, "Value", "Text", idDevis);
            return new SelectList(liste, "Value", "Text");
        }

        [ActionName("Supression")]
        public ActionResult SuppressionConfirmer(int idProgramme)
        {
            var programme = _db.Programme.Find(idProgramme);
            if (programme == null)
            {
                    this.AddToastMessage("Problème lors de la supression",
                        "Le programme n'a pus être supprimé",
                        ToastType.Error, true);
            }
            else
            {
                _db.Programme.Remove(programme);
                _db.SaveChanges();
                this.AddToastMessage("Confirmation de la supression", "Le programme " + '\u0022' + programme.Nom + '\u0022' + ", a bien été supprimé", ToastType.Success);
            }
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
            var existe = _db.Programme.Any(tableProgramme => tableProgramme.IdDevis == programme.IdDevis && tableProgramme.Nom == programme.Nom && tableProgramme.Annee == programme.Annee);
            return existe;
        }

        private bool ValidationDate(Programme programme)
        {
            var dateValide = false;
            var anneeCourrante = DateTime.Now.Year;

            var devisMin =
                from dev in _db.DevisMinistere
                where dev.IdDevis == programme.IdDevis
                select dev;

            anneeCourrante += 2;
            var anneeDevis = Convert.ToInt32(devisMin.First().Annee);
            var anneeProgramme = Convert.ToInt32(programme.Annee);

            if (anneeProgramme >= anneeDevis && anneeProgramme <= anneeCourrante)
            {
                dateValide = true;
            }
            return dateValide;
        }
    }
}

using ApplicationPlanCadre.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ApplicationPlanCadre.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConsoleDevisMinistereController : Controller
    {
        private BDPlanCadre db = new BDPlanCadre();

        public ActionResult _PartialList()
        {
            var devisMinistere = db.DevisMinistere.Include(p => p.Departement);
            return PartialView(devisMinistere.ToList());
        }

        public ActionResult Index()
        {
            var devisMinistere = db.DevisMinistere.Include(p => p.Departement);

            return View(devisMinistere.ToList());
        }

        public ActionResult Creation()
        {
            ViewBag.discipline = ConstruireCodeDevisMinistereSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "discipline, annee, codeSpecialisation")]
            DevisMinistere devisMinistere)
        {
            if (!DevisExiste(devisMinistere) && ModelState.IsValid)
            {
                devisMinistere.codeSpecialisation = devisMinistere.codeSpecialisation.ToUpper().Trim();
                db.DevisMinistere.Add(devisMinistere);
                db.SaveChanges();
                this.AddToastMessage("Création confirmée",
                    "Le devis " + '\u0022' + devisMinistere.nom + '\u0022' + " a bien été créé.",
                    Toast.ToastType.Success);
                return RedirectToAction("Index");

            }

            if (DevisExiste(devisMinistere))
            {
                this.AddToastMessage("Problème pour confirmée", "Erreur, ce devis existe déjà.", Toast.ToastType.Error,
                    true);
            }

            ViewBag.discipline = ConstruireCodeDevisMinistereSelectList();
            return View(devisMinistere);
        }

        public ActionResult Modifier(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DevisMinistere devisMinistere = db.DevisMinistere.Find(id);
            if (devisMinistere == null)
            {
                return HttpNotFound();
            }

            ViewBag.discipline = ConstruireCodeDevisMinistereSelectList(devisMinistere.discipline);
            return View(devisMinistere);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "idDevis, discipline, annee, codeSpecialisation")]
            DevisMinistere devisMinistere)
        {
            if (!DevisExiste(devisMinistere) && ModelState.IsValid)
            {
                devisMinistere.codeSpecialisation = devisMinistere.codeSpecialisation.ToUpper();
                db.Entry(devisMinistere).State = EntityState.Modified;
                db.SaveChanges();
                this.AddToastMessage("Confirmation de la modification",
                    "Le devis " + '\u0022' + devisMinistere.nom + '\u0022' + " a bien été modifié.",
                    Toast.ToastType.Success);
                return RedirectToAction("Index");
            }

            if (DevisExiste(devisMinistere))
            {
                this.AddToastMessage("Problème lors de la modification", "Erreur, ce devis ministeriel existe déjà.",
                    Toast.ToastType.Error, true);
            }

            ViewBag.discipline = ConstruireCodeDevisMinistereSelectList();
            return View(devisMinistere);
        }

        public SelectList ConstruireCodeDevisMinistereSelectList(string discipline = null)
        {
            var liste = db.Departement
                .Select(e => new {discipline = e.discipline, texte = e.discipline + " - " + e.nom}).ToList();
            if (discipline != null)
                return new SelectList(liste, "discipline", "texte", discipline);
            return new SelectList(liste, "discipline", "texte");
        }

        [ActionName("Supression")]
        public ActionResult SurpressionConfirmer(int id)
        {
            DevisMinistere DevisMinistere = db.DevisMinistere.Find(id);
            db.DevisMinistere.Remove(DevisMinistere);
            db.SaveChanges();
            this.AddToastMessage("Confirmation de la suppression",
                "Le devis " + '\u0022' + DevisMinistere.nom + '\u0022' + " a bien été supprimé.",
                Toast.ToastType.Success);
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

        private bool DevisExiste(DevisMinistere devisMinistere)
        {
            bool existe = db.DevisMinistere.Any(p =>
                p.discipline == devisMinistere.discipline && p.annee == devisMinistere.annee &&
                p.codeSpecialisation == devisMinistere.codeSpecialisation && p.idDevis != devisMinistere.idDevis);
            return existe;
        }

    }
}
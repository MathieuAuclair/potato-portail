using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ApplicationPlanCadre.Models.eSports;
using PotatoPortail.Migrations;
using PotatoPortail.Models.eSports;

namespace PotatoPortail.Controllers.Esports
{
    public class JeuController : Controller
    {
        private readonly BDPortail _db = new BDPortail();

        public ActionResult Index()
        {
            return View(_db.Jeux.OrderBy(j => j.nomJeu).ToList());
        }

        public ActionResult Details(int? id, string nomJeu)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Joueur> lstJoueursJeu = new List<Joueur>();

            Jeu jeu = _db.Jeux.Find(id);

            if (jeu == null)
            {
                return HttpNotFound();
            }

            foreach (Joueur joueur in _db.Joueurs)
            {
                if (joueur.JeuEquipeMonojoueur == jeu.nomJeu)
                    lstJoueursJeu.Add(joueur);
            }

            ViewBag.lstJoueursJeu = lstJoueursJeu.OrderBy(j => j.PseudoJoueur).ToList();
            ViewBag.nomJeu = nomJeu;

            return View(jeu);
        }

        // GET: Jeu/Create
        public ActionResult Creation()
        {
            var statuts = _db.Statuts.ToList();
            List<SelectListItem> lstStatuts = new List<SelectListItem>();

            foreach (Statut statut in statuts)
            {
                lstStatuts.Add(new SelectListItem
                {
                    Text = statut.nomStatut,
                    Value = statut.id.ToString()
                });
            }

            ViewBag.Statuts = lstStatuts;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "id,nomJeu,description,urlReference,abreviation,StatutId")]
            Jeu jeu)
        {
            var jeuDuMemeNom = from j in _db.Jeux
                where j.nomJeu.Equals(jeu.nomJeu, StringComparison.OrdinalIgnoreCase)
                select j;

            var abvIdentique = from j in _db.Jeux
                where j.abreviation.Equals(jeu.abreviation, StringComparison.OrdinalIgnoreCase)
                select j;

            if (jeuDuMemeNom.Any())
            {
                ViewBag.JeuExistant = jeuDuMemeNom.First().nomJeu + " est déjà enregistré dans le système.";
                ViewBag.Statuts = new SelectList(_db.Statuts, "id", "nomStatut", jeu.StatutId);
                return View(jeu);
            }

            if (abvIdentique.Any())
            {
                ViewBag.AbvExistante = "L'abréviation " + abvIdentique.First().abreviation +
                                       " est déjà utilisée pour " + abvIdentique.First().nomJeu + ".";
                ViewBag.Statuts = new SelectList(_db.Statuts, "id", "nomStatut", jeu.StatutId);
                return View(jeu);
            }

            if (ModelState.IsValid)
            {
                _db.Jeux.Add(jeu);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jeu);
        }

        // GET: Jeu/Edit/5
        public ActionResult Modifier(int? id, string nomJeu)
        {
            var caracJeu = from carac in _db.Caracteristique
                where carac.JeuId == id
                select carac;

            List<Caracteristique> lstCarac = new List<Caracteristique>();

            foreach (Caracteristique carac in caracJeu)
            {
                lstCarac.Add(carac);
            }

            var statuts = _db.Statuts.ToList();
            List<SelectListItem> lstStatuts = new List<SelectListItem>();

            foreach (Statut statut in statuts)
            {
                lstStatuts.Add(new SelectListItem
                {
                    Text = statut.nomStatut,
                    Value = statut.id.ToString()
                });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Jeu jeu = _db.Jeux.Find(id);
            if (jeu == null)
            {
                return HttpNotFound();
            }

            ViewBag.nomJeu = nomJeu;
            ViewBag.Statuts = lstStatuts;
            ViewBag.carac = lstCarac;

            return View(jeu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "id,nomJeu,description,urlReference,abreviation,StatutId")]
            Jeu jeu)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(jeu).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jeu);
        }

        // GET: Jeu/Delete/5
        public ActionResult Supprimer(int? id, string nomJeu)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var equipesJeu = from e in _db.Equipes
                where e.JeuId == id
                select e;

            Jeu jeu = _db.Jeux.Find(id);
            if (jeu == null)
            {
                return HttpNotFound();
            }

            ViewBag.nomJeu = nomJeu;
            ViewBag.equipesJeu = equipesJeu.ToList();

            return View(jeu);
        }

        // POST: Jeu/Delete/5
        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmationSupprimer(int id)
        {
            Jeu jeu = _db.Jeux.Find(id);
            
            if (jeu == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            _db.Jeux.Remove(jeu);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
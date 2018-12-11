using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models;
using PotatoPortail.Models.eSports;
using PotatoPortail.Toast;

namespace PotatoPortail.Controllers.eSports
{
    public class JeuController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

    
        public ActionResult Index()
        {
            return View(_db.Jeu.OrderBy(jeu => jeu.NomJeu).ToList());
        }
        public ActionResult Details(int? id, string nomJeu)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Joueur> lstJoueursJeu = new List<Joueur>();

            Jeu jeu = _db.Jeu.Find(id);

            if (jeu == null)
            {
                return HttpNotFound();
            }

            foreach (Joueur joueur in _db.Joueur)
            {
                if (joueur.JeuEquipeMonojoueur == jeu.NomJeu)
                    lstJoueursJeu.Add(joueur);
            }

            ViewBag.lstJoueursJeu = lstJoueursJeu.OrderBy(joueur => joueur.PseudoJoueur).ToList();
            ViewBag.nomJeu = nomJeu;

            return View(jeu);
        }
        public ActionResult Creation()
        {
            var statuts = _db.Statut.ToList();
            var lstStatuts = new List<SelectListItem>();

            foreach (Statut statut in statuts)
            {
                lstStatuts.Add(new SelectListItem
                {
                    Text = statut.NomStatut,
                    Value = statut.Id.ToString()
                });
            }

            ViewBag.Statuts = lstStatuts;
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "id,nomJeu,description,urlReference,abreviation,IdStatut")] Jeu jeu)
        {
            var jeuDuMemeNom = from tableJeu in _db.Jeu
                               where tableJeu.NomJeu.Equals(jeu.NomJeu, StringComparison.OrdinalIgnoreCase)
                               select tableJeu;

            var abvIdentique = from tableJeu in _db.Jeu
                               where tableJeu.Abreviation.Equals(jeu.Abreviation, StringComparison.OrdinalIgnoreCase)
                               select tableJeu;

            if (jeuDuMemeNom.Any())
            {
                this.AddToastMessage("Jeu déjà existant.", jeuDuMemeNom.First().NomJeu + " est déjà entré dans le système.", ToastType.Error, true);
                ViewBag.Statuts = new SelectList(_db.Statut, "id", "nomStatut", jeu.IdStatut);
                return View(jeu);
            }

            if(abvIdentique.Any())
            {
                this.AddToastMessage("Abréviation déjà utilisée.", "L'abréviation « " + abvIdentique.First().Abreviation + " » est déjà utilisée pour « " + abvIdentique.First().NomJeu + " ». Choisissez-en une autre.", ToastType.Error, true);
                ViewBag.AbvExistante = "L'abréviation " + abvIdentique.First().Abreviation + " est déjà utilisée pour " + abvIdentique.First().NomJeu + ".";
                ViewBag.Statuts = new SelectList(_db.Statut, "id", "nomStatut", jeu.IdStatut);
                return View(jeu);
            }

            if (ModelState.IsValid)
            {
                _db.Jeu.Add(jeu);
                _db.SaveChanges();
                this.AddToastMessage("Ajout de jeu effectué.", "« " + jeu.NomJeu + " » a été ajouté à la liste des Jeu.", ToastType.Success);
                return RedirectToAction("Index");
            }
            return View(jeu);
        }
        public ActionResult Modifier(int? id, string nomJeu)
        {
            var caracJeu = from carac in _db.Caracteristique
                           where carac.IdJeu == id
                           select carac;

            List<Caracteristique> lstCarac = new List<Caracteristique>();

            foreach (Caracteristique carac in caracJeu)
            {
                lstCarac.Add(carac);
            }

            var statuts = _db.Statut.ToList();

            List < SelectListItem > lstStatuts = new List<SelectListItem>();

            foreach (Statut statut in statuts)
            {
                lstStatuts.Add(new SelectListItem
                {
                    Text = statut.NomStatut,
                    Value = statut.Id.ToString()
                });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Jeu jeu = _db.Jeu.Find(id);
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
        public ActionResult EditItem([Bind(Include = "id,NomItem,IdCaracteristique")] Item item)
        {
            var caracteristique = _db.Caracteristique.Find(item.IdCaracteristique);

            if (caracteristique == null)
            {
                return HttpNotFound();
            }

            var jeu = _db.Jeu.Find(caracteristique.IdJeu);
            if (!ModelState.IsValid) return View(jeu);
            caracteristique.Item.Add(item);

            if (jeu == null)
            {
                return new HttpNotFoundResult();
            }

            _db.SaveChanges();

            return RedirectToAction("Modifier", new { jeu.Id,jeu.NomJeu });
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "id,nomJeu,description,urlReference,abreviation,IdStatut")] Jeu jeu)
        {
            var caracteristiquesJeu = from c in _db.Caracteristique
                                      where c.IdJeu == jeu.Id
                                      select c;

            ViewBag.carac = caracteristiquesJeu.ToList();
            ViewBag.nomJeu = _db.Jeu.Find(jeu.Id)?.NomJeu;

            var jeuDuMemeNom = from tableJeu in _db.Jeu
                               where tableJeu.NomJeu.Equals(jeu.NomJeu, StringComparison.OrdinalIgnoreCase)
                               select tableJeu;

            if (jeuDuMemeNom.Any())
            {
                if (jeu.NomJeu != _db.Jeu.Find(jeu.Id)?.NomJeu)
                {
                    this.AddToastMessage("Jeu déjà existant.", jeuDuMemeNom.First().NomJeu + " est déjà entré dans le système.", ToastType.Error, true);
                    ViewBag.Statuts = new SelectList(_db.Statut, "id", "nomStatut", jeu.IdStatut);
                    jeu.NomJeu = _db.Jeu.Find(jeu.Id)?.NomJeu;
                    jeu.Statut = _db.Jeu.Find(jeu.Id)?.Statut;
                    return View(jeu);
                }
            }

            if (ModelState.IsValid)
            {
                _db.Set<Jeu>().AddOrUpdate(jeu);
                _db.SaveChanges();
                this.AddToastMessage("Modifications apportées.", "Les changements apportés à « " + jeu.NomJeu + " » ont été enregistrés.", ToastType.Success);
                return RedirectToAction("Index");
            }
            return View(jeu);
        }

        public ActionResult Supprimer(int? id, string nomJeu)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var equipeJeu = from e in _db.Equipe
                             where (e.IdJeu == id) && (e.EstMonoJoueur == false)
                             select e;

            var jeu = _db.Jeu.Find(id);
            if (jeu == null)
            {
                return HttpNotFound();
            }

            ViewBag.nomJeu = nomJeu;
            ViewBag.EquipeJeu = equipeJeu.ToList();

            return View(jeu);
        }
        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmationSupprimer(int id)
        {
            var jeu = _db.Jeu.Find(id);

            if (jeu == null)
            {
                return HttpNotFound();
            }

            _db.Jeu.Remove(jeu);
            _db.SaveChanges();
            this.AddToastMessage("Supression effectuée.", "« " + jeu.NomJeu + " » a été supprimé de la liste.", ToastType.Success);
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
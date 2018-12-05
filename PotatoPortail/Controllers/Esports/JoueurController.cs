using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers.Esports
{
    public class JoueurController : Controller
    {
        private readonly BDPortail _db = new BDPortail();

        public ActionResult Index(string sortOrder)
        {
            ViewBag.NomSortParm = string.IsNullOrEmpty(sortOrder) ? "nom_desc" : "";
            ViewBag.PrenomSortParm = string.IsNullOrEmpty(sortOrder) ? "prenom_desc" : "";
            ViewBag.PseudoSortParm = string.IsNullOrEmpty(sortOrder) ? "pseudo_desc" : "";
            ViewBag.JeuSortParm = string.IsNullOrEmpty(sortOrder) ? "jeu_desc" : "";

            var joueurs = from j in _db.Joueurs
                select j;

            switch (sortOrder)
            {
                case "nom_desc":
                    joueurs = joueurs.OrderByDescending(j => j.MembreESports.Nom);
                    break;
                case "prenom_desc":
                    joueurs = joueurs.OrderBy(j => j.MembreESports.Prenom);
                    break;
                case "pseudo_desc":
                    joueurs = joueurs.OrderBy(j => j.PseudoJoueur);
                    break;
                case "jeu_desc":
                    joueurs = joueurs.OrderBy(j => j.Profils.Jeux.NomJeu);
                    break;
                default:
                    joueurs = joueurs.OrderBy(j => j.MembreESports.Nom);
                    break;
            }

            return View(joueurs.ToList());
        }

        // GET: Joueur/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Joueurs joueur = _db.Joueurs.Find(id);

            if (joueur == null)
            {
                return HttpNotFound();
            }

            return View(joueur);
        }

        public ActionResult Creation()
        {
            var etudiants = _db.Etudiant.ToList();
            var etus = etudiants.OrderBy(e => e.NomDeFamille);

            List<SelectListItem> lstEtudiants = new List<SelectListItem>();

            foreach (Etudiant etu in etus)
            {
                lstEtudiants.Add(new SelectListItem
                {
                    Text = etu.Prenom + @" " + etu.NomDeFamille,
                    Value = etu.IdEtudiant.ToString()
                });
            }

            ViewBag.EtudiantId = lstEtudiants;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "id,pseudoJoueur,EtudiantId,ProfilId")]
            Joueurs joueur)
        {
            if (ModelState.IsValid)
            {
                _db.Joueurs.Add(joueur);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EtudiantId = new SelectList(_db.Etudiant, "id", "id", joueur.IdEtudiant);
            return View(joueur);
        }

        public ActionResult Modifier(int? id)
        {
            EditerJoueurVM viewModel = new EditerJoueurVM();

            Joueur joueur = _db.Joueurs.Find(id);

            if (joueur == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            Profil profil = _db.Profils.Find(joueur.Profil.Id);

            if (profil == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            viewModel.JoueurId = joueur.Id;
            viewModel.pseudo = joueur.PseudoJoueur;
            viewModel.courriel = profil.Courriel;
            viewModel.Etudiant = _db.Etudiant.Find(joueur.EtudiantId);
            viewModel.Jeu = _db.Jeux.Find(profil.JeuId);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "JoueurId,pseudo,courriel")]
            EditerJoueurVM viewModel)
        {
            Joueur joueur = _db.Joueurs.Find(viewModel.JoueurId);

            if (joueur == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            Profil profil = _db.Profils.Find(joueur.Profil.Id);

            if (profil == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (ModelState.IsValid)
            {
                joueur.PseudoJoueur = viewModel.pseudo;
                profil.Pseudo = viewModel.pseudo;
                profil.Courriel = viewModel.courriel;

                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        public ActionResult Supprimer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Joueurs joueur = _db.Joueurs.Find(id);

            if (joueur == null)
            {
                return HttpNotFound();
            }

            return View(joueur);
        }

        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmationSupprimer(int id)
        {
            Joueurs joueur = _db.Joueurs.Find(id);

            if (joueur == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            Etudiant etu = _db.Etudiant.Find(joueur.IdEtudiant);
            Profils profil = _db.Profils.Find(joueur.MembreESports.Id);
            Jeux jeu = _db.Jeux.Find(joueur.EquipeMonojoueur.JeuId);

            if (jeu == null || etu == null || profil == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var equipeMonojoueur = from tableEquipe in _db.Equipes
                join tableJoueur in _db.Jeux on tableEquipe.JeuId equals tableJoueur.id
                join tableProfil in _db.Profils on tableJoueur.id equals tableProfil.JeuId
                where (tableEquipe.nomEquipe ==
                       etu.Prenom + etu.NomDeFamille + "_" + jeu.abreviation + "_" + profil.EtudiantId) &&
                      (tableProfil.Id == profil.Id)
                select tableEquipe;

            _db.Joueurs.Remove(joueur);
            _db.Equipes.Remove(equipeMonojoueur.First());
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
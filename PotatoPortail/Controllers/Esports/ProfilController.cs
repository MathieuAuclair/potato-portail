using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers.Esports
{
    public class ProfilController : Controller
    {
        private readonly BDPortail _db = new BDPortail();

        public ActionResult Index()
        {
            var inscriptions = from p in _db.Profils
                where p.EstArchive == false
                select p;

            ViewBag.inscriptions = inscriptions.ToList();
            ViewBag.msgJoueurExistant = TempData["msgJoueurExistant"];

            //var profils = _db.Profils.Include(p => p.Etudiant).Include(p => p.Jeu).Where(p => p.EstArchive == false);
            return View(); //profils.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Profils profil = _db.Profils.Find(id);

            if (profil == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(profil);
        }

        public ActionResult Inscription()
        {
            List<SelectListItem> lstEtudiants = new List<SelectListItem>();
            List<SelectListItem> lstJeux = new List<SelectListItem>();
            List<SelectListItem> lstJeuxSecondaires = new List<SelectListItem>();

            var jeuxActifs = from j in _db.Jeux
                where j.Statuts.NomStatut == "Actif"
                select j;

            foreach (Etudiant etu in _db.Etudiant)
            {
                lstEtudiants.Add(new SelectListItem
                {
                    Text = etu.Prenom + etu.NomDeFamille,
                    Value = etu.IdEtudiant.ToString()
                });
            }

            lstJeuxSecondaires.Add(new SelectListItem {Text = @"----------------------------", Value = 0.ToString()});

            foreach (Jeux jeu in jeuxActifs)
            {
                lstJeux.Add(new SelectListItem
                {
                    Text = jeu.NomJeu,
                    Value = jeu.Id.ToString()
                });

                lstJeuxSecondaires.Add(new SelectListItem
                {
                    Text = jeu.NomJeu,
                    Value = jeu.Id.ToString()
                });
            }

            ViewBag.lstEtudiants = lstEtudiants;
            ViewBag.lstJeux = lstJeux;
            ViewBag.lstJeuxSecondaires = lstJeuxSecondaires;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inscription(
            [Bind(Include = "id,pseudo,courriel,note,EtudiantId,JeuId,estArchive,JeuSecondaireId")]
            Profils profil)
        {
            Etudiant etu = _db.Etudiant.Find(profil.MembreESports.Id);
            Jeux jeu = _db.Jeux.Find(profil.IdJeu);

            var equipeMonojoueurExistante = from e in _db.Equipes
                where e.EstMonoJoueur &&
                      (e.NomEquipe == etu.Prenom + etu.NomDeFamille + "_" + jeu.Abreviation + "_" + profil.MembreESports.Id)
                select e;

            if (profil.IdJeu == profil.IdJeuSecondaire)
            {
                ViewBag.jeuxSelectionnesIdentiques = "Le jeu primaire et le jeu secondaire ne peuvent être les mêmes!";
            }
            else
            {
                if (!equipeMonojoueurExistante.Any() && (profil.IdJeu != 0))
                {
                    if (ModelState.IsValid)
                    {
                        _db.Profils.Add(profil);
                        _db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.equipeMonojoueurExistante = "Cet étudiant est déjà validé en tant que joueur pour ce jeu.";
                }
            }

            List<SelectListItem> lstEtudiants = new List<SelectListItem>();
            List<SelectListItem> lstJeux = new List<SelectListItem>();
            List<SelectListItem> lstJeuxSecondaires = new List<SelectListItem>();

            var jeuxActifs = from j in _db.Jeux
                where j.Statuts.NomStatut == "Actif"
                select j;

            foreach (Etudiant etudiant in _db.Etudiant)
            {
                lstEtudiants.Add(new SelectListItem
                {
                    Text = etudiant.Prenom + etudiant.NomDeFamille,
                    Value = etudiant.IdEtudiant.ToString()
                });
            }

            lstJeuxSecondaires.Add(new SelectListItem {Text = @"----------------------------", Value = 0.ToString()});

            foreach (Jeux jeuActif in jeuxActifs)
            {
                lstJeux.Add(new SelectListItem
                {
                    Text = jeuActif.NomJeu,
                    Value = jeuActif.Id.ToString()
                });

                lstJeuxSecondaires.Add(new SelectListItem
                {
                    Text = jeuActif.NomJeu,
                    Value = jeuActif.Id.ToString()
                });
            }

            ViewBag.lstEtudiants = lstEtudiants;
            ViewBag.lstJeux = lstJeux;
            ViewBag.lstJeuxSecondaires = lstJeuxSecondaires;

            return View(profil);
        }

        public ActionResult Modifier(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Profils profil = _db.Profils.Find(id);
            if (profil == null)
            {
                return HttpNotFound();
            }

            ViewBag.EtudiantId = new SelectList(_db.Etudiant, "id", "nom", profil.MembreESports.Id);
            ViewBag.JeuId = new SelectList(_db.Jeux, "id", "nomJeu", profil.IdJeu);
            return View(profil);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "id,pseudo,courriel,note,EtudiantId,JeuId")]
            Profils profil)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(profil).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EtudiantId = new SelectList(_db.Etudiant, "id", "nom", profil.MembreESports.Id);
            ViewBag.JeuId = new SelectList(_db.Jeux, "id", "nomJeu", profil.IdJeu);
            return View(profil);
        }

        public ActionResult Valider(int? id, int? jeuId)
        {
            var joueur = new Joueurs();
            var equipeMonojoueur = new Equipes();

            var profil = _db.Profils.Find(id);

            if (profil == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            Jeux jeu = _db.Jeux.Find(profil.IdJeu) ?? _db.Jeux.Find(jeuId);

            if (jeu == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            profil.EstArchive = true;

            joueur.PseudoJoueur = profil.Pseudo;
            joueur.IdEtudiant = profil.Id;
            joueur.Profils = profil;

            var equipeMonojoueurJeu = from e in _db.Equipes
                where (e.EstMonoJoueur == true) &&
                      (e.NomEquipe == profil.MembreESports.Prenom + profil.MembreESports.Nom + "_" + jeu.Abreviation +
                       "_" + profil.MembreESports.Id)
                select e;
            if (!equipeMonojoueurJeu.Any())
            {
                equipeMonojoueur.NomEquipe =
                    profil.MembreESports.Prenom + profil.MembreESports.Nom + "_" + jeu.Abreviation + "_" +
                    profil.MembreESports.Id;
                equipeMonojoueur.IdJeu = jeu.Id;
                equipeMonojoueur.EstMonoJoueur = true;

                _db.Joueurs.Add(joueur);
                _db.Equipes.Add(equipeMonojoueur);
                _db.SaveChanges();

                return RedirectToAction("Index", "Joueur");
            }

            else

            {
                TempData["msgJoueurExistant"] = profil.MembreESports.Prenom + profil.MembreESports.Nom +
                                                " est déjà validé en tant que joueur pour " + profil.Jeux.NomJeu + ".";
                return RedirectToAction("Index", "Profil");
            }
        }

        public ActionResult Supprimer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var profil = _db.Profils.Find(id);

            if (profil == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _db.Profils.Remove(profil);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmationSupprimer(int id)
        {
            var profil = _db.Profils.Find(id);
            _db.Profils.Remove(profil ?? throw new InvalidOperationException());
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
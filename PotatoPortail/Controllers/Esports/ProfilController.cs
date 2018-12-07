using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApplicationPlanCadre.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PotatoPortail.Models;
using PotatoPortail.Toast;

namespace PotatoPortail.Controllers.eSports
{
    public class ProfilController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

        public ActionResult Index()
        {
            var inscriptions = from tableProfil in _db.Profils
                               where tableProfil.EstArchive == false
                               select tableProfil;

            ViewBag.inscriptions = inscriptions.ToList();

            var profils = _db.Profils.Include(p => p.MembreESports).Include(tableProfil => tableProfil.Jeux).Where(p => p.EstArchive == false);
            return View(profils.ToList());
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var profil = _db.Profils.Find(id);

            if (profil == null)
            {
                return HttpNotFound();
            }
            return View(profil);
        }
        
        public ActionResult Inscription()
        {
            var utilisateurId = User.Identity.GetUserId();

            var utilisateur = HttpContext.GetOwinContext()
                .GetUserManager<ApplicationUserManager>()
                .FindById(utilisateurId);

            if(!_db.MembreESports.Any(i => i.Id == utilisateurId))
            {
                MembreESports nouveauMembre = new MembreESports
                {
                    Id = utilisateurId,
                    Prenom = utilisateur.prenom,
                    Nom = utilisateur.nom
                };

                _db.MembreESports.Add(nouveauMembre);
                _db.SaveChanges();
            }

            List<SelectListItem> lstEtudiants = new List<SelectListItem>();
            List<SelectListItem> lstJeux = new List<SelectListItem>();
            List<SelectListItem> lstJeuxSecondaires = new List<SelectListItem>();

            var jeuxActifs = from tableJeu in _db.Jeux
                             where tableJeu.Statuts.NomStatut == "Actif"
                             select tableJeu;

            foreach (MembreESports etu in _db.MembreESports)
            {
                lstEtudiants.Add(new SelectListItem
                {
                    Text = etu.NomComplet,
                    Value = etu.Id                    
                });
            }

            lstJeuxSecondaires.Add(new SelectListItem { Text = "----------------------------", Value = 0.ToString() });

            foreach (var jeu in jeuxActifs)
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
        public ActionResult Inscription([Bind(Include = "id,pseudo,courriel,note,JeuId,estArchive,JeuSecondaireId")] Profil profil)
        {
            var utilisateurId = User.Identity.GetUserId();

            var membreEsport = _db.MembreESports.Find(utilisateurId);
            var jeu = _db.Jeux.Find(profil.IdJeu);

            if (jeu == null || membreEsport == null)
            {
                return HttpNotFound();
            }

            var equipeMonojoueurExistante = from tableEquipe in _db.Equipes
                                                where (tableEquipe.EstMonojoueur == true) && (tableEquipe.NomEquipe == membreEsport.NomComplet + "_" + jeu.Abreviation + "_" + utilisateurId)
                                                select tableEquipe;

            if (profil.IdJeu == profil.IdJeuSecondaire)
            {
                this.AddToastMessage("Inscription invalide.", "Veuillez choisir un jeu secondaire différent du jeu primaire ou tout simplement aucun.", ToastType.Error, true);
            }
            else
            {
                if (!equipeMonojoueurExistante.Any() && (profil.IdJeu != 0))
                {
                    if (ModelState.IsValid)
                    {
                        profil.IdMembreESports = User.Identity.GetUserId();

                        if (!_db.Profils.Any(tableProfil => tableProfil.IdMembreESports == profil.IdMembreESports && tableProfil.IdJeu == profil.IdJeu && tableProfil.EstArchive == false))
                        {
                            _db.Profils.Add(profil);
                            _db.SaveChanges();

                            if (profil.JeuSecondaire != null)
                                this.AddToastMessage("Inscription confirmée.", "Votre inscription pour « " + jeu.NomJeu + " » et « " + profil.jeuSecondaire.nomJeu + " » a été ajoutée à la liste.", ToastType.Success);
                            else
                                this.AddToastMessage("Inscription confirmée.", "Votre inscription pour « " + jeu.NomJeu + " » a été ajoutée à la liste.", ToastType.Success);

                            return RedirectToAction("Index");
                        }
                        else
                            this.AddToastMessage("Erreur de validation.", "Vous avez déjà appliqué pour « " + jeu.NomJeu + " ».", ToastType.Error, true);
                    }
                }
                else
                {
                    this.AddToastMessage("Inscription invalide.", "Vous êtes déjà un joueur de « " + jeu.NomJeu + " ».", ToastType.Error, true);
                }
            }

            var lstEtudiants = new List<SelectListItem>();
            var lstJeux = new List<SelectListItem>();
            var lstJeuxSecondaires = new List<SelectListItem>();

            var jeuxActifs = from j in _db.Jeux
                             where j.Statuts.NomStatut == "Actif"
                             select j;

            foreach (MembreESports etudiant in _db.MembreESports)
            {
                lstEtudiants.Add(new SelectListItem
                {
                    Text = etudiant.NomComplet,
                    Value = etudiant.Id
                });
            }

            lstJeuxSecondaires.Add(new SelectListItem { Text = "----------------------------", Value = 0.ToString() });

            foreach (var jeuActif in jeuxActifs)
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
            Profil profil = _db.Profils.Find(id);
            if (profil == null)
            {
                return HttpNotFound();
            }
            ViewBag.EtudiantId = new SelectList(_db.MembreESports, "id", "nom", profil.IdMembreESports);
            ViewBag.JeuId = new SelectList(_db.Jeux, "id", "nomJeu", profil.IdJeu);
            return View(profil);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "id,pseudo,courriel,note,EtudiantId,JeuId")] Profil profil)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(profil).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EtudiantId = new SelectList(_db.MembreESports, "id", "nom", profil.IdMembreESports);
            ViewBag.JeuId = new SelectList(_db.Jeux, "id", "nomJeu", profil.IdJeu);
            return View(profil);
        }

        public ActionResult Valider(int? id, int? jeuId)
        {
            var joueur = new Joueur();
            var equipeMonojoueur = new Equipe();
            Jeu jeu;

            var profil = _db.Profils.Find(id);

            if (profil == null)
            {
                return HttpNotFound();
            }

            if(jeuId == null)
            {
                jeu = _db.Jeux.Find(profil.IdJeu);
            }
            else
            {
                jeu = _db.Jeux.Find(jeuId);
                profil.IdJeuSecondaire = profil.IdJeu;
                profil.Jeux = jeu;
            }
                                        
            profil.EstArchive = true;

            joueur.PseudoJoueur = profil.Pseudo;
            joueur.IdMembreESports = profil.IdMembreESports;
            joueur.Profils = profil;

            var equipeMonojoueurJeu = from tableEquipe in _db.Equipes
                                      where tableEquipe.EstMonoJoueur && (tableEquipe.NomEquipe == profil.MembreESports.NomComplet + "_" + jeu.Abreviation + "_" + profil.IdMembreESports)
                                      select tableEquipe;

            if (jeu == null)
            {
                return HttpNotFound();
            }

            if (!equipeMonojoueurJeu.Any())
            {
                equipeMonojoueur.NomEquipe = profil.MembreESports.NomComplet + "_" + jeu.Abreviation + "_" + profil.IdMembreESports;
                equipeMonojoueur.IdJeu = jeu.Id;
                equipeMonojoueur.EstMonoJoueur = true;

                _db.Joueurs.Add(joueur);
                _db.Equipes.Add(equipeMonojoueur);
                _db.SaveChanges();
                this.AddToastMessage("Validation d'inscription effectuée.", profil.MembreESports.NomComplet + " est désormais un joueur de « " + jeu.NomJeu + " »!", ToastType.Success);

                return RedirectToAction("Index", "Joueur");
            }

            this.AddToastMessage("Erreur de validation.", profil.MembreESports.NomComplet + " est déjà validé en tant que joueur pour « " + profil.Jeux.NomJeu + " ».", ToastType.Error, true);
            return RedirectToAction("Index", "Profil");
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
                return HttpNotFound();
            }

            if(profil.JeuSecondaire != null)
            {
                this.AddToastMessage("Suppression d'inscription.", "L'inscription déposée par " + profil.MembreESports.NomComplet + " pour « " + profil.Jeux.NomJeu + " » et « " + profil.JeuSecondaire.nomJeu + " » a été retirée de la liste.", ToastType.Success);
            }
            else
            {
                this.AddToastMessage("Suppression d'inscription.", "L'inscription déposée par " + profil.MembreESports.NomComplet + " pour « " + profil.Jeux.NomJeu + " » a été retirée de la liste.", ToastType.Success);
            }

            _db.Profils.Remove(profil);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        
        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmationSupprimer(int id)
        {
            Profil profil = _db.Profils.Find(id);
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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApplicationPlanCadre.Models;
using ApplicationPlanCadre.Models.eSports;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;


namespace ApplicationPlanCadre.Controllers
{
    public class ProfilController : Controller
    {
        private BDPlanCadre db = new BDPlanCadre();
        public CompteController CompteController = new CompteController();

        // GET: Profil
        public ActionResult Index()
        {
            var inscriptions = from p in db.Profils
                               where p.estArchive == false
                               select p;

            ViewBag.inscriptions = inscriptions.ToList();

            var profils = db.Profils.Include(p => p.MembreESports).Include(p => p.Jeu).Where(p => p.estArchive == false);
            return View(profils.ToList());
        }

        // GET: Profil/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profil profil = db.Profils.Find(id);
            if (profil == null)
            {
                return HttpNotFound();
            }
            return View(profil);
        }

        // GET: Profil/Create
        public ActionResult Inscription()
        {
            string UtilisateurId = User.Identity.GetUserId();

            var Utilisateur = HttpContext.GetOwinContext()
                .GetUserManager<ApplicationUserManager>()
                .FindById(UtilisateurId);

            if(!db.MembreESports.Where(i => i.id == UtilisateurId).Any())
            {
                MembreESports nouveauMembre = new MembreESports
                {
                    id = UtilisateurId,
                    prenom = Utilisateur.prenom,
                    nom = Utilisateur.nom
                };

                db.MembreESports.Add(nouveauMembre);
                db.SaveChanges();
            }

            List<SelectListItem> lstEtudiants = new List<SelectListItem>();
            List<SelectListItem> lstJeux = new List<SelectListItem>();
            List<SelectListItem> lstJeuxSecondaires = new List<SelectListItem>();

            var jeuxActifs = from j in db.Jeux
                             where j.Statut.nomStatut == "Actif"
                             select j;

            foreach (MembreESports etu in db.MembreESports)
            {
                lstEtudiants.Add(new SelectListItem
                {
                    Text = etu.nomComplet,
                    Value = etu.id.ToString()                    
                });
            }

            lstJeuxSecondaires.Add(new SelectListItem { Text = "----------------------------", Value = 0.ToString() });

            foreach (Jeu jeu in jeuxActifs)
            {
                lstJeux.Add(new SelectListItem
                {
                    Text = jeu.nomJeu,
                    Value = jeu.id.ToString()
                });

                lstJeuxSecondaires.Add(new SelectListItem
                {
                    Text = jeu.nomJeu,
                    Value = jeu.id.ToString()
                });
            }

            ViewBag.lstEtudiants = lstEtudiants;
            ViewBag.lstJeux = lstJeux;
            ViewBag.lstJeuxSecondaires = lstJeuxSecondaires;

            return View();
        }

        // POST: Profil/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inscription([Bind(Include = "id,pseudo,courriel,note,JeuId,estArchive,JeuSecondaireId")] Profil profil)
        {
            string UtilisateurId = User.Identity.GetUserId();

            MembreESports etu = db.MembreESports.Find(UtilisateurId);
            Jeu jeu = db.Jeux.Find(profil.JeuId);

                var equipeMonojoueurExistante = from e in db.Equipes
                                                where (e.estMonojoueur == true) && (e.nomEquipe == etu.nomComplet + "_" + jeu.abreviation + "_" + UtilisateurId)
                                                select e;

            if (profil.JeuId == profil.JeuSecondaireId)
            {
                this.AddToastMessage("Inscription invalide.", "Veuillez choisir un jeu secondaire différent du jeu primaire ou tout simplement aucun.", Toast.ToastType.Error, true);
            }
            else
            {
                if (!equipeMonojoueurExistante.Any() && (profil.JeuId != 0))
                {
                    if (ModelState.IsValid)
                    {
                        profil.MembreESportsId = User.Identity.GetUserId();

                        if (!db.Profils.Where(p => p.MembreESportsId == profil.MembreESportsId && p.JeuId == profil.JeuId && p.estArchive == false).Any())
                        {
                            db.Profils.Add(profil);
                            db.SaveChanges();

                            if (profil.jeuSecondaire != null)
                                this.AddToastMessage("Inscription confirmée.", "Votre inscription pour « " + jeu.nomJeu + " » et « " + profil.jeuSecondaire.nomJeu + " » a été ajoutée à la liste.", Toast.ToastType.Success);
                            else
                                this.AddToastMessage("Inscription confirmée.", "Votre inscription pour « " + jeu.nomJeu + " » a été ajoutée à la liste.", Toast.ToastType.Success);

                            return RedirectToAction("Index");
                            //if (User.IsInRole("Admin eSports"))
                            //    return RedirectToAction("Index");
                            //else
                            //    return RedirectToAction("ESport", "Accueil");
                        }
                        else
                            this.AddToastMessage("Erreur de validation.", "Vous avez déjà appliqué pour « " + jeu.nomJeu + " ».", Toast.ToastType.Error, true);
                    }
                }
                else
                {
                    this.AddToastMessage("Inscription invalide.", "Vous êtes déjà un joueur de « " + jeu.nomJeu + " ».", Toast.ToastType.Error, true);
                }
            }
            List<SelectListItem> lstEtudiants = new List<SelectListItem>();
            List<SelectListItem> lstJeux = new List<SelectListItem>();
            List<SelectListItem> lstJeuxSecondaires = new List<SelectListItem>();

            var jeuxActifs = from j in db.Jeux
                             where j.Statut.nomStatut == "Actif"
                             select j;

            foreach (MembreESports etudiant in db.MembreESports)
            {
                lstEtudiants.Add(new SelectListItem
                {
                    Text = etudiant.nomComplet,
                    Value = etudiant.id.ToString()
                });
            }

            lstJeuxSecondaires.Add(new SelectListItem { Text = "----------------------------", Value = 0.ToString() });

            foreach (Jeu jeuActif in jeuxActifs)
            {
                lstJeux.Add(new SelectListItem
                {
                    Text = jeuActif.nomJeu,
                    Value = jeuActif.id.ToString()
                });

                lstJeuxSecondaires.Add(new SelectListItem
                {
                    Text = jeuActif.nomJeu,
                    Value = jeuActif.id.ToString()
                });
            }

            ViewBag.lstEtudiants = lstEtudiants;
            ViewBag.lstJeux = lstJeux;
            ViewBag.lstJeuxSecondaires = lstJeuxSecondaires;

            return View(profil);
        }

        // GET: Profil/Edit/5
        public ActionResult Modifier(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profil profil = db.Profils.Find(id);
            if (profil == null)
            {
                return HttpNotFound();
            }
            ViewBag.EtudiantId = new SelectList(db.MembreESports, "id", "nom", profil.MembreESportsId);
            ViewBag.JeuId = new SelectList(db.Jeux, "id", "nomJeu", profil.JeuId);
            return View(profil);
        }

        // POST: Profil/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "id,pseudo,courriel,note,EtudiantId,JeuId")] Profil profil)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profil).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EtudiantId = new SelectList(db.MembreESports, "id", "nom", profil.MembreESportsId);
            ViewBag.JeuId = new SelectList(db.Jeux, "id", "nomJeu", profil.JeuId);
            return View(profil);
        }

        public ActionResult Valider(int? id, int? JeuId)
        {
            Joueur joueur = new Joueur();
            Equipe equipeMonojoueur = new Equipe();
            Jeu jeu = new Jeu();

            Profil profil = db.Profils.Find(id);

            if(JeuId == null)
            {
                jeu = db.Jeux.Find(profil.JeuId);
            }
            else
            {
                jeu = db.Jeux.Find(JeuId);
                profil.JeuSecondaireId = profil.JeuId;
                profil.Jeu = jeu;
            }
                                        
            profil.estArchive = true;

            joueur.pseudoJoueur = profil.pseudo;
            joueur.MembreESportsId = profil.MembreESportsId;
            joueur.Profil = profil;

            var equipeMonojoueurJeu = from e in db.Equipes
                                      where (e.estMonojoueur == true) && (e.nomEquipe == profil.MembreESports.nomComplet + "_" + jeu.abreviation + "_" + profil.MembreESportsId)
                                      select e;

            if (!equipeMonojoueurJeu.Any())
            {
                equipeMonojoueur.nomEquipe = profil.MembreESports.nomComplet + "_" + jeu.abreviation + "_" + profil.MembreESportsId;
                equipeMonojoueur.JeuId = jeu.id;
                equipeMonojoueur.estMonojoueur = true;

                db.Joueurs.Add(joueur);
                db.Equipes.Add(equipeMonojoueur);
                db.SaveChanges();
                this.AddToastMessage("Validation d'inscription effectuée.", profil.MembreESports.nomComplet + " est désormais un joueur de « " + jeu.nomJeu + " »!", Toast.ToastType.Success);

                return RedirectToAction("Index", "Joueur");
            }

            else
            {
                this.AddToastMessage("Erreur de validation.", profil.MembreESports.nomComplet + " est déjà validé en tant que joueur pour « " + profil.Jeu.nomJeu + " ».", Toast.ToastType.Error, true);
                return RedirectToAction("Index", "Profil");
            }         
        }

        // GET: Profil/Delete/5
        public ActionResult Supprimer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Profil profil = db.Profils.Find(id);

            if(profil.jeuSecondaire != null)
            {
                this.AddToastMessage("Suppression d'inscription.", "L'inscription déposée par " + profil.MembreESports.nomComplet + " pour « " + profil.Jeu.nomJeu + " » et « " + profil.jeuSecondaire.nomJeu + " » a été retirée de la liste.", Toast.ToastType.Success);
            }
            else
            {
                this.AddToastMessage("Suppression d'inscription.", "L'inscription déposée par " + profil.MembreESports.nomComplet + " pour « " + profil.Jeu.nomJeu + " » a été retirée de la liste.", Toast.ToastType.Success);
            }

            db.Profils.Remove(profil);
            db.SaveChanges();
            
            if (profil == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        // POST: Profil/Delete/5
        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmationSupprimer(int id)
        {
            Profil profil = db.Profils.Find(id);
            db.Profils.Remove(profil);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
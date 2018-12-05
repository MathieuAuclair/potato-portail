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
using ApplicationPlanCadre.ViewModels.eSportsVM;

namespace ApplicationPlanCadre.Controllers
{
    public class JoueurController : Controller
    {
        private BDPlanCadre db = new BDPlanCadre();

        // GET: Joueur
        public ActionResult Index(string sortOrder)
        {
            ViewBag.NomSortParm = String.IsNullOrEmpty(sortOrder) ? "nom_desc" : "";
            ViewBag.PrenomSortParm = String.IsNullOrEmpty(sortOrder) ? "prenom_desc" : "";
            ViewBag.PseudoSortParm = String.IsNullOrEmpty(sortOrder) ? "pseudo_desc" : "";
            ViewBag.JeuSortParm = String.IsNullOrEmpty(sortOrder) ? "jeu_desc" : "";

            var joueurs = from j in db.Joueurs
                          select j;

            switch (sortOrder)
            {
                case "nom_desc":
                    joueurs = joueurs.OrderByDescending(j => j.MembreESports.nom);
                    break;
                case "prenom_desc":
                    joueurs = joueurs.OrderBy(j => j.MembreESports.prenom);
                    break;
                case "pseudo_desc":
                    joueurs = joueurs.OrderBy(j => j.pseudoJoueur);
                    break;
                case "jeu_desc":
                    joueurs = joueurs.OrderBy(j => j.Profil.Jeu.nomJeu);
                    break;
                default:
                    joueurs = joueurs.OrderBy(j => j.MembreESports.nom);
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
            Joueur joueur = db.Joueurs.Find(id);
            if (joueur == null)
            {
                return HttpNotFound();
            }
            return View(joueur);
        }

        // GET: Joueur/Create
        public ActionResult Creation()
        {
            var etudiants = db.MembreESports.ToList();
            var etus = etudiants.OrderBy(e => e.nom);

            List<SelectListItem> lstEtudiants = new List<SelectListItem>();

            foreach (MembreESports etu in etus)
            {
                lstEtudiants.Add(new SelectListItem
                {
                    Text = etu.prenom + " " + etu.nom,
                    Value = etu.id.ToString()
                });
            }
            
            ViewBag.EtudiantId = lstEtudiants;
            return View();
        }

        // POST: Joueur/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "id,pseudoJoueur,EtudiantId,ProfilId")] Joueur joueur)
        {
            if (ModelState.IsValid)
            {
                db.Joueurs.Add(joueur);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EtudiantId = new SelectList(db.MembreESports, "id", "id", joueur.MembreESportsId);
            return View(joueur);
        }

        // GET: Joueur/Edit/5
        public ActionResult Modifier(int? id)
        {
            EditerJoueurVm viewModel = new EditerJoueurVm();

            Joueur joueur = db.Joueurs.Find(id);
            Profil profil = db.Profils.Find(joueur.Profil.id);

            viewModel.JoueurId = joueur.id;
            viewModel.pseudo = joueur.pseudoJoueur;
            viewModel.courriel = profil.courriel;
            viewModel.MembreESports = db.MembreESports.Find(joueur.MembreESportsId);
            viewModel.Jeu = db.Jeux.Find(profil.JeuId);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }     

            if (joueur == null)
            {
                return HttpNotFound();
            }            

            return View(viewModel);
        }

        // POST: Joueur/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "JoueurId,pseudo,courriel")]EditerJoueurVm viewModel)
        {
            Joueur joueur = db.Joueurs.Find(viewModel.JoueurId);
            Profil profil = db.Profils.Find(joueur.Profil.id);

            if (ModelState.IsValid)
            {
                joueur.pseudoJoueur = viewModel.pseudo;
                profil.pseudo = viewModel.pseudo;
                profil.courriel = viewModel.courriel;
                db.SaveChanges();
                this.AddToastMessage("Modifications apportées.", "Les changements ont été sauvegardés.", Toast.ToastType.Success);
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        // GET: Joueur/Delete/5
        public ActionResult Supprimer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Joueur joueur = db.Joueurs.Find(id);
            if (joueur == null)
            {
                return HttpNotFound();
            }
            return View(joueur);
        }

        // POST: Joueur/Delete/5
        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmationSupprimer(int id)
        {
            Joueur joueur = db.Joueurs.Find(id);
            MembreESports etu = db.MembreESports.Find(joueur.MembreESportsId);
            Profil profil = db.Profils.Find(joueur.Profil.id);
            Jeu jeu = db.Jeux.Find(joueur.equipeMonojoueur.JeuId);
            
            var equipeMonojoueur = from e in db.Equipes
                                   join j in db.Jeux on e.JeuId equals j.id
                                   join p in db.Profils on j.id equals p.JeuId
                                   where (e.nomEquipe == etu.nomComplet + "_" + jeu.abreviation + "_" + profil.MembreESportsId) && (p.id == profil.id)
                                   select e;

            this.AddToastMessage("Supression effectuée.", etu.nomComplet + " n'est plus un joueur de « " + jeu.nomJeu + " ».", Toast.ToastType.Success);

            db.Joueurs.Remove(joueur);
            db.Equipes.Remove(equipeMonojoueur.First());
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
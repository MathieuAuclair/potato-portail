using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ApplicationPlanCadre.Models.eSports;
using PotatoPortail.Migrations;
using PotatoPortail.Models;
using PotatoPortail.Models.eSports;

namespace PotatoPortail.Controllers.Esports
{
    public class ProfilController : Controller
    {
        private readonly DatabaseContext _db = new DatabaseContext();

        public ActionResult Index()
        {
            var inscriptions = from p in _db.Profils
                where p.EstArchive == false
                select p;

            ViewBag.inscriptions = inscriptions.ToList();
            ViewBag.msgJoueurExistant = TempData["msgJoueurExistant"];

            var profils = _db.Profils.Include(p => p.Etudiant).Include(p => p.Jeu).Where(p => p.EstArchive == false);
            return View(profils.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Profil profil = _db.Profils.Find(id);

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
                where j.Statut.nomStatut == "Actif"
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inscription(
            [Bind(Include = "id,pseudo,courriel,note,EtudiantId,JeuId,estArchive,JeuSecondaireId")]
            Profil profil)
        {
            Etudiant etu = _db.Etudiant.Find(profil.EtudiantId);
            Jeu jeu = _db.Jeux.Find(profil.JeuId);

            var equipeMonojoueurExistante = from e in _db.Equipes
                where e.estMonojoueur &&
                      (e.nomEquipe == etu.Prenom + etu.NomDeFamille + "_" + jeu.abreviation + "_" + profil.EtudiantId)
                select e;

            if (profil.JeuId == profil.JeuSecondaireId)
            {
                ViewBag.jeuxSelectionnesIdentiques = "Le jeu primaire et le jeu secondaire ne peuvent être les mêmes!";
            }
            else
            {
                if (!equipeMonojoueurExistante.Any() && (profil.JeuId != 0))
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
                where j.Statut.nomStatut == "Actif"
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

            ViewBag.EtudiantId = new SelectList(_db.Etudiant, "id", "nom", profil.EtudiantId);
            ViewBag.JeuId = new SelectList(_db.Jeux, "id", "nomJeu", profil.JeuId);
            return View(profil);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "id,pseudo,courriel,note,EtudiantId,JeuId")]
            Profil profil)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(profil).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EtudiantId = new SelectList(_db.Etudiant, "id", "nom", profil.EtudiantId);
            ViewBag.JeuId = new SelectList(_db.Jeux, "id", "nomJeu", profil.JeuId);
            return View(profil);
        }

        public ActionResult Valider(int? id, int? jeuId)
        {
            Joueur joueur = new Joueur();
            Equipe equipeMonojoueur = new Equipe();

            Profil profil = _db.Profils.Find(id);

            if (profil == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            Jeu jeu = _db.Jeux.Find(profil.JeuId) ?? _db.Jeux.Find(jeuId);

            if (jeu == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            profil.EstArchive = true;

            joueur.PseudoJoueur = profil.Pseudo;
            joueur.EtudiantId = profil.EtudiantId;
            joueur.Profil = profil;

            var equipeMonojoueurJeu = from e in _db.Equipes
                where (e.estMonojoueur == true) &&
                      (e.nomEquipe == profil.Etudiant.Prenom + profil.Etudiant.NomDeFamille + "_" + jeu.abreviation +
                       "_" + profil.EtudiantId)
                select e;
            if (!equipeMonojoueurJeu.Any())
            {
                equipeMonojoueur.nomEquipe =
                    profil.Etudiant.Prenom + profil.Etudiant.NomDeFamille + "_" + jeu.abreviation + "_" +
                    profil.EtudiantId;
                equipeMonojoueur.JeuId = jeu.id;
                equipeMonojoueur.estMonojoueur = true;

                _db.Joueurs.Add(joueur);
                _db.Equipes.Add(equipeMonojoueur);
                _db.SaveChanges();

                return RedirectToAction("Index", "Joueur");
            }

            else

            {
                TempData["msgJoueurExistant"] = profil.Etudiant.Prenom + profil.Etudiant.NomDeFamille +
                                                " est déjà validé en tant que joueur pour " + profil.Jeu.nomJeu + ".";
                return RedirectToAction("Index", "Profil");
            }
        }

        public ActionResult Supprimer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Profil profil = _db.Profils.Find(id);

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
            Profil profil = _db.Profils.Find(id);
            _db.Profils.Remove(profil);
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
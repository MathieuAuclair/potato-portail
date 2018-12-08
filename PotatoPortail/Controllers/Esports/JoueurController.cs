using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models.eSports;
using PotatoPortail.Toast;
using PotatoPortail.ViewModels.eSports;

namespace PotatoPortail.Controllers.eSports
{
    public class JoueurController : Controller
    {
        private readonly BdPortail _db = new BdPortail();
        
        public ActionResult Index(string sortOrder)
        {
            ViewBag.NomSortParm = string.IsNullOrEmpty(sortOrder) ? "nom_desc" : "";
            ViewBag.PrenomSortParm = string.IsNullOrEmpty(sortOrder) ? "prenom_desc" : "";
            ViewBag.PseudoSortParm = string.IsNullOrEmpty(sortOrder) ? "pseudo_desc" : "";
            ViewBag.JeuSortParm = string.IsNullOrEmpty(sortOrder) ? "jeu_desc" : "";

            var joueurs = from tableJoueurs in _db.Joueur
                select tableJoueurs;

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
                    joueurs = joueurs.OrderBy(j => j.Profil.Jeu.NomJeu);
                    break;
                default:
                    joueurs = joueurs.OrderBy(j => j.MembreESports.Nom);
                    break;
            }

            return View(joueurs.ToList());
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var joueur = _db.Joueur.Find(id);
            if (joueur == null)
            {
                return HttpNotFound();
            }

            return View(joueur);
        }

        public ActionResult Creation()
        {
            var etudiants = _db.MembreESports.ToList();
            var etus = etudiants.OrderBy(e => e.Nom);

            var lstEtudiants = etus
                .Select(etu => new SelectListItem {Text = etu.Prenom + " " + etu.Nom, Value = etu.Id}).ToList();

            ViewBag.EtudiantId = lstEtudiants;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "id,pseudoJoueur,EtudiantId,ProfilId")]
            Joueur joueur)
        {
            if (ModelState.IsValid)
            {
                _db.Joueur.Add(joueur);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EtudiantId = new SelectList(_db.MembreESports, "id", "id", joueur.IdMembreESports);
            return View(joueur);
        }

        public ActionResult Modifier(int? id)
        {
            var viewModel = new EditerJoueurViewModel();

            var joueur = _db.Joueur.Find(id);

            if (joueur == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var profil = _db.Profil.Find(joueur.Profil.Id);

            if (profil == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            viewModel.JoueurId = joueur.Id;
            viewModel.Pseudo = joueur.PseudoJoueur;
            viewModel.Courriel = profil.Courriel;
            viewModel.MembreESports = _db.MembreESports.Find(joueur.IdMembreESports);
            viewModel.Jeu = _db.Jeu.Find(profil.IdJeu);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "JoueurId,pseudo,courriel")]
            EditerJoueurViewModel viewModel)
        {
            var joueur = _db.Joueur.Find(viewModel.JoueurId);

            if (joueur == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var profil = _db.Profil.Find(joueur.Profil.Id);

            if (ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            joueur.PseudoJoueur = viewModel.Pseudo;

            if (profil == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            profil.Pseudo = viewModel.Pseudo;
            profil.Courriel = viewModel.Courriel;
            _db.SaveChanges();
            this.AddToastMessage("Modifications apportées.", "Les changements ont été sauvegardés.",
                ToastType.Success);
            return RedirectToAction("Index");
        }
        
        public ActionResult Supprimer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var joueur = _db.Joueur.Find(id);

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
            var joueur = _db.Joueur.Find(id);

            if (joueur == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var membreESports = _db.MembreESports.Find(joueur.IdMembreESports);
            var profil = _db.Profil.Find(joueur.Profil.Id);
            var jeu = _db.Jeu.Find(joueur.EquipeMonojoueur.IdJeu);

            if (profil == null || jeu == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var equipeMonojoueur = from tableEquipe in _db.Equipe
                join tableJeu in _db.Jeu on tableEquipe.IdJeu equals tableJeu.Id
                join tableProfil in _db.Profil on tableJeu.Id equals tableProfil.IdJeu
                where (tableEquipe.NomEquipe == membreESports.NomComplet + "_" + jeu.Abreviation + "_" + profil.IdMembreESports) &&
                      (tableProfil.Id == profil.Id)
                select tableEquipe;

            if (membreESports != null)
                this.AddToastMessage("Supression effectuée.",
                    membreESports.NomComplet + " n'est plus un joueur de « " + jeu.NomJeu + " ».", ToastType.Success);

            _db.Joueur.Remove(joueur);
            _db.Equipe.Remove(equipeMonojoueur.First());
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
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models;
using PotatoPortail.Models.eSports;
using PotatoPortail.Toast;
using PotatoPortail.ViewModels.eSports;

namespace PotatoPortail.Controllers.eSports
{
    public class EquipeController : Controller
    {
        private readonly BdPortail _db = new BdPortail();
        
        public ActionResult Index(int? searchIdJeu)
        {
            var Jeu = from tableJeu in _db.Jeu
                       orderby tableJeu.Statut.NomStatut, tableJeu.NomJeu
                       select tableJeu;

            List<SelectListItem> lstJeu = new List<SelectListItem>
            {
                new SelectListItem {Text = "Tous les jeux", Value = "0"}
            };
            lstJeu.AddRange(Jeu.Select(jeu => new SelectListItem {Text = jeu.NomJeu + " (" + jeu.Statut.NomStatut + ")", Value = jeu.Id.ToString()}));


            ViewBag.Jeu = lstJeu;

            var Equipe = from e in _db.Equipe
                          where e.EstMonoJoueur == false
                          select e;

            if (searchIdJeu == 0)
            {
                Session["dernierTriApplique"] = null;
                goto fin;
            }

            if (searchIdJeu != null)
            {
                Equipe = Equipe.Where(equipe => equipe.Jeu.Id == searchIdJeu);

                Session["dernierTriApplique"] = searchIdJeu;
            }

            if (Session["dernierTriApplique"] != null)
            {
                var idJeuEquipe = Convert.ToInt32(Session["dernierTriApplique"]);

                var lstJeuJeuSelectionne = lstJeu.FirstOrDefault(j => j.Value == idJeuEquipe.ToString());

                if (lstJeuJeuSelectionne != null)
                    lstJeuJeuSelectionne.Selected = true;

                if (lstJeuJeuSelectionne != null && lstJeuJeuSelectionne.Value != "0")
                {
                    var arret = lstJeuJeuSelectionne.Text.IndexOf(" (", StringComparison.Ordinal);
                    ViewBag.TriSelectionne = lstJeuJeuSelectionne.Text.Substring(0, arret);
                }
                else
                    ViewBag.TriSelectionne = "Tous les jeux";

                return View(Equipe.OrderBy(e => e.NomEquipe).Where(e => e.IdJeu == idJeuEquipe));
            }

            fin:

            ViewBag.TriSelectionne = "Tous les jeux";

            return View(Equipe.OrderBy(e => e.NomEquipe).Where( e => e.Jeu.Statut.Id == 1).ToList());
        }

        public ActionResult Details(int? id, string nomEquipe, string nomJeu)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipe equipe = _db.Equipe.Find(id);

            if (equipe == null)
            {
                return HttpNotFound();
            }

            ViewBag.nomEquipe = nomEquipe;
            ViewBag.nomJeu = nomJeu;

            return View(equipe);
        }

        public ActionResult Creation()
        {
            var equipeAAjouter = new CreationEquipeViewModel {Entraineurs = new List<string>()};

            PopulerEntraineurSelectList();
            PopulerListJeuActifs();

            return View(equipeAAjouter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "id,nomEquipe,IdJeu,Entraineurs")] CreationEquipeViewModel equipeAAjouter, string[] entraineur, string button)
        {
            PopulerListJeuActifs();
            PopulerEntraineurSelectList();

            if (EstCeQueEquipeUnique(equipeAAjouter))
            {
                this.AddToastMessage("Ajout d'équipe annulé.", "Le nom « " + equipeAAjouter.NomEquipe + " » est déjà utilisé.", ToastType.Error);
                return View("Creation");
            }

            Equipe nouvelleEquipe = new Equipe { IdJeu = equipeAAjouter.IdJeu, NomEquipe = equipeAAjouter.NomEquipe, EstMonoJoueur = equipeAAjouter.EstMonoJoueur };
            
            ActualiserEquipeEntraineur(entraineur, nouvelleEquipe);

            try
            {
                _db.Equipe.Add(nouvelleEquipe);
                _db.SaveChanges();
                this.AddToastMessage("Ajout d'équipe effectué.", "« " + equipeAAjouter.NomEquipe + " » a été ajoutée à la liste des équipes.", ToastType.Success);
                if (button == "Ajouter des joueurs")
                {
                    ViewBag.nomJeu = nouvelleEquipe.Jeu.NomJeu;
                    PopulerJoueurSelectList(nouvelleEquipe.Jeu.NomJeu);

                    return RedirectToAction("Modifier", new { nouvelleEquipe.Id, nouvelleEquipe.Jeu.NomJeu });
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
            catch
            {
                this.AddToastMessage("Ajout d'équipe annulé.", "Une erreur est survenue", ToastType.Error);
                return RedirectToAction("Creation");
            }
        }

        private bool EstCeQueEquipeUnique(CreationEquipeViewModel EquipePourAjout)
        {
            return (from tableEquipe in _db.Equipe
                where tableEquipe.NomEquipe.Equals(EquipePourAjout.NomEquipe, StringComparison.OrdinalIgnoreCase) && tableEquipe.IdJeu == EquipePourAjout.IdJeu
                select tableEquipe).Any();
        }
        
        public ActionResult Modifier(int? id, string nomJeu, bool? rappelleDetailsJeu)
        {
            var equipeAModifierViewModel = new ModifierEquipeViewModel();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var equipeAModifierQuery = from equipe in _db.Equipe
                                       where equipe.Id == id
                                       select equipe;

            var equipeAModifier = equipeAModifierQuery.First();

            if (equipeAModifier == null)
            {
                return HttpNotFound();
            }


            EquipeAEquipeViewModel(equipeAModifier, equipeAModifierViewModel);

            PopulerEntraineurSelectList();
            PopulerJoueurSelectList(nomJeu);

            ViewBag.rappelleDetailsJeu = rappelleDetailsJeu;

            return View(equipeAModifierViewModel);

        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier(int? id, string nomEquipe, string[] entraineur, string[] joueurs)
        {            
            PopulerListJeuActifs();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var equipePourModification = _db.Equipe
                .Include(queryEquipe => queryEquipe.Entraineur)
                .Single(queryEquipe => queryEquipe.Id == id);

            if (EstCeQueNomEquipeEstUnique(equipePourModification))
            {
                return View("Modifier", equipePourModification);
            }

            PopulerJoueurSelectList(equipePourModification.Jeu.NomJeu);
            PopulerEntraineurSelectList();
            equipePourModification.NomEquipe = nomEquipe;

            if (!TryUpdateModel(equipePourModification, "",
                new[] {"id,nomEquipe,JeuId,Entraineur,Joueur"})) return View(equipePourModification);
            try
            {
                ActualiserEquipeEntraineur(entraineur, equipePourModification);
                ActualiserEquipeJoueur(joueurs, equipePourModification);
                _db.SaveChanges();
                this.AddToastMessage("Modifications apportées.", "Les changements apportés à l'équipe « " + equipePourModification.NomEquipe + " » ont été sauvegardés.", ToastType.Success);
                if(TempData["retourDetails"] != null)
                {
                    if ((bool)TempData["retourDetails"] == true)
                    {
                        return RedirectToAction("Details", "Jeu", new { id = equipePourModification.IdJeu, nomJeu = equipePourModification.Jeu.NomJeu });
                    }
                }
                return RedirectToAction("Index");

            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Impossible de sauvegarder");
            }
            
            return View(equipePourModification);
        }

        private bool EstCeQueNomEquipeEstUnique(Equipe equipePourModification)
        {
           return (from tableEquipe in _db.Equipe
                where tableEquipe.NomEquipe.Equals(equipePourModification.NomEquipe, StringComparison.OrdinalIgnoreCase) &&
                      tableEquipe.IdJeu == equipePourModification.IdJeu &&
                      tableEquipe.Id != equipePourModification.Id
                select tableEquipe).Any();
        }
        
        public ActionResult Supprimer(int? id, string nomEquipe, string nomJeu)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipe equipe = _db.Equipe.Find(id);

            if (equipe == null)
            {
                return HttpNotFound();
            }

            ViewBag.nomEquipe = nomEquipe;
            ViewBag.nomJeu = nomJeu;

            return View(equipe);
        }
        
        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmationSupprimer(int id)
        {
            Equipe equipe = _db.Equipe.Find(id);
            _db.Equipe.Remove(equipe);
            _db.SaveChanges();
            this.AddToastMessage("Suppression effectuée.", "« " + equipe.NomEquipe + " » a été supprimée de la liste.", ToastType.Success);
            return RedirectToAction("Index");
        }

        private void ActualiserEquipeEntraineur(string[] entraineursSelectionne, Equipe equipePourModification)
        {
            if (entraineursSelectionne == null)
            {
                equipePourModification.Entraineur = new List<Entraineur>();
                return;
            }
            var entraineurSelectionneHashSet = new HashSet<string>(entraineursSelectionne);
            var equipeEntraineurs = new HashSet<string>(equipePourModification.Entraineur.Select(e => e.PseudoEntraineur));

            foreach (Entraineur entraineur in _db.Entraineur)
            {
                if (entraineurSelectionneHashSet.Contains(entraineur.PseudoEntraineur))
                {
                    if (!equipeEntraineurs.Contains(entraineur.PseudoEntraineur))
                    {
                        equipePourModification.Entraineur.Add(entraineur);
                    }
                }
                else
                {
                    if (equipeEntraineurs.Contains(entraineur.PseudoEntraineur))
                    {
                        equipePourModification.Entraineur.Remove(entraineur);
                    }
                }
            }
        }
        private void ActualiserEquipeJoueur(string[] joueursSelectionnes, Equipe equipeAModifier)
        {
            if (joueursSelectionnes == null)
            {
                equipeAModifier.Joueur = new List<Joueur>();
                equipeAModifier.Joueur.Clear();
                return;
            }
            var joueursSelectionnesHashSet = new HashSet<string>(joueursSelectionnes);
            var equipeJoueurs = new HashSet<string>(equipeAModifier.Joueur.Select(j => j.PseudoJoueur));

            foreach (Joueur joueur in _db.Joueur)
            {
                if (joueursSelectionnesHashSet.Contains(joueur.PseudoJoueur.ToString()))
                {
                    if (!equipeJoueurs.Contains(joueur.PseudoJoueur))
                    {
                        equipeAModifier.Joueur.Add(joueur);
                    }
                }
                else
                {
                    if (equipeJoueurs.Contains(joueur.PseudoJoueur))
                    {
                        equipeAModifier.Joueur.Remove(joueur);
                    }
                }
            }
        }

        private void PopulerListJeuActifs()
        {
            var jeux = from jeuActifs in _db.Jeu
                       where jeuActifs.Statut.NomStatut == "Actif"
                       select jeuActifs;

            var lstJeu = new List<SelectListItem>();

            foreach (var jeu in jeux)
            {
                lstJeu.Add(new SelectListItem
                {
                    Text = jeu.NomJeu,
                    Value = jeu.Id.ToString()
                });
            }

            ViewBag.Jeu = lstJeu;
        }

        private void PopulerEntraineurSelectList()
        {
            var entraineurs = from ent in _db.Entraineur
                              select ent;

            List<SelectListItem> selectListEntraineurs = new List<SelectListItem>();

            foreach (var e in entraineurs)
            {
                selectListEntraineurs.Add(new SelectListItem
                {
                    Text = e.PseudoEntraineur,
                    Value = e.Id.ToString()
                });
            }

            ViewBag.Entraineurs = selectListEntraineurs;

        }

        private void PopulerJoueurSelectList(string nomJeu)
        {
            var tousLesJoueurs = from joueur in _db.Joueur
                                 select joueur;
            
            var joueurs = new List<Joueur>();
            foreach (var joueur in tousLesJoueurs)
            {
                if (joueur.EquipeMonojoueur.Jeu.NomJeu == nomJeu)
                {
                    joueurs.Add(joueur);
                }
            }
            
            var selectListJoueurs = new List<SelectListItem>();

            foreach (var j in joueurs)
            {
                selectListJoueurs.Add(new SelectListItem
                {
                    Text = j.PseudoJoueur,
                    Value = j.Id.ToString()
                });
            }
            ViewBag.Joueurs = selectListJoueurs;

        }

        //Quesser ça calisse dans vie cette fonction là? Sérieux common!
        private static void EquipeAEquipeViewModel(Equipe equipeAModifier, ModifierEquipeViewModel equipeAModifierViewModel)
        {
            equipeAModifierViewModel.EquipeId = equipeAModifier.Id;
            equipeAModifierViewModel.NomEquipe = equipeAModifier.NomEquipe;
            equipeAModifierViewModel.EstMonoJoueur = equipeAModifier.EstMonoJoueur;
            equipeAModifierViewModel.IdJeu = equipeAModifier.Jeu.Id;
            equipeAModifierViewModel.Jeu = equipeAModifier.Jeu;
            equipeAModifierViewModel.Entraineurs = new List<Entraineur>();
            equipeAModifierViewModel.Joueurs = new List<Joueur>();

            foreach (var entraineur in equipeAModifier.Entraineur)
            {
                equipeAModifierViewModel.Entraineurs.Add(entraineur);
            }
            foreach (var joueur in equipeAModifier.Joueur)
            {
                equipeAModifierViewModel.Joueurs.Add(joueur);
            }
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
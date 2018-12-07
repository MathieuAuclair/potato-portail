using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ApplicationPlanCadre.Controllers;
using PotatoPortail.Models;
using PotatoPortail.Toast;
using PotatoPortail.ViewModels.eSports;

namespace PotatoPortail.Controllers.eSports
{
    public class EquipeController : Controller
    {
        private readonly BdPortail _db = new BdPortail();
        
        public ActionResult Index(int? searchIdJeu)
        {
            var jeux = from tableJeux in _db.Jeux
                       orderby tableJeux.Statuts.NomStatut, tableJeux.NomJeu
                       select tableJeux;

            List<SelectListItem> lstJeux = new List<SelectListItem>
            {
                new SelectListItem {Text = "Tous les jeux", Value = "0"}
            };
            lstJeux.AddRange(jeux.Select(jeu => new SelectListItem {Text = jeu.NomJeu + " (" + jeu.Statuts.NomStatut + ")", Value = jeu.Id.ToString()}));


            ViewBag.Jeux = lstJeux;

            var equipes = from e in _db.Equipes
                          where e.EstMonoJoueur == false
                          select e;

            if (searchIdJeu == 0)
            {
                Session["dernierTriApplique"] = null;
                goto fin;
            }

            if (searchIdJeu != null)
            {
                equipes = equipes.Where(equipe => equipe.Jeux.Id == searchIdJeu);

                Session["dernierTriApplique"] = searchIdJeu;
            }

            if (Session["dernierTriApplique"] != null)
            {
                var idJeuEquipes = Convert.ToInt32(Session["dernierTriApplique"]);

                var lstJeuxJeuSelectionne = lstJeux.FirstOrDefault(j => j.Value == idJeuEquipes.ToString());

                if (lstJeuxJeuSelectionne != null)
                    lstJeuxJeuSelectionne.Selected = true;

                if (lstJeuxJeuSelectionne != null && lstJeuxJeuSelectionne.Value != "0")
                {
                    var arret = lstJeuxJeuSelectionne.Text.IndexOf(" (", StringComparison.Ordinal);
                    ViewBag.TriSelectionne = lstJeuxJeuSelectionne.Text.Substring(0, arret);
                }
                else
                    ViewBag.TriSelectionne = "Tous les jeux";

                return View(equipes.OrderBy(e => e.NomEquipe).Where(e => e.IdJeu == idJeuEquipes));
            }

            fin:

            ViewBag.TriSelectionne = "Tous les jeux";

            return View(equipes.OrderBy(e => e.NomEquipe).ToList());
        }

        public ActionResult Details(int? id, string nomEquipe, string nomJeu)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipe equipe = _db.Equipes.Find(id);

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
            PopulerListJeuxActifs();

            return View(equipeAAjouter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "id,nomEquipe,JeuId,Entraineurs")] CreationEquipeViewModel equipeAAjouter, string[] entraineur, string button)
        {
            PopulerListJeuxActifs();
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
                _db.Equipes.Add(nouvelleEquipe);
                _db.SaveChanges();
                this.AddToastMessage("Ajout d'équipe effectué.", "« " + equipeAAjouter.NomEquipe + " » a été ajoutée à la liste des équipes.", ToastType.Success);
                if (button == "Ajouter des joueurs")
                {
                    ViewBag.nomJeu = nouvelleEquipe.Jeux.NomJeu;
                    PopulerJoueurSelectList(nouvelleEquipe.Jeux.NomJeu);

                    return RedirectToAction("Modifier", new { nouvelleEquipe.Id, nouvelleEquipe.Jeux.NomJeu });
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

        private bool EstCeQueEquipeUnique(CreationEquipeViewModel equipesPourAjout)
        {
            return (from tableEquipe in _db.Equipes
                where tableEquipe.NomEquipe.Equals(equipesPourAjout.NomEquipe, StringComparison.OrdinalIgnoreCase) && tableEquipe.IdJeu == equipesPourAjout.IdJeu
                select tableEquipe).Any();
        }
        
        public ActionResult Modifier(int? id, string nomJeu, bool? rappelleDetailsJeu)
        {
            var equipeAModifierViewModel = new ModifierEquipeViewModel();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var equipeAModifierQuery = from equipe in _db.Equipes
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
            PopulerListJeuxActifs();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var equipePourModification = _db.Equipes
                .Include(queryEquipe => queryEquipe.Entraineurs)
                .Single(queryEquipe => queryEquipe.Id == id);

            if (EstCeQueNomEquipeEstUnique(equipePourModification))
            {
                return View("Modifier", equipePourModification);
            }

            PopulerJoueurSelectList(equipePourModification.Jeux.NomJeu);
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
           return (from tableEquipe in _db.Equipes
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
            Equipe equipe = _db.Equipes.Find(id);

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
            Equipe equipe = _db.Equipes.Find(id);
            _db.Equipes.Remove(equipe);
            _db.SaveChanges();
            this.AddToastMessage("Suppression effectuée.", "« " + equipe.NomEquipe + " » a été supprimée de la liste.", ToastType.Success);
            return RedirectToAction("Index");
        }

        private void ActualiserEquipeEntraineur(string[] entraineursSelectionne, Equipe equipePourModification)
        {
            if (entraineursSelectionne == null)
            {
                equipePourModification.Entraineurs = new List<Entraineur>();
                return;
            }
            var entraineurSelectionneHashSet = new HashSet<string>(entraineursSelectionne);
            var equipeEntraineurs = new HashSet<string>(equipePourModification.Entraineurs.Select(e => e.PseudoEntraineur));

            foreach (Entraineur entraineur in _db.Entraineurs)
            {
                if (entraineurSelectionneHashSet.Contains(entraineur.PseudoEntraineur))
                {
                    if (!equipeEntraineurs.Contains(entraineur.PseudoEntraineur))
                    {
                        equipePourModification.Entraineurs.Add(entraineur);
                    }
                }
                else
                {
                    if (equipeEntraineurs.Contains(entraineur.PseudoEntraineur))
                    {
                        equipePourModification.Entraineurs.Remove(entraineur);
                    }
                }
            }
        }
        private void ActualiserEquipeJoueur(string[] joueursSelectionnes, Equipe equipeAModifier)
        {
            if (joueursSelectionnes == null)
            {
                equipeAModifier.Joueurs = new List<Joueur>();
                equipeAModifier.Joueurs.Clear();
                return;
            }
            var joueursSelectionnesHashSet = new HashSet<string>(joueursSelectionnes);
            var equipeJoueurs = new HashSet<string>(equipeAModifier.Joueurs.Select(j => j.PseudoJoueur));

            foreach (Joueur joueur in _db.Joueurs)
            {
                if (joueursSelectionnesHashSet.Contains(joueur.PseudoJoueur.ToString()))
                {
                    if (!equipeJoueurs.Contains(joueur.PseudoJoueur))
                    {
                        equipeAModifier.Joueurs.Add(joueur);
                    }
                }
                else
                {
                    if (equipeJoueurs.Contains(joueur.PseudoJoueur))
                    {
                        equipeAModifier.Joueurs.Remove(joueur);
                    }
                }
            }
        }

        private void PopulerListJeuxActifs()
        {
            var jeux = from jeuxActifs in _db.Jeux
                       where jeuxActifs.Statuts.NomStatut == "Actif"
                       select jeuxActifs;

            var lstJeux = new List<SelectListItem>();

            foreach (var jeu in jeux)
            {
                lstJeux.Add(new SelectListItem
                {
                    Text = jeu.NomJeu,
                    Value = jeu.Id.ToString()
                });
            }

            ViewBag.Jeux = lstJeux;
        }

        private void PopulerEntraineurSelectList()
        {
            var entraineurs = from ent in _db.Entraineurs
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
            //Selectionne tous les joueurs
            var tousLesJoueurs = from joueur in _db.Joueurs
                                 select joueur;
            
            var joueurs = new List<Joueur>();
            foreach (var joueur in tousLesJoueurs)
            {
                if (joueur.EquipeMonojoueur.Jeu.nomJeu == nomJeu)
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
            equipeAModifierViewModel.IdJeu = equipeAModifier.Jeux.Id;
            equipeAModifierViewModel.Jeu = equipeAModifier.Jeux;
            equipeAModifierViewModel.Entraineurs = new List<Entraineur>();
            equipeAModifierViewModel.Joueurs = new List<Joueur>();

            foreach (var entraineur in equipeAModifier.Entraineurs)
            {
                equipeAModifierViewModel.Entraineurs.Add(entraineur);
            }
            foreach (var joueur in equipeAModifier.Joueurs)
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
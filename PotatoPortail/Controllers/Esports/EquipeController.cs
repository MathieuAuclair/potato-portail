using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApplicationPlanCadre.Models;
using ApplicationPlanCadre.Models.eSports;
using ApplicationPlanCadre.ViewModels.eSportsVM;
using System.Net.Http;

namespace ApplicationPlanCadre.Controllers.eSports
{
    public class EquipeController : Controller
    {
        private readonly BDPlanCadre _db = new BDPlanCadre();

        // GET: Equipe
        public ActionResult Index(int? searchIDJeu)
        {
            var jeux = from tableJeux in _db.Jeux
                       orderby tableJeux.Statut.nomStatut, tableJeux.nomJeu
                       select tableJeux;

            List<SelectListItem> lstJeux = new List<SelectListItem>();

            lstJeux.Add(new SelectListItem { Text = "Tous les jeux", Value = "0" });

            foreach (Jeu jeu in jeux)
            {
                lstJeux.Add(new SelectListItem
                {
                    Text = jeu.nomJeu + " (" + jeu.Statut.nomStatut + ")",
                    Value = jeu.id.ToString()
                });
            }

            ViewBag.Jeux = lstJeux;

            var equipes = from e in _db.Equipes
                          where e.estMonojoueur == false
                          select e;

            if (searchIDJeu == 0)
            {
                Session["dernierTriApplique"] = null;
                goto fin;
            }
            else if (searchIDJeu != null)
            {
                equipes = equipes.Where(e => e.Jeu.id == searchIDJeu);

                Session["dernierTriApplique"] = searchIDJeu;
            }

            if (Session["dernierTriApplique"] != null)
            {
                string texteItemSelectionne = Session["dernierTriApplique"].ToString();

                int idJeuEquipes = Convert.ToInt32(Session["dernierTriApplique"]);

                var lstJeuxJeuSelectionne = lstJeux.FirstOrDefault(j => j.Value == idJeuEquipes.ToString());

                if (lstJeuxJeuSelectionne != null)
                    lstJeuxJeuSelectionne.Selected = true;

                if (lstJeuxJeuSelectionne.Value != "0")
                {
                    int arret = lstJeuxJeuSelectionne.Text.IndexOf(" (");
                    ViewBag.TriSelectionne = lstJeuxJeuSelectionne.Text.Substring(0, arret);
                }
                else
                    ViewBag.TriSelectionne = "Tous les jeux";

                return View(equipes.OrderBy(e => e.nomEquipe).Where(e => e.JeuId == idJeuEquipes));
            }

            fin:

            ViewBag.TriSelectionne = "Tous les jeux";

            return View(equipes.OrderBy(e => e.nomEquipe).ToList());
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
            CreationEquipeViewModel equipeAAjouter = new CreationEquipeViewModel();

            equipeAAjouter.Entraineurs = new List<string>();
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

            //Valide l'unicité
            var equipeMemeNom = from tableEquipe in _db.Equipes
                                where tableEquipe.nomEquipe.Equals(equipeAAjouter.NomEquipe, StringComparison.OrdinalIgnoreCase) && tableEquipe.JeuId == equipeAAjouter.JeuID
                                select tableEquipe;

            if (equipeMemeNom.Any())
            {
                this.AddToastMessage("Ajout d'équipe annulé.", "Le nom « " + equipeAAjouter.NomEquipe + " » est déjà utilisé.", Toast.ToastType.Error);
                return View("Creation");
            }

            Equipe nouvelleEquipe = new Equipe { JeuId = equipeAAjouter.JeuID, nomEquipe = equipeAAjouter.NomEquipe, estMonojoueur = equipeAAjouter.EstMonoJoueur };
            
            ActualiserEquipeEntraineur(entraineur, nouvelleEquipe);

            try
            {
                _db.Equipes.Add(nouvelleEquipe);
                _db.SaveChanges();
                this.AddToastMessage("Ajout d'équipe effectué.", "« " + equipeAAjouter.NomEquipe + " » a été ajoutée à la liste des équipes.", Toast.ToastType.Success);
                if (button == "Ajouter des joueurs")
                {
                    ViewBag.nomJeu = nouvelleEquipe.Jeu.nomJeu;
                    PopulerJoueurSelectList(nouvelleEquipe.Jeu.nomJeu);

                    return RedirectToAction("Modifier", new { nouvelleEquipe.id, nouvelleEquipe.Jeu.nomJeu });
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
            catch
            {
                this.AddToastMessage("Ajout d'équipe annulé.", "Une erreur est survenue", Toast.ToastType.Error);
                return RedirectToAction("Creation");
            }
        }

        // GET: Equipe/Modifier/5
        public ActionResult Modifier(int? id, string nomJeu, bool? rappelleDetailsJeu)
        {
            ModifierEquipeViewModel equipeAModifierViewModel = new ModifierEquipeViewModel();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var equipeAModifierQuery = from equipe in _db.Equipes
                                       where equipe.id == id
                                       select equipe;

            Equipe equipeAModifier = equipeAModifierQuery.First();

            if (equipeAModifier == null)
            {
                return HttpNotFound();
            }


            EquipeAEquipeVM(equipeAModifier, equipeAModifierViewModel);

            PopulerEntraineurSelectList();
            PopulerJoueurSelectList(nomJeu);

            ViewBag.rappelleDetailsJeu = rappelleDetailsJeu;

            return View(equipeAModifierViewModel);

        }



        // POST: Equipe/Modifier/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier(int? id, string nomEquipe, string[] entraineur, string[] joueurs)
        {
            PopulerListJeuxActifs();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var equipeAModifier = _db.Equipes
                .Include(queryEquipe => queryEquipe.Entraineur)
                .Where(queryEquipe => queryEquipe.id == id)
                .Single();

            //Valide l'unicité
            var equipeMemeNom = from tableEquipe in _db.Equipes
                                where tableEquipe.nomEquipe.Equals(nomEquipe, StringComparison.OrdinalIgnoreCase) &&
                                      tableEquipe.JeuId == equipeAModifier.JeuId &&
                                      tableEquipe.id != equipeAModifier.id
                                select tableEquipe;

            if (equipeMemeNom.Any())
            {
                //Si équipe déjà existante avec le même jeu.
                return View("Modifier", equipeAModifier);
            }

            PopulerJoueurSelectList(equipeAModifier.Jeu.nomJeu);
            PopulerEntraineurSelectList();
            equipeAModifier.nomEquipe = nomEquipe;

            //Ajout à la BD
            if (TryUpdateModel(equipeAModifier, "",
                new string[] { "id,nomEquipe,JeuId,Entraineur,Joueur" }))
            {
                try
                {
                    ActualiserEquipeEntraineur(entraineur, equipeAModifier);
                    ActualiserEquipeJoueur(joueurs, equipeAModifier);
                    _db.SaveChanges();
                    this.AddToastMessage("Modifications apportées.", "Les changements apportés à l'équipe « " + equipeAModifier.nomEquipe + " » ont été sauvegardés.", Toast.ToastType.Success);
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Impossible de sauvegarder");
                }
            }
            return View(equipeAModifier);
        }

        // GET: Equipe/Delete/5
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

        // POST: Equipe/Delete/5
        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmationSupprimer(int id)
        {
            Equipe equipe = _db.Equipes.Find(id);
            _db.Equipes.Remove(equipe);
            _db.SaveChanges();
            this.AddToastMessage("Suppression effectuée.", "« " + equipe.nomEquipe + " » a été supprimée de la liste.", Toast.ToastType.Success);
            return RedirectToAction("Index");
        }

        private void ActualiserEquipeEntraineur(string[] entraineursSelectionne, Equipe equipeAModifier)
        {
            if (entraineursSelectionne == null)
            {
                equipeAModifier.Entraineur = new List<Entraineur>();
                return;
            }
            var entraineurSelectionneHashSet = new HashSet<string>(entraineursSelectionne);
            var equipeEntraineurs = new HashSet<string>(equipeAModifier.Entraineur.Select(e => e.pseudoEntraineur));

            foreach (Entraineur entraineur in _db.Entraineurs)
            {
                if (entraineurSelectionneHashSet.Contains(entraineur.pseudoEntraineur.ToString()))
                {
                    if (!equipeEntraineurs.Contains(entraineur.pseudoEntraineur))
                    {
                        equipeAModifier.Entraineur.Add(entraineur);
                    }
                }
                else
                {
                    if (equipeEntraineurs.Contains(entraineur.pseudoEntraineur))
                    {
                        equipeAModifier.Entraineur.Remove(entraineur);
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
            var equipeJoueurs = new HashSet<string>(equipeAModifier.Joueur.Select(j => j.pseudoJoueur));

            foreach (Joueur joueur in _db.Joueurs)
            {
                if (joueursSelectionnesHashSet.Contains(joueur.pseudoJoueur.ToString()))
                {
                    if (!equipeJoueurs.Contains(joueur.pseudoJoueur))
                    {
                        equipeAModifier.Joueur.Add(joueur);
                    }
                }
                else
                {
                    if (equipeJoueurs.Contains(joueur.pseudoJoueur))
                    {
                        equipeAModifier.Joueur.Remove(joueur);
                    }
                }
            }
        }

        private void PopulerListJeuxActifs()
        {
            var jeux = from jeuxActifs in _db.Jeux
                       where jeuxActifs.Statut.nomStatut == "Actif"
                       select jeuxActifs;

            List<SelectListItem> lstJeux = new List<SelectListItem>();

            foreach (Jeu jeu in jeux)
            {
                lstJeux.Add(new SelectListItem
                {
                    Text = jeu.nomJeu,
                    Value = jeu.id.ToString()
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
                    Text = e.pseudoEntraineur,
                    Value = e.id.ToString()
                });
            }

            ViewBag.Entraineurs = selectListEntraineurs;

        }

        private void PopulerJoueurSelectList(string nomJeu)
        {
            //Selectionne tous les joueurs
            var tousLesJoueurs = from joueur in _db.Joueurs
                                 select joueur;

            //Filtre les joueurs du jeu seulement
            List<Joueur> joueurs = new List<Joueur>();
            foreach (Joueur joueur in tousLesJoueurs)
            {
                if (joueur.equipeMonojoueur.Jeu.nomJeu == nomJeu)
                {
                    joueurs.Add(joueur);
                }
            }

            //Transforme en List de SelectListItem
            List<SelectListItem> selectListJoueurs = new List<SelectListItem>();

            foreach (var j in joueurs)
            {
                selectListJoueurs.Add(new SelectListItem
                {
                    Text = j.pseudoJoueur,
                    Value = j.id.ToString()
                });
            }
            ViewBag.Joueurs = selectListJoueurs;

        }

        private void EquipeAEquipeVM(Equipe equipeAModifier, ModifierEquipeViewModel equipeAModifierVM)
        {
            equipeAModifierVM.EquipeId = equipeAModifier.id;
            equipeAModifierVM.NomEquipe = equipeAModifier.nomEquipe;
            equipeAModifierVM.EstMonoJoueur = equipeAModifier.estMonojoueur;
            equipeAModifierVM.JeuID = equipeAModifier.Jeu.id;
            equipeAModifierVM.Jeu = equipeAModifier.Jeu;
            equipeAModifierVM.Entraineurs = new List<Entraineur>();
            equipeAModifierVM.Joueurs = new List<Joueur>();

            foreach (Entraineur entraineur in equipeAModifier.Entraineur)
            {
                equipeAModifierVM.Entraineurs.Add(entraineur);
            }
            foreach (Joueur joueur in equipeAModifier.Joueur)
            {
                equipeAModifierVM.Joueurs.Add(joueur);
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
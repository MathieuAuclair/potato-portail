using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ApplicationPlanCadre.Models.eSports;
using PotatoPortail.Migrations;

namespace PotatoPortail.Controllers.Esports
{
    public class EquipeController : Controller
    {
        private readonly BDPortail _db = new BDPortail();

        public ActionResult Index(int? searchIdJeu)
        {
            var jeux = from j in _db.Jeux   
                       orderby j.Statut.nomStatut, j.nomJeu                       
                       select j;

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

            if (searchIdJeu == 0)
            {
                Session["dernierTriApplique"] = null;
                goto fin;
            }
            else if (searchIdJeu != null)
            {
                equipes = equipes.Where(e => e.Jeu.id == searchIdJeu);

                Session["dernierTriApplique"] = searchIdJeu;
            }

            if (Session["dernierTriApplique"] != null)
            {
                string texteItemSelectionne = Session["dernierTriApplique"].ToString();

                int idJeuEquipes = Convert.ToInt32(Session["dernierTriApplique"]);

                var lstJeuxJeuSelectionne = lstJeux.FirstOrDefault(j => j.Value == idJeuEquipes.ToString());
                
                if (lstJeuxJeuSelectionne != null)
                    lstJeuxJeuSelectionne.Selected = true;

                return View(equipes.OrderBy(e => e.nomEquipe).Where(e => e.JeuId == idJeuEquipes));
            }

            fin:
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
            var jeux = from jeuxActifs in _db.Jeux
                       where jeuxActifs.Statut.nomStatut == "Actif"
                       select jeuxActifs;

            List < SelectListItem > lstJeux = new List<SelectListItem>();

            foreach (Jeu jeu in jeux)
            {
                lstJeux.Add(new SelectListItem
                {
                    Text = jeu.nomJeu,
                    Value = jeu.id.ToString()
                });
            }

            ViewBag.Jeux = lstJeux;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "id,nomEquipe,JeuId,estMonojoueur")] Equipe equipe)
        {
            equipe.estMonojoueur = false;

            if (ModelState.IsValid)
            {
                _db.Equipes.Add(equipe);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(equipe);
        }

        public ActionResult Modifier(int? id, string nomEquipe, string nomJeu)
        {
            var jeux = _db.Jeux.ToList();
            List<SelectListItem> lstJeux = new List<SelectListItem>();

            foreach (Jeu jeu in jeux)
            {
                lstJeux.Add(new SelectListItem
                {
                    Text = jeu.nomJeu,
                    Value = jeu.id.ToString()
                });
            }

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
            ViewBag.Jeux = lstJeux;
            ViewBag.nomJeu = nomJeu;

            return View(equipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "id,nomEquipe,JeuId")] Equipe equipe)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(equipe).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(equipe);
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
            if (equipe == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            _db.Equipes.Remove(equipe);
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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApplicationPlanCadre.Models;
using ApplicationPlanCadre.Models.eSports;

namespace ApplicationPlanCadre.Controllers
{
    public class JeuController : Controller
    {
        private BDPlanCadre db = new BDPlanCadre();

    
        public ActionResult Index()
        {
            return View(db.Jeux.OrderBy(j => j.nomJeu).ToList());
        }
        public ActionResult Details(int? id, string nomJeu)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Joueur> lstJoueursJeu = new List<Joueur>();

            Jeu jeu = db.Jeux.Find(id);

            if (jeu == null)
            {
                return HttpNotFound();
            }

            foreach (Joueur joueur in db.Joueurs)
            {
                if (joueur.jeuEquipeMonojoueur == jeu.nomJeu)
                    lstJoueursJeu.Add(joueur);
            }

            ViewBag.lstJoueursJeu = lstJoueursJeu.OrderBy(j => j.pseudoJoueur).ToList();
            ViewBag.nomJeu = nomJeu;

            return View(jeu);
        }
        public ActionResult Creation()
        {
            var statuts = db.Statuts.ToList();
            List<SelectListItem> lstStatuts = new List<SelectListItem>();

            foreach (Statut statut in statuts)
            {
                lstStatuts.Add(new SelectListItem
                {
                    Text = statut.nomStatut,
                    Value = statut.id.ToString()
                });
            }

            ViewBag.Statuts = lstStatuts;
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation([Bind(Include = "id,nomJeu,description,urlReference,abreviation,StatutId")] Jeu jeu)
        {
            var jeuDuMemeNom = from j in db.Jeux
                               where j.nomJeu.Equals(jeu.nomJeu, StringComparison.OrdinalIgnoreCase)
                               select j;

            var abvIdentique = from j in db.Jeux
                               where j.abreviation.Equals(jeu.abreviation, StringComparison.OrdinalIgnoreCase)
                               select j;

            if (jeuDuMemeNom.Any())
            {
                this.AddToastMessage("Jeu déjà existant.", jeuDuMemeNom.First().nomJeu + " est déjà entré dans le système.", Toast.ToastType.Error, true);
                ViewBag.Statuts = new SelectList(db.Statuts, "id", "nomStatut", jeu.StatutId);
                return View(jeu);
            }

            if(abvIdentique.Any())
            {
                this.AddToastMessage("Abréviation déjà utilisée.", "L'abréviation « " + abvIdentique.First().abreviation + " » est déjà utilisée pour « " + abvIdentique.First().nomJeu + " ». Choisissez-en une autre.", Toast.ToastType.Error, true);
                ViewBag.AbvExistante = "L'abréviation " + abvIdentique.First().abreviation + " est déjà utilisée pour " + abvIdentique.First().nomJeu + ".";
                ViewBag.Statuts = new SelectList(db.Statuts, "id", "nomStatut", jeu.StatutId);
                return View(jeu);
            }

            if (ModelState.IsValid)
            {
                db.Jeux.Add(jeu);
                db.SaveChanges();
                this.AddToastMessage("Ajout de jeu effectué.", "« " + jeu.nomJeu + " » a été ajouté à la liste des jeux.", Toast.ToastType.Success);
                return RedirectToAction("Index");
            }
            return View(jeu);
        }
        public ActionResult Modifier(int? id, string nomJeu)
        {
            var caracJeu = from carac in db.Caracteristiques
                           where carac.JeuId == id
                           select carac;

            List<Caracteristique> lstCarac = new List<Caracteristique>();

            foreach (Caracteristique carac in caracJeu)
            {
                lstCarac.Add(carac);
            }

            var statuts = db.Statuts.ToList();

            List < SelectListItem > lstStatuts = new List<SelectListItem>();

            foreach (Statut statut in statuts)
            {
                lstStatuts.Add(new SelectListItem
                {
                    Text = statut.nomStatut,
                    Value = statut.id.ToString()
                });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Jeu jeu = db.Jeux.Find(id);
            if (jeu == null)
            {
                return HttpNotFound();
            }

            ViewBag.nomJeu = nomJeu;
            ViewBag.Statuts = lstStatuts;
            ViewBag.carac = lstCarac;

            return View(jeu);
        }
        [HttpPost]
        public ActionResult EditItem([Bind(Include = "id,nomItem,CaracteristiqueId")] Item item)
        {
            Caracteristique caracteristique = db.Caracteristiques.Find(item.CaracteristiqueId);
          
            Jeu jeu = db.Jeux.Find(caracteristique.JeuId);
            if (ModelState.IsValid)
            {
                caracteristique.Item.Add(item);
                //db.Set<Caracteristique>().AddOrUpdate(caracteristique);
                db.SaveChanges();
               // this.AddToastMessage("Modifications apportées.", "Les changements apportés à « " + caracteristique.nomCaracteristique + " » ont été enregistrés.", Toast.ToastType.Success, true);
                return RedirectToAction("Modifier", new { jeu.id,jeu.nomJeu });
            }
            return View(jeu);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "id,nomJeu,description,urlReference,abreviation,StatutId")] Jeu jeu)
        {
            var caracteristiquesJeu = from c in db.Caracteristiques
                                      where c.JeuId == jeu.id
                                      select c;

            ViewBag.carac = caracteristiquesJeu.ToList();
            ViewBag.nomJeu = db.Jeux.Find(jeu.id).nomJeu;

            var jeuDuMemeNom = from j in db.Jeux
                               where j.nomJeu.Equals(jeu.nomJeu, StringComparison.OrdinalIgnoreCase)
                               select j;

            if (jeuDuMemeNom.Any())
            {
                if (jeu.nomJeu != db.Jeux.Find(jeu.id).nomJeu)
                {
                    this.AddToastMessage("Jeu déjà existant.", jeuDuMemeNom.First().nomJeu + " est déjà entré dans le système.", Toast.ToastType.Error, true);
                    ViewBag.Statuts = new SelectList(db.Statuts, "id", "nomStatut", jeu.StatutId);
                    jeu.nomJeu = db.Jeux.Find(jeu.id).nomJeu;
                    jeu.Statut = db.Jeux.Find(jeu.id).Statut;
                    return View(jeu);
                }
            }

            if (ModelState.IsValid)
            {
                db.Set<Jeu>().AddOrUpdate(jeu);
                db.SaveChanges();
                this.AddToastMessage("Modifications apportées.", "Les changements apportés à « " + jeu.nomJeu + " » ont été enregistrés.", Toast.ToastType.Success);
                return RedirectToAction("Index");
            }
            return View(jeu);
        }

        public ActionResult Supprimer(int? id, string nomJeu)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var equipesJeu = from e in db.Equipes
                             where e.JeuId == id
                             select e;

            Jeu jeu = db.Jeux.Find(id);
            if (jeu == null)
            {
                return HttpNotFound();
            }

            ViewBag.nomJeu = nomJeu;
            ViewBag.equipesJeu = equipesJeu.ToList();

            return View(jeu);
        }
        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmationSupprimer(int id)
        {
            Jeu jeu = db.Jeux.Find(id);
            db.Jeux.Remove(jeu);
            db.SaveChanges();
            this.AddToastMessage("Supression effectuée.", "« " + jeu.nomJeu + " » a été supprimé de la liste.", Toast.ToastType.Success);
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
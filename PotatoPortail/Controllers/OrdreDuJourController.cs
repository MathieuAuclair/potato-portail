using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using ApplicationPlanCadre.Controllers;
using ApplicationPlanCadre.Models.Reunions;
using ApplicationPlanCadre.ViewModels;
using ApplicationPlanCadre.ViewModels.OrdresDuJourVM;
using Microsoft.AspNet.Identity;
using PotatoPortail.Migrations;
using PotatoPortail.Models;
using PotatoPortail.Toast;

namespace PotatoPortail.Controllers
{
    public class OrdreDuJourController : Controller
    {
        private readonly BdPortail _db = new BdPortail();
        
        public ActionResult Index()
        {
            var ordre = GetDixOrdreDuJour(); 
            return View(ordre);
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ordreDuJour = _db.OrdreDuJour.Find(id);
            var sujetPointPrincipal = new List<SujetPointPrincipal>();
            var listeSousPoint = new List<SousPointSujet>();
            if (ordreDuJour == null)
            {
                return HttpNotFound();
            }

            foreach (var item in _db.SujetPointPrincipal)
            {
                if (item.IdOrdreDuJour == id)
                    sujetPointPrincipal.Add(item);
            }
            
            foreach(var item in sujetPointPrincipal)
            {
                var listeSousPointQuery = GetSousPoint(item.IdPointPrincipal);
                if (listeSousPointQuery == null) continue;
                foreach (var sp in listeSousPointQuery)
                {
                    listeSousPoint.Add(sp);
                }
            }

            var ordreDuJourViewModelCreerOdj = new OrdreDuJourViewModel
            {
                ordreDuJour = ordreDuJour,
                sujetPointPrincipal = sujetPointPrincipal,
                listeSousPointSujet = listeSousPoint
            };
            return View(ordreDuJourViewModelCreerOdj);
        }
        
        public ActionResult _TreeView()
        {
            return View();
        }

        public ActionResult Create()
        {
            var repo = new createRepository();
            OrdreDuJourViewModel viewmodel = repo.createLieu();
            
            var programme = GetProgramme();
            var numProg = Convert.ToInt32(programme.First().Discipline);
            
            var listeOrdreDuJour = GetOrdreDuJourSelonModele(numProg);

            viewmodel.ordreDuJour = listeOrdreDuJour.Last();

            return View(viewmodel); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrdreDuJourViewModel ordreDuJourViewModel)
        {
            if (!regexHeure(ordreDuJourViewModel.ordreDuJour))
            {
                this.AddToastMessage("Erreur dans l'entrée de l'heure", "Veuillez entrez le bon format d'heure",
                    ToastType.Error);
                return RedirectToAction("Create", "OrdreDuJour");
            }

            if (!ChkDate(ordreDuJourViewModel))
            {
                this.AddToastMessage("Erreur dans l'entrée de la date", "Veuillez entrez une date ultérieure",
                    ToastType.Error);
                return RedirectToAction("Create", "OrdreDuJour");
            }

            if (!ModelState.IsValid)
            {
                return View(ordreDuJourViewModel);
            }

            InsererOrdreDuJourDansLaBaseDeDonnee(ordreDuJourViewModel);

            this.AddToastMessage("Création d'un ordre du jour", "La création a été effectuée",
                ToastType.Success);
            return RedirectToAction("Index");
        }

        private void InsererOrdreDuJourDansLaBaseDeDonnee(OrdreDuJourViewModel httpBundle)
        {
            var ordreDuJour = httpBundle.ordreDuJour;
            ordreDuJour.IdModeleOrdreDuJour = _db.ModeleOrdreDuJour.First().IdModele;
            ordreDuJour.SujetPointPrincipal = httpBundle.sujetPointPrincipal;
            _db.OrdreDuJour.Add(ordreDuJour);
            _db.SaveChanges();

            var updatedOrdreDuJour = _db.OrdreDuJour.Find(ordreDuJour.IdOdJ);

            if (updatedOrdreDuJour == null)
            {
                throw new NullReferenceException("L'enregistrement de l'ordre du jour ne se fait pas correctement!");
            }
            if(httpBundle.listeIdSousPointCache != null)
            PopulateSousPointDansOrdreDuJour(httpBundle, updatedOrdreDuJour);
        }

        private void PopulateSousPointDansOrdreDuJour(OrdreDuJourViewModel httpBundle, OrdreDuJour ordreDuJour)
        {
            var indexDuSujetSousPoint = 0;

            foreach (int positionDuSujetPointPrincipal in httpBundle.listeIdSousPointCache)
            {

                var sujetPointPrincipal = ordreDuJour.SujetPointPrincipal.ElementAt(positionDuSujetPointPrincipal);
                var sujetSousPoint = httpBundle.listeSousPoint[indexDuSujetSousPoint];
                indexDuSujetSousPoint++;

                InsertSujetSousPoint(sujetSousPoint, sujetPointPrincipal.IdPointPrincipal);
            }
        }

        private void InsertSujetSousPoint(string sujetDuSousPoint, int idDuPointPrincipal)
        {
            _db.Database.ExecuteSqlCommand(
                "Insert into SousPointSujet Values(@SujetSousPoint, @IdSujetPointPrincipal)",
                new SqlParameter("SujetSousPoint", sujetDuSousPoint),
                new SqlParameter("IdSujetPointPrincipal", idDuPointPrincipal)
            );
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ordreDuJour = _db.OrdreDuJour.Find(id);
            var sujetPointPrincipal = new List<SujetPointPrincipal>();
            var listeSousPoint = new List<SousPointSujet>();
            if (ordreDuJour == null)
            {
                return HttpNotFound();
            }

            foreach (var item in _db.SujetPointPrincipal)
            {
                if (item.IdOrdreDuJour == id)
                    sujetPointPrincipal.Add(item);
            }

            foreach(var item in sujetPointPrincipal)
            {
                listeSousPoint.AddRange(_db.SousPointSujet.Where(souspoint => item.IdPointPrincipal == souspoint.IdSujetPointPrincipal));
            }

            OrdreDuJourViewModel ordreDuJourViewModelCreerOdj = new OrdreDuJourViewModel
            {
                ordreDuJour = ordreDuJour,
                sujetPointPrincipal = sujetPointPrincipal,
                listeSousPointSujet = listeSousPoint
            };
            return View(ordreDuJourViewModelCreerOdj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrdreDuJourViewModel ordreDuJourViewModelCreerOdj)
        {
            if (!ModelState.IsValid) return View(ordreDuJourViewModelCreerOdj);
            var cpt = 0;
            _db.Entry(ordreDuJourViewModelCreerOdj.ordreDuJour).State = EntityState.Modified;
            ordreDuJourViewModelCreerOdj.ordreDuJour.IdModeleOrdreDuJour = _db.ModeleOrdreDuJour.First().IdModele;

            if (ordreDuJourViewModelCreerOdj.sujetPointPrincipal != null)
            {
                foreach (var item in _db.SujetPointPrincipal)
                {
                    if (item.IdOrdreDuJour != ordreDuJourViewModelCreerOdj.ordreDuJour.IdOdJ) continue;
                    var updatedItem = item;
                    updatedItem.SujetPoint = ordreDuJourViewModelCreerOdj.sujetPointPrincipal[cpt].SujetPoint;
                    _db.Entry(updatedItem).State = EntityState.Modified;
                    cpt++;
                }
            }

            _db.SaveChanges();
            this.AddToastMessage("Modification d'un ordre du jour", "La modification a été effectuée",
                ToastType.Success);
            return RedirectToAction("Index");

        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ordreDuJour = _db.OrdreDuJour.Find(id);
            if (ordreDuJour == null)
            {
                return HttpNotFound();
            }

            var sujetPointPrincipal = _db.SujetPointPrincipal.Where(item => item.IdOrdreDuJour == id).ToList();

            var ordreDuJourViewModelCreerOdj = new OrdreDuJourViewModel
            {
                ordreDuJour = ordreDuJour, sujetPointPrincipal = sujetPointPrincipal
            };
            return View(ordreDuJourViewModelCreerOdj);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrdreDuJour ordredujour = _db.OrdreDuJour.Find(id);
            foreach (var item in _db.SujetPointPrincipal)
            {
                if (item.IdOrdreDuJour == id)
                {
                    foreach (var souspoint in _db.SousPointSujet)
                    {
                        if (souspoint.IdSujetPointPrincipal == item.IdOrdreDuJour)
                            _db.SousPointSujet.Remove(souspoint);
                    }
                    _db.SujetPointPrincipal.Remove(item);
                }                    
            }
            _db.OrdreDuJour.Remove(ordredujour ?? throw new InvalidOperationException());
            _db.SaveChanges();
            this.AddToastMessage("Suppression d'un ordre du jour", "La suppression a été effectuée",
                ToastType.Success);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "RCP,RCD")]
        public ActionResult ModifierModeleOrdreDuJour()
        {
            var programme = GetProgramme();
            var numProg = Convert.ToInt32(programme.First().Discipline);
            var listeOrdreDuJour = GetOrdreDuJourSelonModele(numProg);

            if (listeOrdreDuJour == null) return View();
            var modeleViewModel = new ModificationModeleViewModel();
            var listeString = new List<string>();
            foreach (var item in listeOrdreDuJour)
            {
                if (modeleViewModel.listPP != null) continue;
                listeString.AddRange(item.SujetPointPrincipal.Select(spp => spp.SujetPoint));
                modeleViewModel.listPP = listeString;
            }

            return View(modeleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifierModeleOrdreDuJour(ModificationModeleViewModel modifModeleVm)
        {
            if (modifModeleVm == null) throw new ArgumentNullException(nameof(modifModeleVm));
            if (!ModelState.IsValid) return View(modifModeleVm);
            var role = User.IsInRole("RCD") ? "D" : "P";
            var programme = GetProgramme();
            
            var numProgramme = Convert.ToInt32(programme.First().Discipline);

            var modele = new ModeleOrdreDuJour
            {
                Role = role,
                NumeroProgramme = numProgramme,
                PointPrincipal = "Default"
            };

            _db.ModeleOrdreDuJour.Add(modele);
            _db.SaveChanges();

            var odj = new OrdreDuJour
            {
                TitreOdJ = "Modele",
                HeureDebutReunion = "15h00",
                HeureFinReunion = "16h00",
                DateOdJ = Convert.ToDateTime("3000-12-25"),
                IdModeleOrdreDuJour = modele.IdModele
            };
            _db.OrdreDuJour.Add(odj);

            foreach (var item in modifModeleVm.listPP)
            {
                var pointPrincipal = new SujetPointPrincipal
                {
                    SujetPoint = item,
                    OrdreDuJour = odj,
                };
                _db.SujetPointPrincipal.Add(pointPrincipal);
            }
            _db.SaveChanges();
            this.AddToastMessage("Modèle enregistré", "Le modèle a bien été enregistré.",
                ToastType.Success);
            return RedirectToAction("Index");
        }

        public ActionResult Info(int? id, int year)
        {
            ViewBag.AnneeODJ = year;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ordre = GetDateOrdreDuJour(year).ToList();

            return View(ordre);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }

            base.Dispose(disposing);
        }

        private static bool ChkDate(OrdreDuJourViewModel ordreDuJourViewModelCreerOdj)
        {
            var validDate = true;

            var date = ordreDuJourViewModelCreerOdj.ordreDuJour.DateOdJ;
            var dateCourante = DateTime.Today;

            if (date < dateCourante)
            {
                validDate = false;
            }

            return validDate;
        }

        public ActionResult Annuler(string currentUrl)
        {
            if (currentUrl == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return Redirect(currentUrl);
        }

        private IQueryable<OrdreDuJour> GetOrdreDuJour()
        {
            return from odj in _db.OrdreDuJour
                   select odj;
        }

        private IEnumerable<OrdreDuJour> GetDateOrdreDuJour(int annee)
        {
            return from odj in _db.OrdreDuJour
                   where odj.DateOdJ.Year == annee
                   select odj;
        }

        private IQueryable<OrdreDuJour> GetDixOrdreDuJour() 
        {
            return (from odj in _db.OrdreDuJour
                    orderby odj.IdOdJ descending
                    select odj).Take(10);
        }

        private List<SujetPointPrincipal> GetPointPrincipal(int id)
        {
            var listeSujetPointPrincipal = new List<SujetPointPrincipal>();
            var listeSujetPointPrincipalQuery = from sujetPointPrincipal in _db.SujetPointPrincipal
                                      where sujetPointPrincipal.IdPointPrincipal == id
                                      select sujetPointPrincipal;
            if (!listeSujetPointPrincipalQuery.Any()) return listeSujetPointPrincipal;
            foreach (var item in listeSujetPointPrincipalQuery)
            {
                listeSujetPointPrincipal.Add(item);
            }
            return listeSujetPointPrincipal;
        }

        private List<SousPointSujet> GetSousPoint(int id)
        {
            List<SousPointSujet> listeSousPoint = new List<SousPointSujet>();
            var listeSousPointQuery = from sousPointSujet in _db.SousPointSujet
                                    where sousPointSujet.IdSujetPointPrincipal == id
                                    select sousPointSujet;
            if (listeSousPointQuery.Count() != 0)
            {
                foreach (var item in listeSousPointQuery)
                {
                    listeSousPoint.Add(item);
                }
            }
            return listeSousPoint;
        }

        private IQueryable<AccesProgramme> GetProgramme()
        {
            var username = User.Identity.GetUserName();
            IQueryable<AccesProgramme> programme = from accesProgramme in _db.AccesProgramme
                                                   where accesProgramme.UserMail == username
                                                   select accesProgramme;
            return programme;
        }

        private List<OrdreDuJour> GetOrdreDuJourSelonModele(int numProg)
        {
            IQueryable<ModeleOrdreDuJour> listeModele = from modeleOrdreDuJour in _db.ModeleOrdreDuJour
                                                        where modeleOrdreDuJour.NumeroProgramme == numProg
                                                        orderby modeleOrdreDuJour.IdModele descending
                                                        select modeleOrdreDuJour;
            var numId = listeModele.First().IdModele;
            var listeOrdreDuJourQuery = from ordreDuJour in _db.OrdreDuJour
                                                       where ordreDuJour.IdModeleOrdreDuJour == numId
                                                       select ordreDuJour;
            var listeOrdreDuJour = new List<OrdreDuJour>();
            foreach(var item in listeOrdreDuJourQuery)
            {
                listeOrdreDuJour.Add(item);
            }
            return listeOrdreDuJour;
        }

        private bool regexHeure(OrdreDuJour odj)
        {
            Regex regex = new Regex(@"[0-9]{1,2}h[0-9]{2}");
            if (regex.Match(odj.HeureDebutReunion).Success)
            {
                if (regex.Match(odj.HeureFinReunion).Success)
                {
                    return true;
                }
            }

            return false;
        }

        

        public ActionResult ListeOrdreDuJour()
        {
            return PartialView(GetOrdreDuJour());
        }

        

        [HttpPost]
        public void MettreAjourOrdre(List<SujetPointPrincipal> listeElement)
        {
            foreach (var item in listeElement)
            {
                var element = _db.SujetPointPrincipal.Find(item.IdPointPrincipal);
                if (element != null)
                {
                    element.IdPointPrincipal = item.IdPointPrincipal;
                }
            }

            _db.SaveChanges();
        } //debut dragdrop
    }
}
using ApplicationPlanCadre.Data;
using ApplicationPlanCadre.Models;
using ApplicationPlanCadre.Models.Reunions;
using ApplicationPlanCadre.ViewModels;
using ApplicationPlanCadre.ViewModels.OrdresDuJourVM;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ApplicationPlanCadre.Controllers
{
    public class OrdreDuJourController : Controller
    {
        private readonly BDPlanCadre _db = new BDPlanCadre();

        // GET: OdJ
        //[Route("Reunions/Index")]
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

            OrdreDuJour OrdreDuJour = _db.OrdreDuJour.Find(id);
            List<SujetPointPrincipal> sujetPointPrincipal = new List<SujetPointPrincipal>();
            List<SousPointSujet> listeSousPoint = new List<SousPointSujet>();
            if (OrdreDuJour == null)
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
                List<SousPointSujet> listeSousPointQuery = GetSousPoint(item.IdPointPrincipal);
                if(listeSousPointQuery != null)
                {
                    foreach (var sp in listeSousPointQuery)
                    {
                        listeSousPoint.Add(sp);
                    }
                }
            }

            OrdreDuJourViewModel ordreDuJourViewModelCreerOdj = new OrdreDuJourViewModel();
            ordreDuJourViewModelCreerOdj.ordreDuJour = OrdreDuJour;
            ordreDuJourViewModelCreerOdj.sujetPointPrincipal = sujetPointPrincipal;
            ordreDuJourViewModelCreerOdj.listeSousPointSujet = listeSousPoint;
            return View(ordreDuJourViewModelCreerOdj);
        }

        //GET: Odj/_TreeView
        public ActionResult _TreeView()
        {
            return View();
        }

        // GET: OdJ/Create
        public ActionResult Create()
        {
            //createRepository populate la drop down liste en créant un viewmodel
            var repo = new createRepository();
            OrdreDuJourViewModel viewmodel = repo.createLieu();

            //Recherche du numéro de programme
            var programme = GetProgramme();
            int NumProg = Convert.ToInt32(programme.First().discipline);

            //recherche de l'ordre du jour basé sur le modèle
            //à l'aide du numéro de programme
            List<OrdreDuJour> listeOrdreDuJour = GetOrdreDuJourSelonModele(NumProg);

            viewmodel.ordreDuJour = listeOrdreDuJour.Last();

            return View(viewmodel); //permet de generer les lieux
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrdreDuJourViewModel ordreDuJourViewModel)
        {
            if (!regexHeure(ordreDuJourViewModel.ordreDuJour))
            {
                this.AddToastMessage("Erreur dans l'entrée de l'heure", "Veuillez entrez le bon format d'heure",
                    Toast.ToastType.Error);
                return RedirectToAction("Create", "OrdreDuJour");
            }

            if (!chkDate(ordreDuJourViewModel))
            {
                this.AddToastMessage("Erreur dans l'entrée de la date", "Veuillez entrez une date ultérieure",
                    Toast.ToastType.Error);
                return RedirectToAction("Create", "OrdreDuJour");
            }

            if (!ModelState.IsValid)
            {
                return View(ordreDuJourViewModel);
            }

            InsererOrdreDuJourDansLaBaseDeDonnee(ordreDuJourViewModel);

            this.AddToastMessage("Création d'un ordre du jour", "La création a été effectuée",
                Toast.ToastType.Success);
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


        // GET: OdJ/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OrdreDuJour OrdreDuJour = _db.OrdreDuJour.Find(id);
            List<SujetPointPrincipal> sujetPointPrincipal = new List<SujetPointPrincipal>();
            List<SousPointSujet> listeSousPoint = new List<SousPointSujet>();
            if (OrdreDuJour == null)
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
                foreach(var souspoint in _db.SousPointSujet)
                {
                    if(item.IdPointPrincipal == souspoint.IdSujetPointPrincipal){
                        listeSousPoint.Add(souspoint);
                    }
                }
            }

            OrdreDuJourViewModel ordreDuJourViewModelCreerOdj = new OrdreDuJourViewModel();
            ordreDuJourViewModelCreerOdj.ordreDuJour = OrdreDuJour;
            ordreDuJourViewModelCreerOdj.sujetPointPrincipal = sujetPointPrincipal;
            ordreDuJourViewModelCreerOdj.listeSousPointSujet = listeSousPoint;
            return View(ordreDuJourViewModelCreerOdj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrdreDuJourViewModel ordreDuJourViewModelCreerOdj)
        {
            if (ModelState.IsValid)
            {
                int cpt = 0;
                _db.Entry(ordreDuJourViewModelCreerOdj.ordreDuJour).State = EntityState.Modified;
                ordreDuJourViewModelCreerOdj.ordreDuJour.IdModeleOrdreDuJour = _db.ModeleOrdreDuJour.First().IdModele;

                if (ordreDuJourViewModelCreerOdj.sujetPointPrincipal != null)
                {
                    foreach (var item in _db.SujetPointPrincipal)
                    {
                        if (item.IdOrdreDuJour == ordreDuJourViewModelCreerOdj.ordreDuJour.IdOdJ)
                        {
                            var updatedItem = item;
                            updatedItem.SujetPoint = ordreDuJourViewModelCreerOdj.sujetPointPrincipal[cpt].SujetPoint;
                            _db.Entry(updatedItem).State = EntityState.Modified;
                            cpt++;
                        }
                    }
                }

                _db.SaveChanges();
                this.AddToastMessage("Modification d'un ordre du jour", "La modification a été effectuée",
                    Toast.ToastType.Success);
                return RedirectToAction("Index");
            }

            return View(ordreDuJourViewModelCreerOdj);
        }

        // GET: OdJ/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OrdreDuJour OrdreDuJour = _db.OrdreDuJour.Find(id);
            List<SujetPointPrincipal> sujetPointPrincipal = new List<SujetPointPrincipal>();
            if (OrdreDuJour == null)
            {
                return HttpNotFound();
            }

            foreach (var item in _db.SujetPointPrincipal)
            {
                if (item.IdOrdreDuJour == id)
                    sujetPointPrincipal.Add(item);
            }

            OrdreDuJourViewModel ordreDuJourViewModelCreerOdj = new OrdreDuJourViewModel();
            ordreDuJourViewModelCreerOdj.ordreDuJour = OrdreDuJour;
            ordreDuJourViewModelCreerOdj.sujetPointPrincipal = sujetPointPrincipal;
            return View(ordreDuJourViewModelCreerOdj);
        }

        // POST: OdJ/Delete/5
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
            _db.OrdreDuJour.Remove(ordredujour);
            _db.SaveChanges();
            this.AddToastMessage("Suppression d'un ordre du jour", "La suppression a été effectuée",
                Toast.ToastType.Success);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "RCP,RCD")]
        public ActionResult ModifierModeleOrdreDuJour()
        {
            var programme = GetProgramme();
            int NumProg = Convert.ToInt32(programme.First().discipline);
            //Va chercher la liste des ordre du jour en fonction du programme
            List<OrdreDuJour> listeOrdreDuJour = GetOrdreDuJourSelonModele(NumProg);

            if (listeOrdreDuJour != null)
            {
                ModificationModeleViewModel ModeleViewModel = new ModificationModeleViewModel();
                List<string> listeString = new List<string>();
                foreach (var item in listeOrdreDuJour)
                {
                    if (ModeleViewModel.listPP == null)
                    {
                        //Ajout d'un if
                        foreach (var spp in item.SujetPointPrincipal)
                        {
                            listeString.Add(spp.SujetPoint);
                        }
                        ModeleViewModel.listPP = listeString;
                    }
                }

                return View(ModeleViewModel);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifierModeleOrdreDuJour(ModificationModeleViewModel ModifModeleVM)
        {
            if (ModelState.IsValid)
            {
                string role = "";
                if (User.IsInRole("RCD"))
                {
                    role = "D";
                }
                else
                {
                    role = "P";
                }
                //Recherche du numéro
                var programme = GetProgramme();

                //Transfert le num programme (string) en int
                int NumProgramme = Convert.ToInt32(programme.First().discipline);

                ModeleOrdreDuJour Modele = new ModeleOrdreDuJour//Creation du modele
                {
                    Role = role,
                    NumeroProgramme = NumProgramme,
                    PointPrincipal = "Default"
                };
                _db.ModeleOrdreDuJour.Add(Modele);
                _db.SaveChanges();

                OrdreDuJour odj = new OrdreDuJour//Creation de 'ordre du jour
                {
                    TitreOdJ = "Modele",
                    HeureDebutReunion = "15h00",
                    HeureFinReunion = "16h00",
                    DateOdJ = Convert.ToDateTime("3000-12-25"),
                    IdModeleOrdreDuJour = Modele.IdModele
                };
                _db.OrdreDuJour.Add(odj);

                foreach (var item in ModifModeleVM.listPP)//Creation et liaison des PP
                {
                    var PointPrincipal = new SujetPointPrincipal
                    {
                        SujetPoint = item,
                        OrdreDuJour = odj,
                    };
                    _db.SujetPointPrincipal.Add(PointPrincipal);
                }
                _db.SaveChanges();
                this.AddToastMessage("Modèle enregistré", "Le modèle a bien été enregistré.",
               Toast.ToastType.Success);
                return RedirectToAction("Index");
            }
            return View(ModifModeleVM);
        }

        public ActionResult Info(int? id, int year)
        {
            ViewBag.AnneeODJ = year;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<OrdreDuJour> ordre = GetDateOrdreDuJour(year).ToList();
            if (ordre == null)
            {
                return HttpNotFound();
            }

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

        private bool chkDate(OrdreDuJourViewModel ordreDuJourViewModelCreerOdj)
        {
            bool validDate = true;
            DateTime date, dateCourante;

            date = ordreDuJourViewModelCreerOdj.ordreDuJour.DateOdJ;
            dateCourante = DateTime.Today;

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

        private IQueryable<OrdreDuJour> GetOrdreDuJour() //permet de generer les odjs dans la colonne de gauche (list)
        {
            return from odj in _db.OrdreDuJour
                   select odj;
        }

        private IQueryable<OrdreDuJour> GetDateOrdreDuJour(int annee) //permet daller chercher la date dun odj
        {
            return from odj in _db.OrdreDuJour
                   where odj.DateOdJ.Year == annee
                   select odj;
        }

        private IQueryable<OrdreDuJour> GetDixOrdreDuJour() //permet daller chercher les 10 derniers odj
        {
            return (from odj in _db.OrdreDuJour
                    orderby odj.IdOdJ descending
                    select odj).Take(10);
        }

        private List<SujetPointPrincipal> GetPointPrincipal(int id)
        {
            List<SujetPointPrincipal> listeSujetPointPrincipal = new List<SujetPointPrincipal>();
            var listeSujetPointPrincipalQuery = from SujetPointPrincipal in _db.SujetPointPrincipal
                                      where SujetPointPrincipal.IdPointPrincipal == id
                                      select SujetPointPrincipal;
            if (listeSujetPointPrincipalQuery.Count() != 0)
            {
                foreach (var item in listeSujetPointPrincipalQuery)
                {
                    listeSujetPointPrincipal.Add(item);
                }
            }
            return listeSujetPointPrincipal;
        }

        private List<SousPointSujet> GetSousPoint(int id)
        {
            List<SousPointSujet> listeSousPoint = new List<SousPointSujet>();
            var listeSousPointQuery = from SousPointSujet in _db.SousPointSujet
                                    where SousPointSujet.IdSujetPointPrincipal == id
                                    select SousPointSujet;
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
                                                   where accesProgramme.userMail == username
                                                   select accesProgramme;
            return programme;
        }

        private List<OrdreDuJour> GetOrdreDuJourSelonModele(int NumProg)
        {
            //string programme = Convert.ToString(NumProg);
            IQueryable<ModeleOrdreDuJour> listeModele = from ModeleOrdreDuJour in _db.ModeleOrdreDuJour
                                                        where ModeleOrdreDuJour.NumeroProgramme == NumProg
                                                        orderby ModeleOrdreDuJour.IdModele descending
                                                        select ModeleOrdreDuJour;
            int numID = listeModele.First().IdModele;
            IQueryable<OrdreDuJour> listeOrdreDuJourQuery = from OrdreDuJour in _db.OrdreDuJour
                                                       where OrdreDuJour.IdModeleOrdreDuJour == numID
                                                       select OrdreDuJour;
            List<OrdreDuJour> listeOrdreDuJour = new List<OrdreDuJour>();
            if(listeOrdreDuJourQuery != null)
            {
                foreach(var item in listeOrdreDuJourQuery)
                {
                    listeOrdreDuJour.Add(item);
                }
            }            
            return listeOrdreDuJour;
        }

        private bool regexHeure(OrdreDuJour odj) //Permet de valider le format de l'heure entré par l'uti
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
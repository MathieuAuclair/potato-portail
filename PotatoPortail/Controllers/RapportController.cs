using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Dynamic;
using System.Net;
using System.Data.Entity;
using ApplicationPlanCadre.Models;
using Rotativa;
using Rotativa.Options;


namespace ApplicationPlanCadre.Controllers
{
    public class RapportController : Controller
    {
        private BdPortail db = new BdPortail();

        // GET: Rapport
        public ActionResult Index()
        {
            dynamic model = new ExpandoObject();

            model.Programme = db.Programme.ToList();
            model.PlanCadre = db.PlanCadre.ToList();

            return View(model);
        }

        public ActionResult RapportPlanCadre(int id)
        {
            List<PlanCadre> planListe = new List<PlanCadre>();

            var planCadre = from a in db.PlanCadre
                join b in db.PlanCadrePrealable on a.idPlanCadre equals b.idPlanCadre
                where b.idPrealable == id
                select a;

            foreach (PlanCadre plan in planCadre)
            {
                planListe.Add(plan);
            }

            ViewData["listPcPrealableA"] = planListe;
            string footer = Server.MapPath("~/Views/static/footer.html");

            string reglage = string.Format(
                "--header-html  \"{0}\" " +
                "--header-spacing \"3\" " +
                "--footer-html \"{1}\" " +
                "--footer-spacing \"10\" " +
                "--footer-font-size \"10\" "
                , null, footer);

            return new ViewAsPdf("RapportPlanCadre", db.PlanCadre.Find(id))
            {
                CustomSwitches = reglage,
                PageSize = Size.A4,
                PageOrientation = Orientation.Landscape
            };
        }

        public ActionResult RapportProgramme(int id)
        {
            Programme prog = db.Programme.Find(id);
            //string header = Server.MapPath("~/Views/static/header.html");
            string footer = Server.MapPath("~/Views/static/footer.html");
            string reglage = string.Format(
                //"--header-html  \"{0}\" " +
                //"--header-spacing \"0\" " +
                "--footer-html \"{1}\" " +
                "--footer-spacing \"0\" " +
                "--footer-font-size \"10\" "
                //+ "--header-font-size \"10\" "
                , null, footer);

            return new ViewAsPdf("RapportProgramme", db.Programme.Find(id))
            {
                CustomSwitches = reglage,
                PageSize = Size.A4,
            };
        }
    }
}
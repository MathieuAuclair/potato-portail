using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Security;
using PotatoPortail.Models;

namespace PotatoPortail.Controllers
{
    public class AccueilController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.prenom = HttpContext.GetOwinContext()
                .GetUserManager<ApplicationUserManager>()
                .FindById(User.Identity.GetUserId()).prenom;
            getUtilisateur();
            return View();
        }


        public ActionResult Pedagogie()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ConsoleAdmin()
        {
            return View();
        }

        [Authorize]
        public ActionResult ESport()
        {
            return View();
        }

        public ActionResult Reunions()
        {
            return View();
        }

        [Authorize]

        public ActionResult OrdreDuJour()
        {
            return View();
        }

        public ActionResult Stage()
        {
            return View();
        }

        public void getUtilisateur()
        {
            var utilisateur = from tableUtilisateur in _db.Users
                where tableUtilisateur.Email == User.Identity.Name
                select tableUtilisateur;

            Session["PrenomUtilisateur"] = utilisateur.First().prenom;
            Session["NomUtilisateur"] = utilisateur.First().nom;
            Session["RolesUtilisateur"] = getRolesUser();

        }

        private string getRolesUser()
        {
            var roles = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);

            var ligne = "";
            foreach (var role in roles)
            {
                ligne += role + ", ";
            }
            if (ligne != "")
                ligne = ligne.Remove(ligne.Length - 2, 2);
            return ligne;
        }
    }
}
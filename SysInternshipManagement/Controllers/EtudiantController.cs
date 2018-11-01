using System.Linq;
using System.Net;
using System.Web.Mvc;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Controllers
{
    public class EtudiantController : Controller
    {
        private readonly DatabaseContext _bd = new DatabaseContext();

        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/Etudiant/Index.cshtml", _bd.etudiant.ToList());
        }

        [HttpPost]
        public ActionResult Edition(int? idEtudiant)
        {
            if (idEtudiant == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var etudiant = _bd.etudiant.Find(idEtudiant);

            if (etudiant == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View("~/Views/Etudiant/Edition.cshtml", etudiant);
        }

        [HttpPost]
        public ActionResult EnregistrerLesModifications(
            int? idEtudiant,
            string telephone,
            string prenom,
            string role,
            string codePermanent,
            string courrielEcole,
            string courrielPersonnel,
            string numeroDa,
            string nomDeFamille
            )
        {
            if (idEtudiant == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var etudiant = _bd.etudiant.Find(idEtudiant);

            if (etudiant == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            etudiant.Telephone = telephone;
            etudiant.Prenom = prenom;
            etudiant.Role = role;
            etudiant.CodePermanent = codePermanent;
            etudiant.CourrielEcole = courrielEcole;
            etudiant.CourrielPersonnel = courrielPersonnel;
            etudiant.NumeroDa = numeroDa;
            etudiant.NomDeFamille = nomDeFamille;

            _bd.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Creation()
        {
            var etudiant = new Etudiant
            {
                Telephone = "123-456-7890",
                Preference = null,
                Prenom = "prénom",
                NomDeFamille = "nom de famille",
                Role = "etudiant",
                CodePermanent = "ABCD12345678",
                CourrielEcole = "email@cegepjonquiere.ca",
                CourrielPersonnel = "email@cegepjonquiere.ca",
                NumeroDa = "1234567",
            };

            _bd.etudiant.Add(etudiant);
            _bd.SaveChanges();

            return View("~/Views/Etudiant/Edition.cshtml", etudiant);
        }
    }
}
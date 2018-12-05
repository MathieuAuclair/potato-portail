using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ApplicationPlanCadre.Models;
using PotatoPortail.Helpers;
using PotatoPortail.Migrations;

namespace PotatoPortail.Controllers
{
    [RCPContexteRealisationAuthorize]
    public class ContexteRealisationController : Controller
    {
        private readonly BDPortail _db = new BDPortail();

        public ActionResult _PartialList(int? idCompetence)
        {
            EnonceCompetence enonceCompetence = _db.EnonceCompetence.Find(idCompetence);

            if (enonceCompetence == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return PartialView(enonceCompetence.ContexteRealisation.OrderBy(e => e.numero));
        }

        [RCPEnonceCompetenceAuthorize]
        public ActionResult Creation(int? idCompetence)
        {
            if (idCompetence == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EnonceCompetence enonceCompetence = _db.EnonceCompetence.Find(idCompetence);
            if (enonceCompetence == null)
            {
                return HttpNotFound();
            }

            ContexteRealisation contexteRealisation = new ContexteRealisation
            {
                EnonceCompetence = enonceCompetence, 
                idCompetence = enonceCompetence.idCompetence
            };
            return View(contexteRealisation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RCPEnonceCompetenceAuthorize]
        public ActionResult Creation([Bind(Include = "idContexte,description,commentaire,idCompetence")]
            ContexteRealisation contexteRealisation)
        {
            Trim(contexteRealisation);
            AssignerNo(contexteRealisation);
            if (ModelState.IsValid)
            {
                this.AddToastMessage("Confirmation de la création",
                    "Le contexte de réalisation " + '\u0022' + contexteRealisation.description + '\u0022' +
                    " a bien été créé.", Toast.ToastType.Success);
                _db.ContexteRealisation.Add(contexteRealisation);
                _db.SaveChanges();
                return RedirectToAction("Creation", new {idCompetence = contexteRealisation.idCompetence});
            }
            else
            {
                this.AddToastMessage("Confirmation de la création",
                    "Le contexte de réalisation " + '\u0022' + contexteRealisation.description + '\u0022' +
                    " n'a pas bien été créé.", Toast.ToastType.Error);
            }

            contexteRealisation.EnonceCompetence = _db.EnonceCompetence.Find(contexteRealisation.idCompetence);
            return View(contexteRealisation);
        }

        public ActionResult Modifier(int? idContexte)
        {
            if (idContexte == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ContexteRealisation contexteRealisation = _db.ContexteRealisation.Find(idContexte);
            if (contexteRealisation == null)
            {
                return HttpNotFound();
            }

            return View(contexteRealisation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "idContexte,numero,description,commentaire,idCompetence")]
            ContexteRealisation contexteRealisation)
        {
            Trim(contexteRealisation);
            if (ModelState.IsValid)
            {
                _db.Entry(contexteRealisation).State = EntityState.Modified;
                _db.SaveChanges();
                this.AddToastMessage("Confirmation de la modification",
                    "Le contexte de réalisation " + '\u0022' + contexteRealisation.description + '\u0022' +
                    " a bien été modifié.", Toast.ToastType.Success);
                return RedirectToAction("Creation", new {idCompetence = contexteRealisation.idCompetence});
            }

            this.AddToastMessage("Confirmation de la modification",
                "Le contexte de réalisation " + '\u0022' + contexteRealisation.description + '\u0022' +
                " n'a pas été modifié.", Toast.ToastType.Error);

            return View(contexteRealisation);
        }

        [ActionName("Supression")]
        public ActionResult SurpressionConfirmer(int idContexte)
        {
            ContexteRealisation contexteRealisation = _db.ContexteRealisation.Find(idContexte);
            if (contexteRealisation == null)
            {
                this.AddToastMessage("Confirmation de la supression",
                    "Le contexte de réalisation n'a pas été supprimé.", Toast.ToastType.Error);
            }
            else
            {
                _db.ContexteRealisation.Remove(contexteRealisation);
                AjusterNo(contexteRealisation);
                _db.SaveChanges();
                this.AddToastMessage("Confirmation de la supression",
                    "Le contexte de réalisation " + '\u0022' + contexteRealisation.description + '\u0022' +
                    " a bien été supprimé.", Toast.ToastType.Success);
            }

            if (contexteRealisation == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return RedirectToAction("Creation", new {idCompetence = contexteRealisation.idCompetence});
        }

        private void AssignerNo(ContexteRealisation contexteRealisation)
        {
            int dernierNo = 0;
            IQueryable<int> requete = (from cp in _db.ContexteRealisation
                where cp.idCompetence == contexteRealisation.idCompetence
                select cp.numero);

            if (requete.Any())
            {
                dernierNo = requete.Max();
            }

            contexteRealisation.numero = dernierNo + 1;
        }

        private void AjusterNo(ContexteRealisation contexteRealisation)
        {
            IQueryable<ContexteRealisation> requete = (from cp in _db.ContexteRealisation
                where cp.idCompetence == contexteRealisation.idCompetence && cp.numero > contexteRealisation.numero
                select cp);
            foreach (ContexteRealisation cp in requete)
            {
                cp.numero--;
            }
        }

        private void Trim(ContexteRealisation contexteRealisation)
        {
            if (contexteRealisation.description != null)
                contexteRealisation.description = contexteRealisation.description.Trim();
        }

        protected override void Dispose(bool disposer)
        {
            if (disposer)
            {
                _db.Dispose();
            }

            base.Dispose(disposer);
        }
    }
}
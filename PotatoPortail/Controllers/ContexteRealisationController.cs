using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ApplicationPlanCadre.Controllers;
using PotatoPortail.Helpers;
using PotatoPortail.Models;
using PotatoPortail.Toast;

namespace PotatoPortail.Controllers
{
    [RCPContexteRealisationAuthorize]
    public class ContexteRealisationController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

        public ActionResult _PartialList(int? idCompetence)
        {
            EnonceCompetence enonceCompetence = _db.EnonceCompetence.Find(idCompetence);

            if (enonceCompetence == null)
            {
                return HttpNotFound();
            }

            return PartialView(enonceCompetence.ContexteRealisation.OrderBy(e => e.Numero));
        }

        [RCPEnonceCompetenceAuthorize]
        public ActionResult Creation(int? idCompetence)
        {
            if (idCompetence == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var enonceCompetence = _db.EnonceCompetence.Find(idCompetence);

            if (enonceCompetence == null)
            {
                return HttpNotFound();
            }

            var contexteRealisation = new ContexteRealisation
            {
                EnonceCompetence = enonceCompetence, IdCompetence = enonceCompetence.IdCompetence
            };

            return View(contexteRealisation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RCPEnonceCompetenceAuthorize]
        public ActionResult Creation([Bind(Include = "idContexte,description,commentaire,idCompetence")] ContexteRealisation contexteRealisation)
        {
            Trim(contexteRealisation);
            AssignerNo(contexteRealisation);
            if (ModelState.IsValid)
            {
                this.AddToastMessage("Confirmation de la création", "Le contexte de réalisation " + '\u0022' + contexteRealisation.Description + '\u0022' + " a bien été créé.", ToastType.Success);
                _db.ContexteRealisation.Add(contexteRealisation);
                _db.SaveChanges();
                return RedirectToAction("Creation", new {contexteRealisation.IdCompetence});
            }
            else
            {
                this.AddToastMessage("Confirmation de la création", "Le contexte de réalisation " + '\u0022' + contexteRealisation.Description + '\u0022' + " n'a pas bien été créé.", ToastType.Error);
            }
            contexteRealisation.EnonceCompetence = _db.EnonceCompetence.Find(contexteRealisation.IdCompetence);

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
        public ActionResult Modifier([Bind(Include = "idContexte,numero,description,commentaire,idCompetence")] ContexteRealisation contexteRealisation)
        {
            Trim(contexteRealisation);
            if (ModelState.IsValid)
            {
                _db.Entry(contexteRealisation).State = EntityState.Modified;
                _db.SaveChanges();
                this.AddToastMessage("Confirmation de la modification",
                    "Le contexte de réalisation " + '\u0022' + contexteRealisation.Description + '\u0022' +
                    " a bien été modifié.", ToastType.Success);
                return RedirectToAction("Creation", new {contexteRealisation.IdCompetence});
            }
            else
            {
                this.AddToastMessage("Confirmation de la modification", "Le contexte de réalisation " + '\u0022' + contexteRealisation.Description + '\u0022' + " n'a pas été modifié.", ToastType.Error);
            }

            return View(contexteRealisation);
        }

        [ActionName("Supression")]
        public ActionResult SurpressionConfirmer(int idContexte)
        {
            var contexteRealisation = _db.ContexteRealisation.Find(idContexte);
            if (contexteRealisation==null)
            {
                this.AddToastMessage("Confirmation de la supression", "Le contexte de réalisation n'a pas été supprimé.", ToastType.Error);
            }
            else
            {
                _db.ContexteRealisation.Remove(contexteRealisation);
                AjusterNo(contexteRealisation);
                _db.SaveChanges();
                this.AddToastMessage("Confirmation de la supression", "Le contexte de réalisation " + '\u0022' + contexteRealisation.Description + '\u0022' + " a bien été supprimé.", ToastType.Success);
            }

            if (contexteRealisation == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Creation", new {contexteRealisation.IdCompetence});
        }

        private void AssignerNo(ContexteRealisation contexteRealisation)
        {
            var dernierNo = 0;
            var requete = (from cp in _db.ContexteRealisation
                                     where cp.IdCompetence == contexteRealisation.IdCompetence
                                     select cp.Numero);

            if (requete.Any())
            {
                dernierNo = requete.Max();
            }
            contexteRealisation.Numero = dernierNo + 1;
        }

        private void AjusterNo(ContexteRealisation contexteRealisation)
        {
            var requete = (from tableContexteRealisation in _db.ContexteRealisation
                                                    where tableContexteRealisation.IdCompetence == contexteRealisation.IdCompetence && tableContexteRealisation.Numero > contexteRealisation.Numero
                                                    select tableContexteRealisation);

            foreach (var childPorn in requete) //la variable s'appelait cp, j'en ai déduis ce que j'ai pu
            {                                  //donnez des fucking noms significatif, un bon nom aurait été: contexteDeRealisation
                childPorn.Numero--;
            }
        }

        private static void Trim(ContexteRealisation contexteRealisation)
        {
            if (contexteRealisation.Description != null)
                contexteRealisation.Description = contexteRealisation.Description.Trim();
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
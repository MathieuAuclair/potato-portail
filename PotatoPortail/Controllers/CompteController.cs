using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Web.Mvc;
using ApplicationPlanCadre.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ApplicationPlanCadre.Helpers;
using ApplicationPlanCadre.Toast;
using System.Web.Security;

namespace ApplicationPlanCadre.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompteController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        
        public CompteController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public CompteController()
        {
            // pour déconnexion
        }

        public ActionResult Index()
        {
            var utilisateurs = db.Users.ToList();
            foreach (var utilisateur in utilisateurs)
            {
                utilisateur.roles = UserManager.GetRoles(utilisateur.Id);
            }

            return View(utilisateurs);
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        [AllowAnonymous]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Connexion(string returnUrl)
        {
            string psw = new PasswordGenerator().GeneratePassword(10);
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Accueil");
            }

            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index", "Accueil");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Connexion(ConnexionViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var resultat = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                shouldLockout: false);
            switch (resultat)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    this.AddToastMessage("", "Tentative de connexion non valide.", ToastType.Error, true);
                    return View(model);
            }
        }

        public void EnregistrementModelDefault(EnregistrementViewModel model, IEnumerable<string> role,
            IEnumerable<string> discipline)
        {
            if (role != null)
                model.Roles = role;
            else
                model.Roles = new List<string>();

            if (discipline != null)
                model.Disciplines = discipline;
            else
                model.Disciplines = new List<string>();
        }

        public void ModifierModelDefault(ModifierUtilisateurViewModel model, IEnumerable<string> role,
            IEnumerable<string> discipline)
        {
            if (role != null)
                model.Roles = role;
            else
                model.Roles = new List<string>();

            if (discipline != null)
                model.Disciplines = discipline;
            else
                model.Disciplines = new List<string>();
        }

        public ActionResult Enregistrement()
        {
            ViewBag.role = ConstruireRoleSelectList();
            ViewBag.discipline = new ConsoleDevisMinistereController().ConstruireCodeDevisMinistereSelectList();
            EnregistrementViewModel model = new EnregistrementViewModel();
            EnregistrementModelDefault(model, null, null);
            return View(model);
        }

        private bool IsRCP(ICollection<string> role)
        {
            bool isRCP = false;
            if (role != null)
                foreach (string r in role)
                    if (r == "RCP")
                    {
                        isRCP = true;
                        break;
                    }

            return isRCP;
        }

        private void CreationRCPAccesProgramme(ApplicationUser utilisateur, ICollection<string> discipline)
        {
            BDPlanCadre bd = new BDPlanCadre();
            foreach (string code in discipline)
            {
                AccesProgramme accesProgramme = new AccesProgramme {userMail = utilisateur.UserName, discipline = code};
                bd.AccesProgramme.Add(accesProgramme);
            }

            bd.SaveChanges();
        }

        private void EnleverToutRCPAccesProgramme(ApplicationUser utilisateur)
        {
            BDPlanCadre bd = new BDPlanCadre();
            bd.AccesProgramme.RemoveRange(bd.AccesProgramme.Where(e => e.userMail == utilisateur.UserName));
            bd.SaveChanges();
        }

        private void ModifierRCPAccesProgramme(ApplicationUser utilisateur, ICollection<string> discipline)
        {
            EnleverToutRCPAccesProgramme(utilisateur);
            CreationRCPAccesProgramme(utilisateur, discipline);
        }

        private void ModifierRoles(ApplicationUser utilisateur, ICollection<string> role)
        {
            UserManager.RemoveFromRoles(utilisateur.Id, utilisateur.roles.ToArray());
            UserManager.AddToRoles(utilisateur.Id, role.ToArray());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Enregistrement(EnregistrementViewModel model, ICollection<string> role,
            ICollection<string> discipline)
        {
            bool rolePresent = role != null;
            bool isRCP = IsRCP(role);
            bool programmeRCP = isRCP && discipline != null || !isRCP;

            if (ModelState.IsValid && rolePresent && programmeRCP)
            {
                string password = new PasswordGenerator().GeneratePassword(10);
                ApplicationUser utilisateur = new ApplicationUser
                    {nom = model.Nom, prenom = model.Prenom, UserName = model.Email, Email = model.Email};
                bool courrielEnvoyer = new MailHelper().SendActivationMail(utilisateur, password);
                if (courrielEnvoyer)
                {
                    var resultat = UserManager.Create(utilisateur, password);
                    if (resultat.Succeeded)
                    {
                        UserManager.AddToRoles(utilisateur.Id, role.ToArray());
                        if (isRCP)
                            CreationRCPAccesProgramme(utilisateur, discipline);

                        this.AddToastMessage("Confirmation", "Le courriel a été envoyé avec succès", ToastType.Success);
                        return RedirectToAction("Index", "Compte");
                    }

                    AddErrors(resultat);
                }
                else
                    this.AddToastMessage("Problème d'enregistrement",
                        "Une erreur est survenue lors de l'envoi du courriel, veuillez réessayer plus tard.",
                        Toast.ToastType.Error, true);
            }

            if (!rolePresent)
                this.AddToastMessage("rolePresent", "L'utilisateur doit avoir au minimum un rôle.", ToastType.Error,
                    true);
            if (!programmeRCP)
                this.AddToastMessage("rolePresent", "Un RCP doit avoir au minimum un programme d'assigné.",
                    ToastType.Error, true);

            EnregistrementModelDefault(model, role, discipline);
            ViewBag.role = ConstruireRoleSelectList();
            ViewBag.discipline = new ConsoleDevisMinistereController().ConstruireCodeDevisMinistereSelectList();
            return View(model);
        }

        private IEnumerable<string> GetDisciplines(ApplicationUser utilisateur)
        {
            return (from accesProgramme in new BDPlanCadre().AccesProgramme
                where accesProgramme.userMail == utilisateur.UserName
                select accesProgramme.discipline).ToList();
        }

        public ActionResult Modifier(string utilisateurId)
        {
            if (utilisateurId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser utilisateur = UserManager.FindById(utilisateurId);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }

            ModifierUtilisateurViewModel model = new ModifierUtilisateurViewModel
            {
                UserId = utilisateur.Id, Prenom = utilisateur.prenom, Nom = utilisateur.nom, Email = utilisateur.Email,
                Roles = UserManager.GetRoles(utilisateur.Id), Disciplines = GetDisciplines(utilisateur)
            };
            ViewBag.role = ConstruireRoleSelectList();
            ViewBag.discipline = new ConsoleDevisMinistereController().ConstruireCodeDevisMinistereSelectList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier(ModifierUtilisateurViewModel model, ICollection<string> role,
            ICollection<string> discipline)
        {
            bool rolePresent = role != null;
            bool isRCP = IsRCP(role);
            bool programmeRCP = isRCP && discipline != null || !isRCP;

            if (ModelState.IsValid && rolePresent && programmeRCP)
            {
                string password = new PasswordGenerator().GeneratePassword(10);
                ApplicationUser utilisateur = UserManager.FindById(model.UserId);
                utilisateur.prenom = model.Prenom;
                utilisateur.nom = model.Nom;
                utilisateur.roles = UserManager.GetRoles(utilisateur.Id);
                var resultatMail = UserManager.SetEmail(model.UserId, model.Email);
                if (resultatMail.Succeeded)
                {
                    bool mailEnvoyer = new MailHelper().SendEditMail(utilisateur, password);
                    if (mailEnvoyer)
                    {
                        var resultatUpdate = UserManager.Update(utilisateur);
                        if (resultatUpdate.Succeeded)
                        {
                            ModifierRoles(utilisateur, role);
                            if (isRCP)
                                ModifierRCPAccesProgramme(utilisateur, discipline);
                            else
                                EnleverToutRCPAccesProgramme(utilisateur);
                            this.AddToastMessage(
                                "Modification du compte", "Le compte a été modifié avec succès", ToastType.Success);
                            return RedirectToAction("Index", "Compte");
                        }

                        AddErrors(resultatUpdate);
                    }
                    else
                        this.AddToastMessage("Problème d'email",
                            "Une erreur est survenue lors de l'envoi du courriel, veuillez réessayer plus tard.",
                            ToastType.Error, true);
                }

                AddErrors(resultatMail);
            }

            if (!rolePresent)
                this.AddToastMessage("rolePresent", "L'utilisateur doit avoir au minimum un rôle.", ToastType.Error,
                    true);
            if (!programmeRCP)
                this.AddToastMessage("rolePresent", "Un RCP doit avoir au minimum un programme d'assigné.",
                    ToastType.Error, true);

            ModifierModelDefault(model, role, discipline);
            ViewBag.role = ConstruireRoleSelectList();
            ViewBag.discipline = new ConsoleDevisMinistereController().ConstruireCodeDevisMinistereSelectList();
            return View(model);
        }

        private SelectList ConstruireRoleSelectList()
        {
            var liste = db.Roles.Select(e => e.Name).ToList();
            return new SelectList(liste, "role");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Deconnexion()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Connexion", "Compte");
        }

        protected override void Dispose(bool disposer)
        {
            if (disposer)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposer);
        }

        #region Applications auxiliaires

        // Utilisé(e) pour la protection XSRF lors de l'ajout de connexions externes
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Accueil");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties {RedirectUri = RedirectUri};
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }

                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }
}
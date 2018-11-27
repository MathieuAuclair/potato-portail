using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ApplicationPlanCadre.Helpers;
using ApplicationPlanCadre.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SysInternshipManagement.Helpers;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompteController : Controller
    {
        public CompteController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public CompteController()
        {
            //Création d'un controlleur sans paramètre override
        }

        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ActionResult Index()
        {
            var utilisateurs = _db.Users.ToList();
            foreach (var utilisateur in utilisateurs)
            {
                utilisateur.roles = UserManager.GetRoles(utilisateur.Id);
            }

            return View(utilisateurs);
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            set => _signInManager = value;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        [AllowAnonymous]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Connexion(string returnUrl)
        {
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
                    return PartialView("Lockout");
                default:
                    ModelState.AddModelError("", @"Tentative de connexion non valide.");
                    return View(model);
            }
        }

        public void EnregistrementModelDefault(EnregistrementViewModel model, IEnumerable<string> role,
            IEnumerable<string> codeProgramme)
        {
            model.Roles = role ?? new List<string>();

            model.CodeProgrammes = codeProgramme ?? new List<string>();
        }

        public void ModifierModelDefault(ModifierUtilisateurViewModel model, IEnumerable<string> role,
            IEnumerable<string> codeProgramme)
        {
            model.Roles = role ?? new List<string>();

            model.CodeProgrammes = codeProgramme ?? new List<string>();
        }

        private bool IsRCP(ICollection<string> role)
        {
            bool isRcp = false;

            if (role != null)
                foreach (string r in role)
                    if (r == "RCP")
                    {
                        isRcp = true;
                        break;
                    }

            return isRcp;
        }

        private void CreationRcpAccesProgramme(ApplicationUser utilisateur, ICollection<string> codeProgramme)
        {
            DatabaseContext bd = new DatabaseContext();
            foreach (string code in codeProgramme)
            {
                AccesProgramme accesProgramme = new AccesProgramme
                    {userMail = utilisateur.UserName, codeProgramme = code};
                bd.AccesProgramme.Add(accesProgramme);
            }

            bd.SaveChanges();
        }

        private void EnleverToutRCPAccesProgramme(ApplicationUser utilisateur)
        {
            DatabaseContext bd = new DatabaseContext();
            bd.AccesProgramme.RemoveRange(bd.AccesProgramme.Where(e => e.userMail == utilisateur.UserName));
            bd.SaveChanges();
        }

        private void ModifierRcpAccesProgramme(ApplicationUser utilisateur, ICollection<string> codeProgramme)
        {
            EnleverToutRCPAccesProgramme(utilisateur);
            CreationRcpAccesProgramme(utilisateur, codeProgramme);
        }

        private void ModifierRoles(ApplicationUser utilisateur, ICollection<string> role)
        {
            UserManager.RemoveFromRoles(utilisateur.Id, utilisateur.roles.ToArray());
            UserManager.AddToRoles(utilisateur.Id, role.ToArray());
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Enregistrement()
        {
            ViewBag.role = ConstruireRoleSelectList();
            ViewBag.codeProgramme = new ConsoleDevisMinistereController().ConstruireCodeDevisMinistereSelectList();
            EnregistrementViewModel model = new EnregistrementViewModel();
            EnregistrementModelDefault(model, null, null);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Enregistrement(EnregistrementViewModel model, ICollection<string> role,
            ICollection<string> codeProgramme)
        {
            bool isRolePresent = role != null;
            bool programmeRcp = IsRCP(role) && codeProgramme != null || !IsRCP(role);

            if (ModelState.IsValid && isRolePresent && programmeRcp)
            {
                string password = "gitgood12345";//new PasswordGenerator().GeneratePassword(10);
                ApplicationUser utilisateur = new ApplicationUser
                    {nom = model.Nom, prenom = model.Prenom, UserName = model.Email, Email = model.Email};
                bool courrielEnvoyer = new MailHelper().SendActivationMail(utilisateur, password);
                if (courrielEnvoyer)
                {
                    var resultat = UserManager.Create(utilisateur, password);
                    if (resultat.Succeeded)
                    {
                        UserManager.AddToRoles(utilisateur.Id, role.ToArray());
                        if (IsRCP(role))
                            CreationRcpAccesProgramme(utilisateur, codeProgramme);
                        return RedirectToAction("Index", "Compte");
                    }

                    AddErrors(resultat);
                }
                else
                    ModelState.AddModelError("netMail",
                        @"Une erreur est survenue lors de l'envoi du courriel, veuillez réessayer plus tard.");
            }

            if (!isRolePresent)
                ModelState.AddModelError("rolePresent", @"L'utilisateur doit avoir au minimum un rôle.");
            if (!programmeRcp)
                ModelState.AddModelError("rolePresent", @"Un RCP doit avoir au minimum un programme d'assigné.");

            EnregistrementModelDefault(model, role, codeProgramme);
            ViewBag.role = ConstruireRoleSelectList();
            ViewBag.codeProgramme = new ConsoleDevisMinistereController().ConstruireCodeDevisMinistereSelectList();
            return View(model);
        }

        private IEnumerable<string> GetCodeProgrammes(ApplicationUser utilisateur)
        {
            return (from accesProgramme in new DatabaseContext().AccesProgramme
                where accesProgramme.userMail == utilisateur.UserName
                select accesProgramme.codeProgramme).ToList();
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
                Roles = UserManager.GetRoles(utilisateur.Id), CodeProgrammes = GetCodeProgrammes(utilisateur)
            };
            ViewBag.role = ConstruireRoleSelectList();
            ViewBag.codeProgramme = new ConsoleDevisMinistereController().ConstruireCodeDevisMinistereSelectList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier(ModifierUtilisateurViewModel model, ICollection<string> role,
            ICollection<string> codeProgramme)
        {
            bool isRolePresent = role != null;
            bool isRcp = IsRCP(role);
            bool isRcpFromProgramme = isRcp && codeProgramme != null || !isRcp;

            if (!(ModelState.IsValid && isRolePresent && isRcpFromProgramme))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

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
                        if (isRcp)
                            ModifierRcpAccesProgramme(utilisateur, codeProgramme);
                        else
                            EnleverToutRCPAccesProgramme(utilisateur);
                        return RedirectToAction("Index", "Compte");
                    }

                    AddErrors(resultatUpdate);
                }
                else
                    ModelState.AddModelError("netMail",
                        @"Une erreur est survenue lors de l'envoi du courriel, veuillez réessayer plus tard.");
            }

            AddErrors(resultatMail);

            ModifierModelDefault(model, role, codeProgramme);
            ViewBag.role = ConstruireRoleSelectList();
            ViewBag.codeProgramme = new ConsoleDevisMinistereController().ConstruireCodeDevisMinistereSelectList();
            return View(model);
        }

        private SelectList ConstruireRoleSelectList()
        {
            var liste = _db.Roles.Select(e => e.Name).ToList();
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

        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

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
            public ChallengeResult(string provider, string redirectUri, string userId = null)
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
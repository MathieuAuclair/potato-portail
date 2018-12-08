using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PotatoPortail.Migrations;

namespace PotatoPortail.Helpers
{
    public abstract class RcpAuthorize : AuthorizeAttribute
    {
        protected readonly BdPortail Db = new BdPortail();
        protected abstract string IdName { get; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorized = base.AuthorizeCore(httpContext);
            if (!authorized)
            {
                return false;
            }

            var user = httpContext.User;
            if (user.IsInRole("RCP"))
            {
                return ModelVerification(httpContext);
            }

            return false;
        }

        protected virtual bool ModelVerification(HttpContextBase httpContext)
        {
            string id = httpContext.Request.Params.Get(IdName);
            if (id == null || id == "0")
                return true;
            return IsRcpOwner(httpContext.User.Identity.Name, Convert.ToInt32(id));
        }

        protected abstract bool IsRcpOwner(string username, int id);
    }

    public class RcpDevisMinistereAuthorize : RcpAuthorize
    {
        protected override string IdName => "idDevis";

        protected override bool IsRcpOwner(string username, int id)
        {
            var devisMinistere = Db.DevisMinistere.Find(id);
            return Db.AccesProgramme.Any(accesProgramme => accesProgramme.Discipline == devisMinistere.Discipline);
        }
    }

    public class RcpEnonceCompetenceAuthorize : RcpAuthorize
    {
        protected override string IdName => "idCompetence";

        protected override bool IsRcpOwner(string username, int id)
        {
            var enonceCompetence = Db.EnonceCompetence.Find(id);
            return Db.AccesProgramme.Any(e =>
                e.Discipline == enonceCompetence.DevisMinistere.Discipline);
        }
    }

    public class RcpContexteRealisationAuthorize : RcpAuthorize
    {
        protected override string IdName => "idContexte";

        protected override bool IsRcpOwner(string username, int id)
        {
            var contexteRealisation = Db.ContexteRealisation.Find(id);
            return Db.AccesProgramme.Any(e =>
                e.Discipline == contexteRealisation.EnonceCompetence.DevisMinistere.Discipline);
        }
    }

    public class RcpElementCompetenceAuthorize : RcpAuthorize
    {
        protected override string IdName => "idElement";

        protected override bool IsRcpOwner(string username, int id)
        {
            var elementCompetence = Db.ElementCompetence.Find(id);
            return Db.AccesProgramme.Any(e =>
                e.Discipline == elementCompetence.EnonceCompetence.DevisMinistere.Discipline);
        }
    }

    public class RcpCriterePerformanceAuthorize : RcpAuthorize
    {
        protected override string IdName => "idCritere";

        protected override bool IsRcpOwner(string username, int id)
        {
            var criterePerformance = Db.CriterePerformance.Find(id);
            return Db.AccesProgramme.Any(e => e.Discipline == criterePerformance.ElementCompetence.EnonceCompetence
                                                  .DevisMinistere.Discipline);
        }
    }

    public class RcpProgrammeAuthorize : RcpAuthorize
    {
        protected override string IdName => "idProgramme";

        protected override bool IsRcpOwner(string username, int id)
        {
            var programme = Db.Programme.Find(id);
            return Db.AccesProgramme.Any(e =>
                e.Discipline == programme.DevisMinistere.Discipline);
        }
    }

    public class RcpPlanCadreAuthorize : RcpAuthorize
    {
        protected override string IdName => "idPlanCadre";

        protected override bool IsRcpOwner(string username, int id)
        {
            var planCadre = Db.PlanCadre.Find(id);
            return Db.AccesProgramme.Any(e =>
                e.Discipline == planCadre.Programme.DevisMinistere.Discipline);
        }
    }
}
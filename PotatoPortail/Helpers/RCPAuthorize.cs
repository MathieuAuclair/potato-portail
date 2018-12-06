using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationPlanCadre.Models;
using PotatoPortail.Migrations;

namespace PotatoPortail.Helpers
{
    public abstract class RcpAuthorize : AuthorizeAttribute
    {
        protected readonly BDPortail Db = new BDPortail();
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

    public class RCPDevisMinistereAuthorize : RcpAuthorize
    {
        protected override string IdName => "idDevis";

        protected override bool IsRcpOwner(string username, int id)
        {
            DevisMinistere devisMinistere = Db.DevisMinistere.Find(id);
            return Db.AccesProgramme.Any(e => e.codeProgramme == devisMinistere.EnteteProgramme.codeProgramme);
        }
    }

    public class RCPEnonceCompetenceAuthorize : RcpAuthorize
    {
        protected override string IdName => "idCompetence";

        protected override bool IsRcpOwner(string username, int id)
        {
            EnonceCompetence enonceCompetence = Db.EnonceCompetence.Find(id);
            return Db.AccesProgramme.Any(e =>
                e.codeProgramme == enonceCompetence.DevisMinistere.EnteteProgramme.codeProgramme);
        }
    }

    public class RCPContexteRealisationAuthorize : RcpAuthorize
    {
        protected override string IdName => "idContexte";

        protected override bool IsRcpOwner(string username, int id)
        {
            ContexteRealisation contexteRealisation = Db.ContexteRealisation.Find(id);
            return Db.AccesProgramme.Any(e =>
                e.codeProgramme == contexteRealisation.EnonceCompetence.DevisMinistere.EnteteProgramme.codeProgramme);
        }
    }

    public class RCPElementCompetenceAuthorize : RcpAuthorize
    {
        protected override string IdName => "idElement";

        protected override bool IsRcpOwner(string username, int id)
        {
            ElementCompetence elementCompetence = Db.ElementCompetence.Find(id);
            return Db.AccesProgramme.Any(e =>
                e.codeProgramme == elementCompetence.EnonceCompetence.DevisMinistere.EnteteProgramme.codeProgramme);
        }
    }

    public class RCPCriterePerformanceAuthorize : RcpAuthorize
    {
        protected override string IdName => "idCritere";

        protected override bool IsRcpOwner(string username, int id)
        {
            CriterePerformance criterePerformance = Db.CriterePerformance.Find(id);
            return Db.AccesProgramme.Any(e => e.codeProgramme == criterePerformance.ElementCompetence.EnonceCompetence
                                                  .DevisMinistere.EnteteProgramme.codeProgramme);
        }
    }

    public class RCPProgrammeAuthorize : RcpAuthorize
    {
        protected override string IdName => "idProgramme";

        protected override bool IsRcpOwner(string username, int id)
        {
            Programme programme = Db.Programme.Find(id);
            return Db.AccesProgramme.Any(e =>
                e.codeProgramme == programme.DevisMinistere.EnteteProgramme.codeProgramme);
        }
    }

    public class RCPPlanCadreAuthorize : RcpAuthorize
    {
        protected override string IdName => "idPlanCadre";

        protected override bool IsRcpOwner(string username, int id)
        {
            PlanCadre planCadre = Db.PlanCadre.Find(id);
            return Db.AccesProgramme.Any(e =>
                e.codeProgramme == planCadre.Programme.DevisMinistere.EnteteProgramme.codeProgramme);
        }
    }
}
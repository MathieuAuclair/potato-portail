using System.Web.Mvc;
using System.Web.Routing;

namespace PotatoPortail.Helpers
{
    //La balise [FilterAuthorize] est utilisé lorsque plusieurs rôles doivent être intégré au [Authorize]

    public class FilterAuthorize: AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { controller = "Accueil", action = "Index" }));
            }
        }
    }
}

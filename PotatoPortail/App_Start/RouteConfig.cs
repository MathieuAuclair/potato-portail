using System.Web.Mvc;
using System.Web.Routing;

namespace PotatoPortail
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Accueil", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "PotatoPortail.Controllers" }
            );
        }
    }
}

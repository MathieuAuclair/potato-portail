using Microsoft.Owin;
using Owin;
using PotatoPortail;

[assembly: OwinStartup(typeof(Startup))]

namespace PotatoPortail
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
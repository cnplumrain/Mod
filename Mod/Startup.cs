using Microsoft.Owin;
using Mod.Web;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace Mod.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

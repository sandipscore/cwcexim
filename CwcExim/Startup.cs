using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CwcExim.Startup))]
namespace CwcExim
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

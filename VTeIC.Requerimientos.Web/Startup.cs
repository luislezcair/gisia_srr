using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VTeIC.Requerimientos.Web.Startup))]
namespace VTeIC.Requerimientos.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AwareswebApp.Startup))]
namespace AwareswebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

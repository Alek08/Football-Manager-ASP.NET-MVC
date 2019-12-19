using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Virtual_Football_Manager.Startup))]
namespace Virtual_Football_Manager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectAspNETv2.Startup))]
namespace ProjectAspNETv2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DiaMakerMvc.Startup))]
namespace DiaMakerMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

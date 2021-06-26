using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SistemaOfertas.Startup))]
namespace SistemaOfertas
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

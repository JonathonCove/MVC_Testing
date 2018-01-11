using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Scarecrow.Startup))]
namespace Scarecrow
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

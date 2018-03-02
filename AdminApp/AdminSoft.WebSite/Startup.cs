using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PLNFramework.Security.MVCUI.Startup))]
namespace PLNFramework.Security.MVCUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
using Microsoft.Owin;
using Owin;
using NJsonSchema;
using NSwag.AspNetCore;
using System.Reflection;

[assembly: OwinStartupAttribute(typeof(PLNFramework.Security.MVCUI.Startup))]
namespace PLNFramework.Security.MVCUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            //app.UseFacebookAuthentication(new Microsoft.Owin.Security.Facebook.FacebookAuthenticationOptions()
            //{
            //    AppId = "2049308795393284",
            //    AppSecret = "cc96ec17417942a50bc0d590041947bf",
            //    Scope = { "email" }
            //});
        }
    }
}
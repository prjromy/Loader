using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartupAttribute(typeof(Loader.Startup))]
namespace Loader
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
            //app.UseCookieAuthentication(
            //new CookieAuthenticationOptions
            //{

            //    // YOUR LOGIN PATH
            //    LoginPath = new PathString("/Account/Login")
            //}
     
        
    }
}

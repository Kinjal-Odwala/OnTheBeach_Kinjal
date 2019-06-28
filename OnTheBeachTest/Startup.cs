using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnTheBeachTest.Startup))]
namespace OnTheBeachTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           // ConfigureAuth(app);
        }
    }
}

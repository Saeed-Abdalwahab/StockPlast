using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StockPortal.Startup))]
namespace StockPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
 
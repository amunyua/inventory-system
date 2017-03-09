using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InventoryManagementSystemMVC.Startup))]
namespace InventoryManagementSystemMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

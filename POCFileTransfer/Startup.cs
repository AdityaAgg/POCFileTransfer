using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(POCFileTransfer.Startup))]
namespace POCFileTransfer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

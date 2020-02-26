using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DebitCreditMemo.Startup))]
namespace DebitCreditMemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

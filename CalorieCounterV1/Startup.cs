using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CalorieCounterV1.Startup))]
namespace CalorieCounterV1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

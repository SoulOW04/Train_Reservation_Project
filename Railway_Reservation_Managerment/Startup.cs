using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Railway_Reservation_Managerment.Startup))]
namespace Railway_Reservation_Managerment
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

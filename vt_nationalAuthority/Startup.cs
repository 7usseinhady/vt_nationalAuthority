using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;


[assembly: OwinStartup("start", typeof(vt_nationalAuthority.Startup))]
namespace vt_nationalAuthority
{
    public partial class Startup
    {
        /// <summary>
        /// Configuration Run When Application Start
        /// </summary>
        /// <param name="app">Interface For Application Builder</param>
        public void Configuration(IAppBuilder app)
        {
            var config = new HubConfiguration();
            var idProvider = new userProvider();
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);
            app.MapSignalR();
        }
        }
    }
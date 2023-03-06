using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

//[assembly: OwinStartup(typeof(ApiNationalAuthority.Startup))]
[assembly: OwinStartup("ApiLayer", typeof(ApiNationalAuthority.Startup))]

namespace ApiNationalAuthority
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

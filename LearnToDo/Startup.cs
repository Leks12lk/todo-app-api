using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(LearnToDo.Startup))]

namespace LearnToDo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			var config = new System.Web.Http.HttpConfiguration();
			ConfigureAuth(app, config);
        }
    }
}

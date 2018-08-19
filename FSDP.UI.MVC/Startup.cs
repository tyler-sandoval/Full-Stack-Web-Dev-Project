using Owin;
using System;
using Microsoft.Owin;
using System.IO;
using System.Web.Http;



namespace FSDP.UI.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            


        }

    }

}

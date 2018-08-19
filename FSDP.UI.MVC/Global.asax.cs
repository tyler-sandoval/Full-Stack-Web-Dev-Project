using FSDP.UI.MVC.Models;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using System;
using Gnostice.StarDocsSDK;

namespace FSDP.UI.MVC
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=301868
    public class MvcApplication : System.Web.HttpApplication
    {
        public static StarDocs starDocs;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //setup connection to Gnostice API for StarDocs
            starDocs = new StarDocs(new ConnectionInfo(new Uri("https://api.gnostice.com/stardocs/v1"), "b20bf47a1f544cd6992c9315ce5bc99c", "90d88b7a078740fcb0d3c8a5aa682957"));

            //Authenticate starDocs
            starDocs.Auth.loginApp();
        }
    }
}

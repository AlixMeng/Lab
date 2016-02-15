using System.Security.Policy;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using LabRequest.Web.Controllers;

namespace LabRequest.Web.Infrastracture
{
    public class RoutingSystem
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute("Lab", "{controller}/{action}/{page}/{sortorder}",
            //    new
            //    {
            //        controller = "TestRequest",
            //        action = "TestRequestList",
            //        page = UrlParameter.Optional,
            //        sortorder = UrlParameter.Optional
            //    });


            routes.MapRoute("Default", "{controller}/{action}/{id}",
                  new
                  {
                      controller = "Account",
                      action = "Login",
                      id = UrlParameter.Optional
                  });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectAspNETv2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Welcome", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "admin",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
          );

            routes.MapRoute(
               name: "vendeur",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Proprietaire", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
             name: "vendeur2",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Admin", action = "Bloc", id = UrlParameter.Optional }
         );
        }
    }
}

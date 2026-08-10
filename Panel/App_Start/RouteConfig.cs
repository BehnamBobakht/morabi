using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Panel
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes) 
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("PictureThumb1", "thumb/{picurl}", new { controller = "Download", action = "GetThumb" });
            routes.MapRoute("PictureThumb2", "thumb/{picurl}/o{orginal}", new { controller = "Download", action = "GetThumb" });
            routes.MapRoute("PictureThumb3", "thumb/{picurl}/w{width}", new { controller = "Download", action = "GetThumb" });
            routes.MapRoute("PictureThumb4", "thumb/{picurl}/h{height}", new { controller = "Download", action = "GetThumb" });
            routes.MapRoute("PictureThumb5", "thumb/{picurl}/q{quality}", new { controller = "Download", action = "GetThumb" });
            routes.MapRoute("PictureThumb6", "thumb/{picurl}/w{width}/h{height}", new { controller = "Download", action = "GetThumb" });
            routes.MapRoute("PictureThumb7", "thumb/{picurl}/w{width}/q{quality}", new { controller = "Download", action = "GetThumb" });
            routes.MapRoute("PictureThumb8", "thumb/{picurl}/h{height}/q{quality}", new { controller = "Download", action = "GetThumb" });
            routes.MapRoute("PictureThumb9", "thumb/{picurl}/w{width}/h{height}/q{quality}", new { controller = "Download", action = "GetThumb" });

            routes.MapRoute(
                name: "Error",
                url: "error/{id}",
                defaults: new { controller = "Error", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

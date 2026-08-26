using System.Web.Mvc;
using System.Web.Routing;

namespace Media
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("LearningDocumentRead", "learning/documents/read/{id}", new { controller = "Documents", action = "Read", id = UrlParameter.Optional });
            routes.MapRoute("LearningDocuments", "learning/documents/{id}", new { controller = "Documents", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute("Learning", "learning", new { controller = "Learning", action = "Index" });

            routes.MapRoute(
                name: "CourseWatch",
                url: "courses/watch/{id}",
                defaults: new { controller = "Courses", action = "Watch", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Courses",
                url: "courses",
                defaults: new { controller = "Courses", action = "Index" }
            );

            routes.MapRoute(
                name: "MyVideosWatch",
                url: "my-videos/watch/{id}",
                defaults: new { controller = "MyVideos", action = "Watch", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "MyVideos",
                url: "my-videos",
                defaults: new { controller = "MyVideos", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Courses", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

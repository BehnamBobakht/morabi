using System.Web.Mvc;

namespace Panel.Areas.Calendar
{
    public class CalendarAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Calendar";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Calendar_Events",
                url: "calendar/event/{id}",
                defaults: new { controller = "event", action = "index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Calendar_default",
                "calendar/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
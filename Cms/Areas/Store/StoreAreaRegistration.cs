using System.Web.Mvc;

namespace Cms.Areas.Store
{
    public class StoreAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "store";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {

            //context.MapRoute(
            //    "Store_default",
            //    "Store/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);


            context.MapRoute(
                "Store_default",
                "store/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Cms.Areas.Store.Controllers" }
            );

        }
    }
}
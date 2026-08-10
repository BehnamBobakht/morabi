using System.Web.Mvc;

namespace Panel.Areas.Store
{
    public class StoreAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Store";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Store_ShopUser",
                url: "store/shop/shopuser/{id}",
                defaults: new { controller = "shopuser", action = "index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Store_default",
                "store/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
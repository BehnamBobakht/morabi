using System.Web.Mvc;

namespace Panel.Areas.Setting
{
    public class SettingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Setting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Setting_WebsiteDomain",
                url: "setting/website/domain/{id}",
                defaults: new { controller = "domain", action = "index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Setting_default",
                "setting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
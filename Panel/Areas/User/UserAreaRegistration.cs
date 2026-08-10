using System.Web.Mvc;

namespace Panel.Areas.User
{
    public class UserAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "user";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "UserGroup_Users",
                url: "user/usergroup/users/{id}",
                defaults: new { controller = "usergroup", action = "Users", id = UrlParameter.Optional }
            );

            context.MapRoute(
                name: "UserGroup_Permission",
                url: "user/usergroup/permission/{id}",
                defaults: new { controller = "usergroup", action = "Permission", id = UrlParameter.Optional }
            );

            context.MapRoute(
                name: "UserGroup_SiteUsers",
                url: "user/siteuser/groups/{id}",
                defaults: new { controller = "siteuser", action = "Groups", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "User_default",
                "user/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DataLayer.Enumarables;
using System.Xml;
using System.IO;

namespace Cms
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Product_landig_page", "landing/{prcustomLabel}", new { controller = "Product", action = "landing", proutput = "landing" });

            routes.MapRoute("Product_search_page", "search/{prcustomLabel}", new { controller = "Product", action = "Search", proutput = "search" });
            routes.MapRoute("Product_search_page_2", "search", new { controller = "Product", action = "Search", proutput = "search" });

            routes.MapRoute("SeoRobots", "robots.txt", new { controller = "Seo", action = "Robots" });
            routes.MapRoute("SeoSitemapInfo", "sitemap-{sitemap}-{pageindex}.xml", new { controller = "Seo", action = "SitemapInfo" });
            routes.MapRoute("SeoSitemapListXml", "sitemap.xml", new { controller = "Seo", action = "SitemapList" });
            routes.MapRoute("SeoSitemapList", "sitemap", new { controller = "Seo", action = "SitemapList" });
            routes.MapRoute("Product_Page", "pr/{prid}/{pridname}", new { controller = "Product", action = "Index" });
            routes.MapRoute("ProductBrand_Page", "pb/{pbid}/{pbidname}", new { controller = "ProductBrand", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("ProductBrand_Search_Page", "brands", new { controller = "ProductBrand", action = "Search" });
            routes.MapRoute("Logout", "logout", new { controller = "Account", action = "Logout" }, new[] { "Cms.Controllers" });
            routes.MapRoute("Login", "login", new { controller = "Account", action = "Login" }, new[] { "Cms.Controllers" });
            routes.MapRoute("SignIn", "signin", new { controller = "Account", action = "SignIn" }, new[] { "Cms.Controllers" });
            routes.MapRoute("SignUp", "signup", new { controller = "Account", action = "SignUp" });
            routes.MapRoute("Verify", "verify", new { controller = "Account", action = "Verify" });

            routes.MapRoute("Services", "Services", new { controller = "Product", action = "Services" }, new[] { "Cms.Controllers" });
            routes.MapRoute("category", "category", new { controller = "ProductCategory", action = "Search" }, new[] { "Cms.Controllers" });
            routes.MapRoute("ProductCategory", "ProductCategory/Index/{id}", new { controller = "ProductCategory", action = "Index", id = UrlParameter.Optional }, new[] { "Cms.Controllers" });
            routes.MapRoute("Productcategorycategory", "Productcategory/{pctypeId}/{name}", new { controller = "ProductCategory", action = "Search", pctypeId = UrlParameter.Optional }, new[] { "Cms.Controllers" });
            routes.MapRoute("ProductSubCategory", "productsubcategory/{pscategoryId}/{name}", new { controller = "ProductSubCategory", action = "Search", pscategoryId = UrlParameter.Optional }, new[] { "Cms.Controllers" });
            routes.MapRoute("Register", "register", new { controller = "Account", action = "Register" });
            routes.MapRoute("Forget", "forget", new { controller = "Account", action = "Forget" }, new[] { "Cms.Controllers" });
            routes.MapRoute("Recover", "recover", new { controller = "Account", action = "Recover" }, new[] { "Cms.Controllers" });
            routes.MapRoute("MessageShop", "profile/messageshop/{id}", new { controller = "Account", action = "MessageShop", apiId = UrlParameter.Optional });
            routes.MapRoute("Message", "profile/message", new { controller = "Account", action = "Message" });
            routes.MapRoute("Follow", "profile/follow", new { controller = "Account", action = "Follow" });
            routes.MapRoute("Favorite", "favorite", new { controller = "Account", action = "Favorite" }, new[] { "Cms.Controllers" });
            routes.MapRoute("Profile", "profile", new { controller = "Account", action = "Profile" }, new[] { "Cms.Controllers" });
            routes.MapRoute("History", "History", new { controller = "Account", action = "History" }, new[] { "Cms.Controllers" });
            routes.MapRoute("Tracking", "tracking", new { controller = "Account", action = "Tracking" });
            routes.MapRoute("Post_Page", "page/{pocategory}", new { controller = "Post", action = "Page" });
            routes.MapRoute("Post_PageApi", "content/{pocategory}", new { controller = "Post", action = "content" });

            routes.MapRoute("Post_IndexApi", "post/{poid}/{poidname}", new { controller = "Post", action = "Post", apiId = UrlParameter.Optional });

            //routes.MapRoute("Post_Category", "po/{poidlabel}", new { controller = "Post", action = "SearchCategory" });
            routes.MapRoute("Post_CategoryName", "poCategory/{poidlabel}/{pocategoryId}", new { controller = "Post", action = "Search", pocategoryId = UrlParameter.Optional }, new[] { "Cms.Controllers" });
            routes.MapRoute("Post_Index", DataLayer.Base.BaseWebsite.PostUrl+"/{poid}/{poidname}", new { controller = "Post", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Banner_Index", "ba/{baid}/{baidname}", new { controller = "Banner", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Gallery_Index", "ga/{gaid}/{gaidname}", new { controller = "Gallery", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Error_Index", "error/{id}", new { controller = "ErrorPage", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("StaticPage_Index", "st/{id}", new { controller = "StaticPage", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Category_Index", "cat/{name}", new { controller = "Category", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Store_Index", "seller/{prstoreId}/{stname}", new { controller = "Store", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Brand_Index", "brand/{brbrandId}/{brname}", new { controller = "Brand", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Post_Landing", "land/{pocategory}", new { controller = "Post", action = "Landing" });
            routes.MapRoute("Post_SearchPage", "searchpage/{pocategory}", new { controller = "Post", action = "Search2" });
            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "Cms.Controllers" }
            );
            routes.LowercaseUrls = true;

        }
    }
}

using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace Cms
{
    public class MvcApplication : System.Web.HttpApplication
    {
        UnitOfWork _context = new UnitOfWork();



        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //ThreadPool.QueueUserWorkItem(o => PingServers());
            MapAll.Maping();


        }

        protected void Application_PostAuthorizeRequest()
        {
            HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        }

        protected void Application_AcquireRequestState()
        {

            IncreaseWebsiteView();
            RemoveWwwUrl();
            SetWebsiteLanguage();
            ToLowerUrl();
        }
        private void ToLowerUrl()
        {
            var currentUrl = Request.Url.ToString();
            var controllerobj = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"];
            if (controllerobj != null)
            {
                string controller = controllerobj.ToString().ToLower();

                if ((!string.IsNullOrEmpty(controller)) && (controller == "post" || controller == "product" || controller == "staticpage"))
                {
                    bool hasUppercase = !currentUrl.Contains(currentUrl.ToLower());
                    if (hasUppercase == true)
                    {
                        string url = Request.Url.ToString().ToLower();
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", url);
                    }
                }
            }
            if (currentUrl.Contains("https://banionline.ir"))
            {
                BaniOnline(currentUrl);
            }
            if (currentUrl.Contains("https://atashza.com"))
            {
                Atashza(currentUrl);
            }
            //Bisehna();

        }
        private void BaniOnline(string currentUrl)
        {
            switch (currentUrl)
            {
                case "https://banionline.ir/search/herbal":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/search/toothpaste");
                        break;
                    }
                case "https://banionline.ir/search/natura":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/search/toothpaste");
                        break;
                    }
                case "https://banionline.ir/pr/18/کرم-مرطوب-کننده-آرگان":
                    {
                        string newUrl = "https://banionline.ir/pr/66/%DA%A9%D8%B1%D9%85-%D8%AF%D8%B3%D8%AA-%D8%A7%D9%88%D8%B1%D9%87-%D8%A8%D8%A7%D9%84%D9%87-%D8%A2";
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", newUrl);

                        break;
                    }
                case "https://banionline.ir/pr/38/خمیر-دندان-آلوئه-ورا-بزرگ":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/pr/7/toothpaste-pack");
                        break;
                    }
                case "https://banionline.ir/pr/15/کرم-مرطوب-کننده-دست-و-صورت-عسل":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/pr/92/%D9%85%D8%B1%D8%B7%D9%88%D8%A8-%DA%A9%D9%86%D9%86%D8%AF%D9%87-%D8%B5%D9%88%D8%B1%D8%AA-%D9%88-%D8%A8%D8%AF%D9%86-%D8%A8%D8%A7%D9%84%D9%87-%D8%A2");
                        break;
                    }
                case "https://banionline.ir/panel/search/toothbrush":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/search/toothbrush");
                        break;
                    }
                case "https://banionline.ir/pr/27/پک-ولنتاین-بوفی":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/pr/47/%D9%BE%DA%A9-%D8%AE%D9%85%DB%8C%D8%B1-%D8%AF%D9%86%D8%AF%D8%A7%D9%86-%D8%B7%D8%A8%DB%8C%D8%B9%DB%8C-%D8%AE%D8%A7%D9%86%D9%88%D8%A7%D8%AF%D9%87-%D8%A2%D9%84%D9%81%D8%A7%D8%AF%D9%86%D8%AA");
                        break;
                    }
                case "https://banionline.ir/pr/20/رول-ضد-تعریق-لینفیلد-مدل-powerحجم-75-میلی-لیتر":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/pr/98/%D9%85%D8%A7%D9%85-%D8%B1%D9%88%D9%84-%D8%A2%D8%A8%DB%8C-%D8%A8%D8%A7%D9%84%D9%87-%D8%A2");
                        break;
                    }
                case "https://banionline.ir/pr/14/کرم-مرطوب-کننده-لینفیلد-مدل-لوندر-حجم-250-میلی-لیتر":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/pr/81/%DA%A9%D8%B1%D9%85-%D8%AF%D8%B3%D8%AA-%D8%B2%DB%8C%D8%AA%D9%88%D9%86-%D8%A8%D8%A7%D9%84%D9%87-%D8%A2");
                        break;
                    }
                case "https://banionline.ir/pr/12/کرم-مرطوب-کننده-لینفیلد-مدل-آلوئه-ورا-حجم-250-میلی-لیتر":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/pr/81/%DA%A9%D8%B1%D9%85-%D8%AF%D8%B3%D8%AA-%D8%B2%DB%8C%D8%AA%D9%88%D9%86-%D8%A8%D8%A7%D9%84%D9%87-%D8%A2");
                        break;
                    }
                case "https://banionline.ir/pr/24/مام-زنانه-لینفیلد-مدل-energy":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/pr/97/%D9%85%D8%A7%D9%85-%D8%B1%D9%88%D9%84-%D8%B5%D9%88%D8%B1%D8%AA%DB%8C-%D8%A8%D8%A7%D9%84%D9%87-%D8%A2");
                        break;
                    }
                case "https://banionline.ir/pr/17/کرم-آبرسان-دست-و-صورت-آووکادو":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/pr/75/%DA%A9%D8%B1%D9%85-%D8%AF%D8%B3%D8%AA-%D9%85%D8%AE%D8%B5%D9%88%D8%B5-%D9%BE%D9%88%D8%B3%D8%AA-%D8%AE%D8%B4%DA%A9-%D8%A8%D8%A7%D9%84%D9%87-%D8%A2");
                        break;
                    }
                case "https://banionline.ir/pr/17/کرم-مرطوب-کننده-لینفیلد-مدل-آووکادو-حجم-250-میلی-لیتر":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/pr/75/%DA%A9%D8%B1%D9%85-%D8%AF%D8%B3%D8%AA-%D9%85%D8%AE%D8%B5%D9%88%D8%B5-%D9%BE%D9%88%D8%B3%D8%AA-%D8%AE%D8%B4%DA%A9-%D8%A8%D8%A7%D9%84%D9%87-%D8%A2");
                        break;
                    }
                case "https://banionline.ir/pr/25/پک-خمیردندان-نعنا-و-آلوئه-ورا-و-نارگیل-50-گرمی-مخصوص-روز-پدر":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/pr/47/%D9%BE%DA%A9-%D8%AE%D9%85%DB%8C%D8%B1-%D8%AF%D9%86%D8%AF%D8%A7%D9%86-%D8%B7%D8%A8%DB%8C%D8%B9%DB%8C-%D8%AE%D8%A7%D9%86%D9%88%D8%A7%D8%AF%D9%87-%D8%A2%D9%84%D9%81%D8%A7%D8%AF%D9%86%D8%AA");
                        break;
                    }
                case "https://banionline.ir/pr/21/رول-ضد-تعریق-لینفیلد-مدل-fresh-حجم-75-میلی-لیتر":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/pr/98/%D9%85%D8%A7%D9%85-%D8%B1%D9%88%D9%84-%D8%A2%D8%A8%DB%8C-%D8%A8%D8%A7%D9%84%D9%87-%D8%A2");
                        break;
                    }
                case "https://banionline.ir/pr/16/کرم-آبرسان-دست-و-صورت-زعفران":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/pr/81/%DA%A9%D8%B1%D9%85-%D8%AF%D8%B3%D8%AA-%D8%B2%DB%8C%D8%AA%D9%88%D9%86-%D8%A8%D8%A7%D9%84%D9%87-%D8%A2");
                        break;
                    }
                case "https://banionline.ir/pr/19/رول-ضد-تعریق-لینفیلد-مدل-health-حجم-75-میلی-لیتر":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/pr/98/%D9%85%D8%A7%D9%85-%D8%B1%D9%88%D9%84-%D8%A2%D8%A8%DB%8C-%D8%A8%D8%A7%D9%84%D9%87-%D8%A2");
                        break;
                    }
                case "https://banionline.ir/pr/24/رول-ضد-تعریق-لینفیلد-مدل-energy-حجم-75-میلی-لیتر":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/pr/98/%D9%85%D8%A7%D9%85-%D8%B1%D9%88%D9%84-%D8%A2%D8%A8%DB%8C-%D8%A8%D8%A7%D9%84%D9%87-%D8%A2");
                        break;
                    }
                case "https://banionline.ir/pr/15/کرم-مرطوب-کننده-عسل":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/pr/66/%DA%A9%D8%B1%D9%85-%D8%AF%D8%B3%D8%AA-%D8%A7%D9%88%D8%B1%D9%87-%D8%A8%D8%A7%D9%84%D9%87-%D8%A2");
                        break;
                    }
                case "https://banionline.ir/pr/15/کرم-مرطوب-کننده-لینفیلد-مدل-عسل-حجم-250-میلی-لیتر":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/pr/66/%DA%A9%D8%B1%D9%85-%D8%AF%D8%B3%D8%AA-%D8%A7%D9%88%D8%B1%D9%87-%D8%A8%D8%A7%D9%84%D9%87-%D8%A2");
                        break;
                    }
                case "https://banionline.ir/brand/1/lindfeild":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/brand/7/balea");
                        break;
                    }
                case "https://banionline.ir/po/10/%D9%87%D9%85%D9%87-%DA%86%DB%8C%D8%B2-%D8%AF%D8%B1%D8%A8%D8%A7%D8%B1%D9%87-%D8%AE%D9%85%DB%8C%D8%B1-%D8%AF%D9%86%D8%AF%D8%A7%D9%86-%D8%B7%D8%A8%DB%8C%D8%B9%DB%8C-%D9%88-%DA%AF%DB%8C%D8%A7%D9%87%DB%8C":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/po/10/everything-about-natural-and-herbal-toothpaste");
                        break;
                    }
                case "https://banionline.ir/brand/4/alphadent":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/brand/3/alphadent");
                        break;
                    }
                case "https://banionline.ir/panel/pr/34/%D8%B3%D8%B1%DA%86%D9%88%D8%A8-%D9%85%D8%B3%D9%88%D8%A7%DA%A9-10-%D8%AA%D8%A7%DB%8C%DB%8C":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/pr/34/%D8%B3%D8%B1%DA%86%D9%88%D8%A8-%D9%85%D8%B3%D9%88%D8%A7%DA%A9-10-%D8%AA%D8%A7%DB%8C%DB%8C");
                        break;
                    }
                case "https://banionline.ir/panel/pr/36/%D8%AF%D8%B3%D8%AA%D9%87-%DA%86%D9%88%D8%A8-%D9%85%D8%B3%D9%88%D8%A7%DA%A9":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/pr/36/%D8%AF%D8%B3%D8%AA%D9%87-%DA%86%D9%88%D8%A8-%D9%85%D8%B3%D9%88%D8%A7%DA%A9");
                        break;
                    }
                case "https://banionline.ir/panel/pr/33/%D8%B3%D8%B1%DB%8C-%DA%86%D9%88%D8%A8-%D9%85%D8%B3%D9%88%D8%A7%DA%A9-3-%D8%AA%D8%A7%DB%8C%DB%8C":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/pr/33/%D8%B3%D8%B1%DB%8C-%DA%86%D9%88%D8%A8-%D9%85%D8%B3%D9%88%D8%A7%DA%A9-3-%D8%AA%D8%A7%DB%8C%DB%8C");
                        break;
                    }
                case "https://banionline.ir/search/skin-caring":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://banionline.ir/search/skin-beauty");
                        break;
                    }
                case "https://banionline.ir/Assets/KhoshKala/Image/download.png":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", Request.Url.ToString());
                        HttpContext.Current.Response.Redirect("https://banionline.ir/error/410");
                        break;
                    }
                case "https://banionline.ir/brand/2/lg":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", Request.Url.ToString());
                        HttpContext.Current.Response.Redirect("https://banionline.ir/error/410");
                        break;
                    }
                case "https://banionline.ir/pr/5/samsungultra":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", Request.Url.ToString());
                        HttpContext.Current.Response.Redirect("https://banionline.ir/error/410");
                        break;
                    }
                case "https://banionline.ir/pr/3/iphoneee":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", Request.Url.ToString());
                        HttpContext.Current.Response.Redirect("https://banionline.ir/error/410");
                        break;
                    }
                //case "https://banionline.ir/brand/1/lindfeild":
                //    {
                //        HttpContext.Current.Response.Status = "410 Gone";
                //        HttpContext.Current.Response.AddHeader("Location", Request.Url.ToString());
                //        HttpContext.Current.Response.Redirect("https://banionline.ir/error/410");
                //        break;
                //    }

                default:
                    break;
            }


        }
        private void Atashza(string currentUrl)
        {
            switch (currentUrl)
            {
                case "https://atashza.com/search/ProductType1":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://atashza.com/search/fire-gel");
                        break;
                    }
                case "https://atashza.com/search/producttype2":
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", "https://atashza.com/search/industrial-alcohol");
                        break;
                    } 
                case "https://atashza.com/search/productcategory25":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory9":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory16":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory26":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory30":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory6":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory13":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory19":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory21":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory29":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory4":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory12":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory5":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory31":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory32":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory20":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory17":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory22":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory18":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory7":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory23":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory14":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/producttype1":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory27":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                case "https://atashza.com/search/productcategory35":
                    {
                        HttpContext.Current.Response.Status = "410 Gone";
                        HttpContext.Current.Response.AddHeader("Location", currentUrl);
                        HttpContext.Current.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        break;
                    }
                default:
                    break;
            }


        }
        private void Bisehna()
        {
            string currentUrl = Request.Url.ToString();
            if (currentUrl.Contains("bisehna.com/"))
            {
                currentUrl.Replace("bisehna.com/", "sleep-home.com/");

                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", currentUrl);
            }

        }

        private void PingServers()
        {
            string local_type = Enum_Code.SYSTEM_TYPE_LOCAL.ToString();
            string sync_type = Enum_Code.SYSTEM_TYPE_SYNCSERVER.ToString();
            string api_type = Enum_Code.SYSTEM_TYPE_API.ToString();
            string oauth_type = Enum_Code.SYSTEM_TYPE_OAUTH.ToString();

            List<WebsiteDomain> listDomain = _context.WebsiteDomain.Where(p =>
                    p.Website.Code.Label == local_type ||
                    p.Website.Code.Label == sync_type ||
                    p.Website.Code.Label == api_type ||
                    p.Website.Code.Label == oauth_type).ToList();

            while (true)
            {
                foreach (WebsiteDomain domain in listDomain)
                {
                    string url = domain.Domain.Replace("/api/", "");
                    try
                    {
                        var webClient = new WebClient();
                        webClient.DownloadString(url);
                    }
                    catch (Exception) { }
                }
                Thread.Sleep(TimeSpan.FromMinutes(3));
            }
        }

        private void SetWebsiteLanguage()
        {
            //if (HttpContext.Current.Session["LANGUAGE"] == null)
            //{
            //    WebsiteDomain domain = _context.WebsiteDomain.GetByUrl(Request.Url);
            //    if (domain == null)
            //        Session["LANGUAGE"] = "fa";
            //    else
            //    {
            //        Session["LANGUAGE"] = (domain.LanguageId != null) ? domain.Language.Culture : "fa";
            //        if (string.IsNullOrEmpty(domain.RedirectUrl))
            //        {
            //            Template template = domain.Website.Template;
            //            BaseWebsite.WebsiteLabel = (template != null) ? template.Label : "Default";
            //        }
            //        else
            //        {
            //            Session["LANGUAGE"] = "fa";
            //            string currentUrl = Request.Url.ToString().ToLower();
            //            currentUrl = domain.RedirectUrl;
            //            HttpContext.Current.Response.Redirect(currentUrl);
            //        }
            //    }
            //}
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session["LANGUAGE"].ToString());
            //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Session["LANGUAGE"].ToString());
            var currentLang = DataLayer.Base.BaseLanguage.GetCurrentLanguageString();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentLang);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLang);
            Response.AppendHeader("lang", currentLang);

        }

        private void RemoveWwwUrl()
        {
            string currentUrl = Request.Url.ToString().ToLower();
            if (currentUrl.Contains("http://www.") ||
                currentUrl.Contains("https://www."))
            {
                string url = Request.Url.ToString().ToLower();
                url = url.Replace("https://www.", "https://");
                url = url.Replace("http://www.", "http://");
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", url);
            }
        }

        private void IncreaseWebsiteView()
        {
            if (Session["SITEVIEW"] == null)
            {
                Session["SITEVIEW"] = Session.SessionID;
                //try
                //{
                //    _context.WebsiteView.IncreaseView();
                //}
                //catch (Exception ex)
                //{
                //    string msg = ex.Message;
                //}
            }
        }


        protected void Application_BeginRequest()
        {
            string url = Request.Url.ToString()/*.Replace(" ","")*/.Trim();


        }





    }

}

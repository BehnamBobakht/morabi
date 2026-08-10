using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;

namespace Cms.Controllers
{
    public class SeoController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();
        private int baseDivision = 15000;

        public ActionResult Robots()
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine("User-agent: *");
            text.AppendLine("Disallow: /profile/");
            text.AppendLine("Disallow: /login");
            text.AppendLine("Disallow: /register");
            text.AppendLine("Disallow: /signin");
            text.AppendLine("Disallow: /signup");
            text.AppendLine("Disallow: /register*");
            text.AppendLine("Disallow: /cart");
            text.AppendLine("Disallow: /compare");
            text.AppendLine("Disallow: /compare?*");
            text.AppendLine("Disallow: /st/purchaseguide");
            text.AppendLine("Disallow: /st/rules");
            text.AppendLine("Disallow: /seller/");
            text.AppendLine("Disallow: /register/");
            text.AppendLine("Disallow: /register*");
            text.AppendLine("Disallow : / basket/*");
            text.AppendLine("Disallow: / login?*");
            text.AppendLine("Disallow: / profile*");

            text.AppendLine(string.Format("Sitemap: {0}sitemap.xml", BaseWebsite.ShopUrl));
            return Content(text.ToString(), "text/plain");
        }
        public ActionResult SitemapList()
        {
            //string baseUrl = GetBaseUrl();
            string baseUrl = "";
            //sitemap = sitemap == null ? null : sitemap.ToUpper();
            //int pageIndex = pageindex == null ? 1 : pageindex.GetInteger();

            StringBuilder xmlString = new StringBuilder();
            xmlString.AppendLine("<?xml version='1.0' encoding='UTF-8'?>");
            xmlString.AppendLine("<urlset xmlns='http://www.sitemaps.org/schemas/sitemap/0.9' xmlns:image='http://www.google.com/schemas/sitemap-image/1.1'>");
            string lastMod = "";
            try
            {
                lastMod = BaseWebsite.WebsiteSetting.LastContentUpdate != null ? DateTime.Parse(BaseWebsite.WebsiteSetting.LastContentUpdate.Value.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en-US"))).ToString("yyyy-MM-dd") : "";
                AppendUrlIndex(xmlString, baseUrl, BaseWebsite.ShopUrl, lastMod, "1");
            }
            catch { }


            List<ProductTypeVm> listProductType = _context.ProductType.Search(index: 1, pageSize: baseDivision);
            foreach (ProductTypeVm item in listProductType)
            {
                try
                {
                    lastMod = item.SyncDatetime != null ? DateTime.Parse(item.SyncDatetime.Value.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en-US"))).ToString("yyyy-MM-dd") : "";
                    AppendUrlIndex(xmlString, baseUrl, item.Href, lastMod, "0.9");
                }
                catch { }

            }

            List<ProductCategoryVm> listProductCategory = _context.ProductCategory.Search(index: 1, pageSize: baseDivision);
            foreach (ProductCategoryVm item in listProductCategory)
            {
                try
                {
                    lastMod = item.UpdateDatetime != null ? DateTime.Parse(item.UpdateDatetime.Value.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en-US"))).ToString("yyyy-MM-dd") : "";
                    AppendUrlIndex(xmlString, baseUrl, item.Href, lastMod, "0.9");
                }
                catch { }

            }

            List<ProductSubCategoryVm> listProductSubCategory = _context.ProductSubCategory.Search(index: 1, pageSize: baseDivision);
            foreach (ProductSubCategoryVm item in listProductSubCategory)
            {
                try
                {
                    lastMod = item.UpdateDatetime != null ? DateTime.Parse(item.UpdateDatetime.Value.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en-US"))).ToString("yyyy-MM-dd") : "";
                    AppendUrlIndex(xmlString, baseUrl, item.Href, lastMod, "0.9");
                }
                catch { }

            }

            List<ProductBrandVm> listProductBrand = _context.ProductBrand.Search(index: 1, pageSize: baseDivision);
            foreach (ProductBrandVm item in listProductBrand)
            {
                try
                {
                    lastMod = item.SyncDatetime != null ? DateTime.Parse(item.SyncDatetime.Value.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en-US"))).ToString("yyyy-MM-dd") : "";
                    AppendUrlIndex(xmlString, baseUrl, item.Href, lastMod, "0.9");
                }
                catch { }

            }

            ProductSearchVm listProduct = _context.Product.SearchVm(index: 1, pageSize: baseDivision);
            foreach (ProductMinVm item in listProduct.Products)
            {
                try
                {
                    lastMod = item.UpdateDatetime != null ? DateTime.Parse(item.UpdateDatetime.Value.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en-US"))).ToString("yyyy-MM-dd") : "";
                    AppendUrlIndex(xmlString, baseUrl, item.ProductUrl, lastMod, "0.7");

                }
                catch { }
            }

            PostSearchVm listPost = _context.Post.SearchVm(index: 1, pageSize: baseDivision);
            foreach (PostVm item in listPost.Posts)
            {
                lastMod = DateTime.Parse(item.UpdateDateTime.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en-US"))).ToString("yyyy-MM-dd");
                AppendUrlIndex(xmlString, baseUrl, item.Href, lastMod, "0.8");
            }


            xmlString.AppendLine("</urlset>");

            return Content(xmlString.ToString(), "text/xml");
        }

        //public ActionResult SitemapList()
        //{
        //    string baseUrl = GetBaseUrl() + "/";

        //    StringBuilder xmlString = new StringBuilder();
        //    xmlString.AppendLine("<sitemapindex xmlns='http://www.sitemaps.org/schemas/sitemap/0.9'>");

        //    for (int i = 0; i < Math.Ceiling((decimal)(_context.ProductType.SearchCount()) / baseDivision); i++)
        //    {
        //        AppendSiteMapIndex(xmlString, baseUrl, (i + 1), Enum_Sitemap.PRODUCTTYPE);
        //    }
        //    for (int i = 0; i < Math.Ceiling((decimal)(_context.ProductCategory.SearchCount()) / baseDivision); i++)
        //    {
        //        AppendSiteMapIndex(xmlString, baseUrl, (i + 1), Enum_Sitemap.PRODUCTCATEGORY);
        //    }
        //    for (int i = 0; i < Math.Ceiling((decimal)(_context.ProductSubCategory.SearchCount()) / baseDivision); i++)
        //    {
        //        AppendSiteMapIndex(xmlString, baseUrl, (i + 1), Enum_Sitemap.PRODUCTSUBCATEGORY);
        //    }
        //    for (int i = 0; i < Math.Ceiling((decimal)(_context.ProductBrand.SearchCount()) / baseDivision); i++)
        //    {
        //        AppendSiteMapIndex(xmlString, baseUrl, (i + 1), Enum_Sitemap.PRODUCTBRAND);
        //    }
        //    for (int i = 0; i < Math.Ceiling((decimal)(_context.Product.SearchVm().TotalCount) / baseDivision); i++)
        //    {
        //        AppendSiteMapIndex(xmlString, baseUrl, (i + 1), Enum_Sitemap.PRODUCT);
        //    }
        //    for (int i = 0; i < Math.Ceiling((decimal)(_context.Post.SearchCount()) / baseDivision); i++)
        //    {
        //        AppendSiteMapIndex(xmlString, baseUrl, (i + 1), Enum_Sitemap.POST);
        //    }
        //    xmlString.AppendLine("</sitemapindex>");
        //    return Content(xmlString.ToString(), "text/xml");
        //}

        //public ActionResult SitemapInfo(string sitemap = null, string pageindex = null)
        //{
        //    //string baseUrl = GetBaseUrl();
        //    string baseUrl = "";
        //    sitemap = sitemap == null ? null : sitemap.ToUpper();
        //    int pageIndex = pageindex == null ? 1 : pageindex.GetInteger();

        //    StringBuilder xmlString = new StringBuilder();
        //    xmlString.AppendLine("<?xml version='1.0' encoding='UTF-8'?>");
        //    xmlString.AppendLine("<urlset xmlns='http://www.sitemaps.org/schemas/sitemap/0.9' xmlns:image='http://www.google.com/schemas/sitemap-image/1.1'>");

        //    if (sitemap == Enum_Sitemap.PRODUCTTYPE.ToString())
        //    {
        //        List<ProductTypeVm> list = _context.ProductType.Search(index: pageIndex, pageSize: baseDivision);
        //        foreach (ProductTypeVm item in list)
        //        {
        //            AppendUrlIndex(xmlString, baseUrl, item.Href);
        //        }
        //    }
        //    else if (sitemap == Enum_Sitemap.PRODUCTCATEGORY.ToString())
        //    {
        //        List<ProductCategoryVm> list = _context.ProductCategory.Search(index: pageIndex, pageSize: baseDivision);
        //        foreach (ProductCategoryVm item in list)
        //        {
        //            AppendUrlIndex(xmlString, baseUrl, item.Href);
        //        }
        //    }
        //    else if (sitemap == Enum_Sitemap.PRODUCTSUBCATEGORY.ToString())
        //    {
        //        List<ProductSubCategoryVm> list = _context.ProductSubCategory.Search(index: pageIndex, pageSize: baseDivision);
        //        foreach (ProductSubCategoryVm item in list)
        //        {
        //            AppendUrlIndex(xmlString, baseUrl, item.Href);
        //        }
        //    }
        //    else if (sitemap == Enum_Sitemap.PRODUCTBRAND.ToString())
        //    {
        //        List<ProductBrandVm> list = _context.ProductBrand.Search(index: pageIndex, pageSize: baseDivision);
        //        foreach (ProductBrandVm item in list)
        //        {
        //            AppendUrlIndex(xmlString, baseUrl, item.Href);
        //        }
        //    }
        //    else if (sitemap == Enum_Sitemap.PRODUCT.ToString())
        //    {
        //        List<Product> list = _context.Product.Search(index: pageIndex, pageSize: baseDivision);
        //        foreach (Product item in list)
        //        {
        //            AppendUrlIndex(xmlString, baseUrl, item.GetLink());
        //        }
        //    }
        //    else if (sitemap == Enum_Sitemap.POST.ToString())
        //    {
        //        List<Post> list = _context.Post.Search(index: pageIndex, pageSize: baseDivision);
        //        foreach (Post item in list)
        //        {
        //            AppendUrlIndex(xmlString, baseUrl, item.GetLink());
        //        }
        //    }

        //    xmlString.AppendLine("</urlset>");

        //    return Content(xmlString.ToString(), "text/xml");
        //}

        public ActionResult GoogleWebmaster(string webmasterId)
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine("google-site-verification: google" + webmasterId + ".html");
            return Content(text.ToString(), "text/plain");
        }

        private void AppendUrlIndex(StringBuilder xmlString, string baseUrl, string url, string lastmod, string priority = "0.9")
        {
            xmlString.AppendLine("<url>");
            xmlString.AppendLine("<loc>" + baseUrl + url + "</loc>");
            xmlString.AppendLine("<lastmod>" + lastmod + "</lastmod>");
            xmlString.AppendLine("<changefreq>daily</changefreq>");
            xmlString.AppendLine("<priority>" + priority + "</priority>");
            xmlString.AppendLine("</url>");
        }

        private void AppendSiteMapIndex(StringBuilder xmlString, string baseUrl, int pageIndex, Enum_Sitemap sitemap)
        {
            xmlString.AppendLine("<sitemap>");

            //xmlString.AppendLine("<loc>");
            xmlString.AppendLine($"<loc>{baseUrl}sitemap-{sitemap.ToString().ToLower()}-{pageIndex}.xml</loc>".Replace(" ", ""));
            //xmlString.AppendLine(baseUrl + "sitemap-" + sitemap.ToString().ToLower() + "-" + pageIndex + ".xml");
            //xmlString.AppendLine("</loc>");
            xmlString.AppendLine("</sitemap>");
        }

        private string GetBaseUrl()
        {
            string baseUrl = HttpContext.Request.Url.Host;
            baseUrl = baseUrl == "localhost" ? baseUrl + ":" + HttpContext.Request.Url.Port : baseUrl;
            baseUrl = "https://" + baseUrl;
            return baseUrl;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
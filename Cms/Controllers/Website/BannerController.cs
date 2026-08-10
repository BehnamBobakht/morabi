using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class BannerController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? baid, string baidname = "")
        {
            Banner banner = _context.Banner.GetById(baid);
            banner.ClickCount = banner.ClickCount + 1;
            _context.Banner.Update(banner);
            _context.Save();
            if (banner.Url == "" || banner.Url == null)
                return Redirect("/");
            return Redirect(banner.Url);
        }
        [CustomCache(Duration =1, cacheType = Enum_OutputCache.BannerSearch,VaryByParam ="*")]

        public ActionResult Search(
            string bacategory = null,
            int? bacategoryId = null,
            int? baIndex = null,
            int? baSize = null,
            string baviewName = null,
            Enum_BannerOrder baorder = Enum_BannerOrder.NONE,
                        string lang = ""
)
        {
            baIndex = baIndex != null ? baIndex.Value : 1;
            baSize = baSize != null ? baSize.Value : 10;
            List<Banner> list = _context.Banner.Search(
                categoryId: bacategoryId,
                categoryLabel: bacategory,
                index: baIndex.Value,
                pageSize: baSize.Value,
                order: baorder);
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, baviewName, list);
        }

        public ActionResult Search2(
           string bacategory = null,
           int? bacategoryId = null,
           int? baIndex = null,
           int? baSize = null,
           string baviewName = null,
           Enum_BannerOrder baorder = Enum_BannerOrder.NONE,
                       string lang = ""
)
        {
            baIndex = baIndex != null ? baIndex.Value : 1;
            baSize = baSize != null ? baSize.Value : 10;
            List<Banner> list = _context.Banner.Search(
                categoryId: bacategoryId,
                categoryLabel: bacategory,
                index: baIndex.Value,
                pageSize: baSize.Value,
                order: baorder);
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, baviewName, list);
        }

        public ActionResult SearchCategory(
            string bacategory = null,
            int? bacategoryId = null,
            int? baIndex = null,
            int? baSize = null,
            string baviewName = null,
            Enum_BannerOrder baorder = Enum_BannerOrder.NONE,
                        string lang = ""
)
        {
            baIndex = baIndex != null ? baIndex.Value : 1;
            baSize = baSize != null ? baSize.Value : 10;
            List<Banner> list = _context.Banner.Search(
                categoryId: bacategoryId,
                categoryLabel: bacategory,
                index: baIndex.Value,
                pageSize: baSize.Value,
                order: baorder);
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, baviewName, list);
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
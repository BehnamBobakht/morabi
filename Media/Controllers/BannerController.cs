using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Media.Controllers
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
            if (string.IsNullOrWhiteSpace(baviewName))
                return PartialView(list);

            return PartialView(baviewName, list);
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
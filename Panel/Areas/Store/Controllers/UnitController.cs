using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
namespace Panel.Areas.Store.Controllers
{
    public class UnitController : Controller
    {
        // GET: Store/Unit
        private UnitOfWork _context = new UnitOfWork();
        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_PriceUnit);
            return View(_context.Website.GetAll().ToView());
        }
        public ActionResult Update()
        {
            var setting = BaseWebsite.WebsiteSetting;
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_PriceUnit);
            var field = _context.ShopWebsiteSetting.Where(x => x.WebsiteId == setting.WebsiteId).FirstOrDefault();
            return View(field);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int WebsiteId = 0, double UnitPrice = 0)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_PriceUnit);
            var field = _context.ShopWebsiteSetting.Where(x => x.WebsiteId == WebsiteId).FirstOrDefault();
            field.UnitPrice = UnitPrice;
            _context.ShopWebsiteSetting.Update(field);
            _context.Save();
            _context.Product.UpdateAllProductPrice(_context);
            return View(field);
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
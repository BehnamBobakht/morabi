using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.Entities;
using DataLayer.Base;
using Cms.Areas.Store.Security;


namespace Cms.Areas.Store.Controllers
{

    public class ShippingStateController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
    [AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]

        public ActionResult Index()
        {

            var currentAccount = BaseWebsite.CurrentAccount;
            var model = _context.SendType.GetByStoreId(currentAccount.StoreId);
            
            return View(model);
        }
        [HttpPost]
        [AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]

        public ActionResult Index(SendType shipping)
        {

            var currentAccount = BaseWebsite.CurrentAccount;
            shipping.StoreId = currentAccount.StoreId;
            _context.SendType.EditByStore(shipping);
            return RedirectToAction("index");
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
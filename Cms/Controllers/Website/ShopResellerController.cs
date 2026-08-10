using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class ShopResellerController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? rsid, string rsidname = null)
        {
            ShopReseller reseller = _context.ShopReseller.GetById(rsid);
            if (reseller == null)
                return RedirectPermanent("/error/404");
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, null, reseller);
        }

        [HttpGet]
        public ActionResult Search(
            int? rsnotId = null, 
            int? rsshopId = null, 
            int? rsindex = null, 
            int? rspageSize = null, 
            string rsname = null, 
            string rsviewName = null,
            Enum_ShopResellerOrder rsorder = Enum_ShopResellerOrder.NONE)
        {
            List<ShopReseller> results = _context.ShopReseller.Search(
                shopId: rsshopId,
                index: rsindex,
                name: rsname,
                notId: rsnotId,
                pageSize: rspageSize);

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, rsviewName, results);
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
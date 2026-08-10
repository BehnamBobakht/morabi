using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class ShopResellerCollectionController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Search(string scviewname = null)
        {
            List<ShopResellerCollection> results = _context.ShopResellerCollection.Search();
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, scviewname, results);
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
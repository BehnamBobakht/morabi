using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class MerchantController : Controller
    {
        // GET: Merchant
        UnitOfWork _context = new UnitOfWork();
        public ActionResult GetActiveMerchants()
        {
            var activeMerchants = _context.Merchant.GetAllActive();
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, "Merchants", activeMerchants);
        }
    }
}
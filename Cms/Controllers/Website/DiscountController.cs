using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class DiscountController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(
            Enum_Code dcType = Enum_Code.DISCOUNT_TYPE_NONE,
            string dclabel = null,
            int? dcnotId = null,
            int? dcIndex = null,
            int? dcSize = null,
            string dcviewName = null,
           Boolean? active =null
            )
        {
            dcIndex = dcIndex != null ? dcIndex.Value : 1;
            dcSize = dcSize != null ? dcSize.Value : 10;
            var list = _context.Discount.Search(
                type: dcType,
                notId: dcnotId,
                label: dclabel,
                index: dcIndex.Value,
                pageSize: dcSize.Value,
                active : active
                );
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, dcviewName, list);
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
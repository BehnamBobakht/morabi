using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class ColorController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ActionResult Search(
            int? clnotId = null,
            int? cltypeId = null,
            int? clindex = null,
            int? clpageSize = null,
            string clname = null,
            string clviewName = null)
        {
            List<ColorVm> results = new List<ColorVm>();
            if (cltypeId != null)
                results = _context.Color.Search(
                typeId: cltypeId,
                index: clindex,
                name: clname,
                notId: clnotId,
                pageSize: clpageSize);

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, clviewName, results);
        }
        [HttpGet]
        public ActionResult SearchPage(ProductSearchVm productSearch)
        {

            productSearch.Colors = _context.Color.Search(pageSize: 100, typeId: productSearch.ProductTypeId);

            return PartialView(BaseController.GetView(this), productSearch);
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
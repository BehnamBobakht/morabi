using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers
{
    public class ProductSubCategoryController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Search(
            string pscategoryId = null,
            int? psnotId = null,
            int? psindex = null,
            int? pspageSize = null,
            string psname = null,
            string psviewName = null,
            string pscustomLabel = null,
            Enum_ProductCategoryOrder psorder = Enum_ProductCategoryOrder.NONE)
        {
            pspageSize = pspageSize == null ? 10 : pspageSize;
            psindex = psindex == null ? 1 : psindex;
            List<ProductSubCategoryVm> list = _context.ProductSubCategory.Search(
                active:true,
                categoryId: pscategoryId,
                notId: psnotId,
                index: psindex.Value,
                name: psname,
                pageSize: pspageSize.Value,
                order: psorder,
                customLabel: pscustomLabel
                );
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, psviewName, list);
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
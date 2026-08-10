using Binbin.Linq;
using DataLayer.Base;
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
    public class ProductCategoryController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
        public ActionResult Index(string id)
        {
            var model = _context.ProductCategory.GetByCustomLabel(id);
            return PartialView("~/Views/" + BaseWebsite.WebsiteLabel + "/ProductCategory/Index.cshtml", model);
        }
        public ActionResult Search(
            int? pctypeId = null, 
            int? pcnotId = null, 
            int? pcindex = null, 
            int? pcpageSize = null, 
            string pcname = null, 
            string pcviewName = null,
            string pccustomLabel = null,
            Enum_ProductCategoryOrder pcorder = Enum_ProductCategoryOrder.NONE,
                        string lang = ""

            )
        {
            var MyQuery = PredicateBuilder.True<ProductCategoryVm>();
            pcpageSize = pcpageSize == null ? 10 : pcpageSize;
            pcindex = pcindex == null ? 1 : pcindex;
            List<ProductCategoryVm> list = _context.ProductCategory.Search(
                active:true,
                notId: pcnotId,
                index: pcindex.Value,
                name: pcname,
                pageSize: pcpageSize.Value,
                typeId: pctypeId,
                order:pcorder,
                customLabel : pccustomLabel);            
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, pcviewName, list);
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
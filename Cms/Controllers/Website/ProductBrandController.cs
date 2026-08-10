using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.ViewModels;

namespace Cms.Controllers
{
    public class ProductBrandController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? pbId = null, string pbidname = null)
        {
            ProductBrand productBrand = _context.ProductBrand.GetById(pbId);
            if (productBrand == null || productBrand.Active == false)
                return RedirectPermanent("/error/404");

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, null, productBrand);
        }

        public ActionResult Search(
            int? pbnotId = null, 
            int? pbshopId = null, 
            int? pbtypeId = null,
            int? pbcategoryId = null,
            int? pbsubcategoryId = null,
            int? pbindex = null, 
            int? pbpageSize = null, 
            string pbId = null,
            string pbname = null, 
            string pbviewName = "search", 
            Enum_ProductBrandOrder pborder = Enum_ProductBrandOrder.NONE)
        {
            pbindex = pbindex != null ? pbindex.Value : 1;
            pbpageSize = pbpageSize != null ? pbpageSize.Value : 100;
            List<ProductBrandVm> list = _context.ProductBrand.Search(
                pbId: pbId,
                shopId: pbshopId,
                index: pbindex.Value,
                name: pbname,
                notId: pbnotId,
                pageSize: pbpageSize.Value,
                typeId:pbtypeId,
                categoryId: pbcategoryId,
                subcategoryId: pbsubcategoryId,
                order: pborder);
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, pbviewName, list);
        }

        public ActionResult SearchPage(ProductSearchVm productSearch)
        {

            productSearch.ProductBrand = _context.ProductBrand.Search(pageSize: 100, typeId: productSearch.ProductTypeId);

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
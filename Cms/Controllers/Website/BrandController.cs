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
    public class BrandController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int brbrandId , string brname = "", int? prpageSize = null, int? prindex = null, bool? hasDiscount = null, bool? prstock = null,
 Enum_ProductOrder prorder = Enum_ProductOrder.MORE_SELL)
        {
            var brandVm = _context.ProductBrand.GetBrandVm(brbrandId);
            if(brandVm == null)
            {
                return Redirect(BaseController.RedirectToError(errorCode: 410));
            }
            var result = new ProductBrandSerachVm();
            result.ProductBrandVm = brandVm;
            prpageSize = prpageSize == null ? 48 : prpageSize;
            prindex = prindex == null ? 1 : prindex;
            result.ProductSearchVm = _context.Product.SearchVm(index: prindex.Value, pageSize: prpageSize.Value, brandId: brbrandId.ToString(), order: prorder,hasDiscount:hasDiscount,prstock:prstock);
            result.ProductSearchVm.HasDiscount = hasDiscount;
            result.ProductSearchVm.Prstock = prstock;
            return PartialView(BaseController.GetView(this), result);

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
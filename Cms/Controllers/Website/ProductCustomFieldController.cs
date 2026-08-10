using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers
{
    public class ProductCustomFieldController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Search (int? pageSize = null, string name = null, string syncName = null, string ViewName= null, int? productTypeId = null ,int? productCategoryId = null ,int? productSubCategoryId = null ,bool inSearchResult=false)
        {

            List<ProductCustomFieldVm> list = _context.ProductCustomField.SearchVm(pageSize: pageSize, typeId: productTypeId, categoryId: productCategoryId, subcategoryId: productSubCategoryId, inSearchResult: inSearchResult);
                

            return PartialView(BaseController.GetView(this, ViewName), list);
        }
        public ActionResult SearchPage(ProductSearchVm productSearch)
        {
            productSearch.ProductCustomFields = _context.ProductCustomField.SearchVm(typeId: productSearch.ProductTypeId, categoryId: productSearch.ProductCategoryId, subcategoryId: productSearch.ProductSubCategoryId, inSearchResult: true);
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
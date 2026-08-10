using Binbin.Linq;
using Cms.Controllers.Website;
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
    public class ProductTypeController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
        public ActionResult Index(string id)
        {
            var model = _context.ProductType.GetByCustomLabel(id);
            return PartialView("~/Views/" + BaseWebsite.WebsiteLabel + "/ProductType/Index.cshtml",model);
        }
        [CustomCache(Duration = 1, cacheType = Enum_OutputCache.ProductTypeMenu,
   VaryByParam = "*")]
        public ActionResult Search(string ptId = null, int? ptnotId = null, int? ptshopId = null, int? ptindex = null, int? ptpageSize = null, string ptname = null, string ptviewName = null, Boolean? isService = null, Boolean? isActive = null, string ptcustomLabel = null,
                        string lang = "")
        {
            List<ProductTypeVm> results = _context.ProductType.Search(
                Id: ptId,
                shopId: ptshopId,
                index: ptindex,
                name: ptname,
                notId: ptnotId,
                pageSize: ptpageSize,
               isService: isService,
               isActive:isActive,
               customLabel: ptcustomLabel
                );
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, ptviewName, results);
        }
        [CustomCache(Duration = 1, cacheType = Enum_OutputCache.ProductTypeMenu,
VaryByParam = "*")]
        public ActionResult Search2(string ptId = null, int? ptnotId = null, int? ptshopId = null, int? ptindex = null, int? ptpageSize = null, string ptname = null, string ptviewName = null, Boolean? isService = null, Boolean? isActive = null, string ptcustomLabel = null,
                   string lang = "")
        {
            List<ProductType> results = _context.ProductType.Search2(
                Id: ptId,
                shopId: ptshopId,
                index: ptindex,
                name: ptname,
                notId: ptnotId,
                pageSize: ptpageSize,
               isService: isService,
               isActive: isActive,
               customLabel: ptcustomLabel
                );
            return PartialView(BaseController.GetView(this, ptviewName), results);
            //return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, ptviewName, results);
        }
        public ActionResult SearchCustom(string ptId = null, int? ptnotId = null, int? ptshopId = null, int? ptindex = null, int? ptpageSize = null, string ptname = null, string ptviewName = null, Boolean? isService = null, Boolean? isActive = null,string typeId="",string categoryId="",string subcategoryId="")
        {
            List<ProductTypeVm> results = _context.ProductType.Search(
                Id: ptId,
                shopId: ptshopId,
                index: ptindex,
                name: ptname,
                notId: ptnotId,
                pageSize: ptpageSize,
               isService: isService,
               isActive: isActive

                );
            ViewProductTypeCustom model = new ViewProductTypeCustom();
            model.ProductTypes = results;
            model.TypeId = typeId;
            model.CategoryId = categoryId;
            model.SubCategoryId = subcategoryId;
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, ptviewName, model);
        }
        public ActionResult SearchOption(int ? typeId = null,int ? categoryId=null,int ? subcategoryId=null,string ptviewName="")
        {
            //var list = _context.ProductOption.Where(s =>  s.ProductTypeId==typeId ).ToList();

            List<ProductOption> list = _context.ProductOption.Search(
                typeId: typeId
            );
            List<ProductOption> results = list
                .Where( s => s.ProductOptionItem.Any
                    (c => c.ProductQuantity.Count > 0 && c.ProductQuantity.Any(x=>(x.Product.ProductSubCategoryId == subcategoryId || subcategoryId == null) && (categoryId == null || x.Product.ProductCategoryId == categoryId)))).ToList();

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, ptviewName, results);
        }
        public ActionResult SearchPage(ProductSearchVm productSearch,bool ? isService=false)
        {
            productSearch.ProductTypes = _context.ProductType.Search(pageSize: 100, isActive: true, isService: isService);
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
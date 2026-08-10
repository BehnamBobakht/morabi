using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using DataLayer.ViewModels;
using DataLayer.Api;

namespace Cms.Controllers
{
    public class StoreController : Controller
    {
        UnitOfWork _context = new UnitOfWork();


        public ActionResult Search(
            int? stnotId = null, 
            int? sttypeId = null, 
            int? stindex = null, 
            int? stpageSize = null, 
            string stId = null,
            string stname = null, 
            string stviewName = null,
            Boolean? stactive = null,
            int? categoryId = null, 
            int? subcategoryId=null)
        {
            stindex = stindex != null ? stindex.Value : 1;
            stpageSize = stpageSize != null ? stpageSize.Value : 10;
            List<Store> list = _context.Store.Search(
                stId: stId,
                index: stindex.Value,
                name: stname,
                notId: stnotId,
                pageSize: stpageSize.Value,
                typeId:sttypeId,
                active:stactive,
                categoryId: categoryId,
                subcategoryId: subcategoryId
                );
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, stviewName, list);
        }

        public ActionResult SearchCompetitiveFeature(string stviewName = null)
        {
            List<CompetitiveFeature> list = _context.CompetitiveFeature.Search();
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, stviewName, list);
        }
        public ActionResult Index(string  prstoreId="", string stname = "",int ? prpageSize=null,int ? prindex=null, Enum_ProductOrder prorder = Enum_ProductOrder.MORE_SELL)
        {
            ViewSearchStore viewmodel = new ViewSearchStore();
            prpageSize = prpageSize == null ? 48 : prpageSize;
            prindex = prindex == null ? 1 : prindex;
            var searchQuery = _context.Product.GetSearchQuery(storeId: prstoreId);
            List<Product> results = _context.Product.Search(
             pageSize: prpageSize.Value,
             index: prindex.Value,
            order: prorder,
            query: searchQuery);
            ViewSearchProduct search = new ViewSearchProduct()
            {
                TotalCount = _context.Product.SearchDetail(storeId: prstoreId).TotalCount,
                PageIndex = prindex.Value,
                PageSize = prpageSize.Value,
                Results = results
            };
            viewmodel.SearchProduct = search;
            var store = _context.Store.GetById(int.Parse(prstoreId));
            viewmodel.Store = store;
            return PartialView(BaseController.GetView(this), viewmodel);
           
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
//using DataLayer.Enumarables;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using DataLayer.Base;
//using DataLayer.Entities;
//using DataLayer.ViewModels.Report;
//using Stimulsoft.Report;
//using Stimulsoft.Report.Mvc;

//namespace Panel.Areas.Report.Controllers
//{
//    public class StoreController : Controller
//    {
//        private UnitOfWork _context = new UnitOfWork();

//        public ActionResult StoreProducts( int? statusId = null, int? storeId = null, string name = "")
//        {
//           // _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StoreProduct);

//           //var index =  1 ;
//           //var pagesize = 9999999;
//           // int stockCount = 0;
//           // var model = _context.StoreProduct.Search(stockCount: out stockCount, statusId: statusId, index: index, pageSize: pagesize, name: name, storeId: storeId);
//           // var report = new StoreProductsReport();
//           // report.Date = DateTime.Now.ToPersianWithTime();
//           // report.TotalCount = stockCount;
//           // report.Store ="همه فروشندگان";

//           // if(storeId.HasValue)
//           // {
//           //     var store = _context.Store.GetById(storeId);
//           //     if(store!=null)
//           //     {
//           //         report.Store = store.Name;
//           //     }
//           // }
//           // int count = 1;
//           // foreach(var p in model.OrderBy(o => o.Product.GetName()).ToList())
//           // {
//           //     var repItem = new StoreProductsReportItem()
//           //     {
//           //         Id=count,
//           //         Name=p.Product.GetName(),
//           //         Count= p.StoreProductQuantity.Sum(s => s.Count),
//           //         StoreName=p.Store.Name
//           //     };
//           //     report.Items.Add(repItem);
//           //     count++;  
//           // }
//           // report.Items = report.Items;
//           // TempData["StoreProducts"] = report;
//            return View();
//        }
//        //public ActionResult StoreProductsReport()
//        //{
//        //    var report = new StiReport();
//        //    report.Load(Server.MapPath("/Panel/Reports/StoreProductsReport.mrt"));
//        //    report.Compile();
//        //    var data = TempData["StoreProducts"] as StoreProductsReport;
//        //    report.RegBusinessObject("StoreProductsReport", data);
//        //    return StiMvcViewer.GetReportSnapshotResult(report);
//        //}

//        public ActionResult ViewerEvent()
//        {
//            return StiMvcViewer.ViewerEventResult(HttpContext);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                _context.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
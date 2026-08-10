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
//using DataLayer.ViewModels;
//using static DataLayer.ViewModels.ViewStateReport;

//namespace Panel.Areas.Report.Controllers
//{
//    public class StateController : Controller
//    {
//        private UnitOfWork _context = new UnitOfWork();

//        public ActionResult Index( int? countryId = null, int? StateId = null)
//        {
//            //_context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StoreProduct);

//            //var index = 1;
//            //var pagesize = 9999999;
//            //int stockCount = 0;
//            //var model = _context.AccountOrder.Search(/*stockCount: out*/ /*stockCount*/ stateId: StateId, index: index, pageSize: pagesize);
//            //var report = new ViewStateReport();
//            //report.Date = DateTime.Now.GetDateString();
//            //report.TotalCount =model.Count;


//            //report.State = "";
//            //if (StateId.HasValue)
//            //{
//            //    var state = _context.State.GetById(StateId);
//            //    if (state != null)
//            //    {
//            //        report.State = state.Name;
//            //    }
//            //}
//            //int count = 1;
//            //foreach (var p in model.OrderByDescending(s=>s.Datetime).ToList())
//            //{
//            //    var repItem = new ProductsReportItem()
//            //    {
//            //        Id = p.ID,
//            //        Datetime = p.Datetime.ToLongDateString(),
//            //        Price = p.Price.ToString(),
//            //        ShippingPrice = p.SendPrice.ToString(),
//            //    };
//            //    report.Items.Add(repItem);
//            //    count++;
//            //}
//            //report.Items = report.Items;
//            //TempData["StateOrder"] = report;
//            return View();
//        }
//        public ActionResult StateOrderReport()
//        {
//            var report = new StiReport();
//            report.Load(Server.MapPath("/Panel/Report/StateOrderReport.mrt"));
//            report.Compile();
//            var data = TempData["StateOrder"] as ViewStateReport;
//            report.RegBusinessObject("StateOrderReport", data);
//            return StiMvcViewer.GetReportSnapshotResult(report);
//        }

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
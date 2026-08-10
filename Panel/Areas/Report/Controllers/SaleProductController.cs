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

//namespace Panel.Areas.Report.Controllers
//{
//    public class SaleProductController : Controller
//    {
//        private UnitOfWork _context = new UnitOfWork();

//        public ActionResult index(Enum_ProductOrder prorder = Enum_ProductOrder.LESS_SELL)
//        {
//            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StoreProduct);
//            var index = 1;
//            var pagesize = 99999;
           
//            List<Product> list = new List<Product>();
//            var model  = _context.Product.Search(
//           order: prorder,
//           index: index, pageSize: pagesize);
//            var report = new ProductsReport();
//            report.Date = DateTime.Now.ToPersian();
//            report.TotalCount = _context.Product.Search(
//           order: prorder).ToList().Count;
//            if (prorder== Enum_ProductOrder.MORE_SELL)
//            {
//                    report.OrderType = "گزارش محصولات پر فروش";
//            }
//            else
//            {
//                report.OrderType = "گزارش محصولات کم فروش";
//            }
//            int count = 1;
           
//            foreach (var p in model)
//            {
//                var repItem = new ProductsSaleReportItem()
//                {
//                    Id = p.ID,
//                    Name = p.GetName(),
//                    Count = p.AccountOrderProduct.Sum(s => s.Count),
//                    TotalPrice = p.AccountOrderProduct.Sum(s => s.Price)
//                };
//                report.Items.Add(repItem);
//                count++;
//            }
//            if(prorder == Enum_ProductOrder.MORE_SELL)
//            {
//                report.Items = report.Items.OrderByDescending(s=>s.Count).ToList();
//            }
//            else
//            {
//                report.Items = report.Items.Where(x=>x.TotalPrice!=0).OrderBy(s => s.Count ).ToList();
//            }
            
//            TempData["Products"] = report;
//            return View();
//        }
//        public ActionResult ProductsReport()
//        {
//            var report = new StiReport();
//            report.Load(Server.MapPath("/Panel/Report/ProductsReport.mrt"));
//            report.Compile();
//            var data = TempData["Products"] as ProductsReport;
//            report.RegBusinessObject("ProductsReport", data);
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
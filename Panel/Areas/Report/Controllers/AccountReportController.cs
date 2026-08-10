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
//using System.Globalization;

//namespace Panel.Areas.Report.Controllers
//{
//    public class AccountReportController : Controller
//    {
//        private UnitOfWork _context = new UnitOfWork();

//        public ActionResult index(string from = null, string to = null)
//        {
//            var index = 1;
//            var pagesize = 99999;
//            //if (!string.IsNullOrEmpty(from))
//            //{
//            //    string[] stringfrom = from.Split('/');
//            //    from = stringfrom[1] + "/" + stringfrom[0] + "/" + stringfrom[2];
//            //}
//            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
//            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();           
//            var TotalCount = _context.Account.SearchCount(
//                    fromDatetime: fromDate,
//                    toDatetime: toDate
//                 );
//            List<Account> model = _context.Account.Search(
//                    index: index,
//                    pageSize: pagesize,
//                    fromDatetime: fromDate,
//                    toDatetime: toDate
//                 )
//                .ToList();
//            var report = new AccountReport();
//            var list = model.ToView();
//            report.Date = DateTime.Now.ToPersian();
//            report.TotalCount = TotalCount;
//            report.Subject = "";
//            int count = 1;

//            foreach (var p in list)
//            {
//                var repItem = new AccountReportItem()
//                {
//                    Id = p.Id,
//                    FullName = p.FullName,
//                    Email = p.Email,
//                    Mobile = p.Mobile,
//                    BirthDay = p.BirthDay.ToPersian(),
//                    RegisterDate=p.CreateDate
//                };
//                report.Items.Add(repItem);
//                count++;
//            }

//            TempData["Accounts"] = report;
//            return View();
//        }
//        public ActionResult AccountReport()
//        {
//            var report = new StiReport();
//            report.Load(Server.MapPath("/Panel/Report/AccountReport.mrt"));
//            report.Compile();
//            var data = TempData["Accounts"] as AccountReport;
//            report.RegBusinessObject("AccountReport", data);
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
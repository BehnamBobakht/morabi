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
//    public class AccountOrderController : Controller
//    {
//        private UnitOfWork _context = new UnitOfWork();

//        public ActionResult index(string from = null,
//            string to = null, int[] stores = null,int[] statusId = null, string customer = null,  string status = null, string product = null, string IsLastOrder = null, string IsOne = null)
//        {
//            if (statusId == null && status != null)
//            {
//                statusId = status.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
//            }
//            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StoreProduct);
//            var index = 1;
//            var pagesize = 99999;

//            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
//            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();
//            List<AccountOrder> model = new List<AccountOrder>();
//            var TotalCount = 0;
//            if (!string.IsNullOrEmpty(IsLastOrder))
//            {
//                var today = DateTime.Today;
//                var last = today.AddMonths(-2);
               
//                fromDate = last;
//                 TotalCount = _context.AccountOrder.SearchCount(
//                    stores: stores,
//                    fromDatetime: fromDate,
//                    customer: customer,
//                    statusId: statusId,
//                    product: product,
//                    IsOne: IsOne
//                 );

//                model = _context.AccountOrder.Search(
//                        index: index,
//                        pageSize: pagesize,
//                        fromDatetime: fromDate,
//                        customer: customer,
//                        stores: stores,
//                        product: product,
//                        statusId: statusId,
//                        IsOne: IsOne)
//                    .ToList();

//            }
//            else
//            {
//                 TotalCount = _context.AccountOrder.SearchCount(
//                    stores: stores,
//                    fromDatetime: fromDate,
//                    toDatetime: toDate,
//                    customer: customer,
//                    statusId: statusId,
//                    product: product,
//                    IsOne: IsOne
//                 );

//                model = _context.AccountOrder.Search(
//                        index: index,
//                        pageSize: pagesize,
//                        fromDatetime: fromDate,
//                        toDatetime: toDate,
//                        customer: customer,
//                        stores: stores,
//                        product: product,
//                        statusId: statusId,
//                        IsOne: IsOne)
//                    .ToList();
//            }

           
            
//            var report = new AccountOrdersReport();
//            //var list = model.ToView();
//            int row = 1;
//            foreach (var item in model)
//            {
//                foreach (var productItemOrder in item.AccountOrderProduct)
//                {
//                    OrderReportItem orderItem =new OrderReportItem();
//                    orderItem.ProductName = productItemOrder.Product.Name;
//                    orderItem.Id = item.ID;
//                    orderItem.Row = row;
//                    AccountOrder parenOrder = item.AccountOrder2;
//                    if (productItemOrder.ProductDiscount != 0)
//                    {
//                        orderItem.Discount = (productItemOrder.ProductPrice - productItemOrder.ProductDiscount)*10;
//                    }
//                    if (parenOrder.RebateId != null)
//                    {
//                        orderItem.Rebate = parenOrder.Rebate.Name + "-" + parenOrder.RebatePrice;

//                    }
//                    orderItem.Count = productItemOrder.Count;
//                    orderItem.Status = new ViewCode(item.Code).Name;
//                    orderItem.StoreName = item.StoreId.HasValue ? item.Store.Name : "";
//                    orderItem.FullName = item.Account.FullName;
                   
                   

//                    //switch (item.Status.Label)
//                    //{
//                    //    case nameof(Enumarables.Enum_Code.ORDER_STATUS_SUCCESS):
//                    //    {
//                    //        this.NextStatus = Enumarables.Enum_Code.ORDER_STATUS_POST.ToString();
//                    //        break;
//                    //    }

//                    //}
//                    orderItem.UnitPrice = productItemOrder.ProductPrice*10;
//                    orderItem.TotalPrice = productItemOrder.Price*10;
//                    orderItem.Datetime = item.Datetime.ToPersian();
//                    report.Items.Add(orderItem);
//                    row = row+ 1;
//                }
//            }
//            report.Date = DateTime.Now.ToPersian();
//            report.TotalCount = TotalCount;
//            report.Subject = "";
//            int count = 1;
            
            
//            //foreach (var p in list)
//            //{
//            //    var repItem = new OrderReportItem()
//            //    {
//            //        Id = p.Id,
//            //        FullName = p.Account.FullName,
//            //        Count = p.ProductCount,
//            //        TotalPrice = p.Price.ToString(),
//            //        Status=p.Status.Name,
//            //        StoreName=p.StoreName,
//            //        Datetime=p.Datetime.ToPersian(),
//            //        ProductName = p.ProductName
//            //    };
//            //    report.Items.Add(repItem);
//            //    count++;
//            //}

//            TempData["Products"] = report;
//            return View();
//        }
//        public ActionResult ProductsReport()
//        {
//            var report = new StiReport();
//            report.Load(Server.MapPath("/Panel/Report/AccountOrdersReport.mrt"));
//            report.Compile();
//            var data = TempData["Products"] as AccountOrdersReport;
//            report.RegBusinessObject("AccountOrdersReport", data);
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
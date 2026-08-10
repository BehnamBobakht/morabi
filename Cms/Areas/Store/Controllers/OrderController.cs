using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Base;
using DataLayer.Entities;
using Cms.Areas.Store.Security;

namespace Cms.Areas.Store.Controllers
{
    public class OrderController : Controller
    {
         UnitOfWork _context = new UnitOfWork();

        [AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]

        public ActionResult Index(int index = 1, int pageSize = 10, string name = null,int? statusId=null,string refId=null,string customer=null,string from="",string to="")
        {

            if (statusId == 0)
                statusId = null;
            var currentAccount = DataLayer.Base.BaseWebsite.CurrentAccount;
            var statuses = _context.Code.GetAllByCodeGroup(DataLayer.Enumarables.Enum_CodeGroup.ORDER_STATUS,true,"store");
            ViewBag.Statuses = statuses;
            var statusLables = statuses.Select(s => s.Label).ToList();
            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();

            var model = _context.AccountOrder.Search(index:index, pageSize: pageSize,accountId: null,storeId: currentAccount.StoreId,merchantId: null,refId: null,statusId:(statusId.HasValue? new int[] { statusId.Value }: new int[] { }),statusStr: statusLables,customer: name,product:name, fromDatetime:fromDate, toDatetime:toDate);
            ViewBag.TotalCount = _context.AccountOrder.SearchCount(accountId: null, storeId: currentAccount.StoreId, merchantId: null, refId: null, statusId: (statusId.HasValue ? new int[] { statusId.Value } : new int[] { }), statusStr: statusLables, customer: name, product: name, fromDatetime: fromDate, toDatetime: toDate);
            ViewBag.pageSize = pageSize.ToString();
            return View(model.ToView());
        }
        public ActionResult OrderProducts(int id)
        {
            if (!BaseStore.StoreOrderId_IsValid(id, _context))
          return View("Invalid");
            var currentAccount = DataLayer.Base.BaseWebsite.CurrentAccount;
            var model = _context.AccountOrder.FirstOrDefault(s=>s.ID==id);
            return View(model);
        }
        public ActionResult ChangeStatus(int id)
        {
            if (!BaseStore.StoreOrderId_IsValid(id, _context))
          return View("Invalid");
            var currentAccount = DataLayer.Base.BaseWebsite.CurrentAccount;
            _context.AccountOrder.ChangeStatus(id,_context);
            return RedirectToAction("Index");
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
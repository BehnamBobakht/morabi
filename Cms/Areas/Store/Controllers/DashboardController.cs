using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Entities;
using Cms.Areas.Store.Security;


namespace Cms.Areas.Store.Controllers
{
    public class DashboardController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
        [AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]
        public ActionResult Index() 
        {
            return View(); 
        }
        [AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]

        public ActionResult SuccessStatusCount()
        {

            var currentAccount = DataLayer.Base.BaseWebsite.CurrentAccount;
            var status=_context.Code.GetByLabel(DataLayer.Enumarables.Enum_Code.ORDER_STATUS_SUCCESS);
            var validStatusIds = _context.Code.GetAllByCodeGroup(DataLayer.Enumarables.Enum_CodeGroup.ORDER_STATUS, false, "store").Select(s=>s.ID).ToArray();
            var countAll = _context.AccountOrder.SearchCount(storeId: currentAccount.StoreId, statusId: validStatusIds);
            var color = "green";
            var count = _context.AccountOrder.SearchCount(storeId:currentAccount.StoreId,statusId: new int[] { status.ID });
            return PartialView("StatusCount", new Tuple<string,string,int,int>(status.Name,color,count, countAll));
        }
        [AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]

        public ActionResult ProcessStatusCount()
        {

            var currentAccount = DataLayer.Base.BaseWebsite.CurrentAccount;
            var status = _context.Code.GetByLabel(DataLayer.Enumarables.Enum_Code.ORDER_STATUS_PROCESS);
            var validStatusIds = _context.Code.GetAllByCodeGroup(DataLayer.Enumarables.Enum_CodeGroup.ORDER_STATUS, false, "store").Select(s => s.ID).ToArray();
            var countAll = _context.AccountOrder.SearchCount(storeId: currentAccount.StoreId, statusId: validStatusIds);
            var color = "amber";
            var count = _context.AccountOrder.SearchCount(storeId: currentAccount.StoreId, statusId: new int[] { status.ID });
            return PartialView("StatusCount", new Tuple<string, string, int, int>(status.Name, color, count, countAll));
        }
        public ActionResult PostStatusCount()
        {

            var currentAccount = DataLayer.Base.BaseWebsite.CurrentAccount;
            var status = _context.Code.GetByLabel(DataLayer.Enumarables.Enum_Code.ORDER_STATUS_POST);
            var validStatusIds = _context.Code.GetAllByCodeGroup(DataLayer.Enumarables.Enum_CodeGroup.ORDER_STATUS, false, "store").Select(s => s.ID).ToArray();
            var countAll = _context.AccountOrder.SearchCount(storeId: currentAccount.StoreId, statusId: validStatusIds);
            var color = "blue";
            var count = _context.AccountOrder.Search(storeId: currentAccount.StoreId, statusId: new int[] { status.ID }).Count;
            return PartialView("StatusCount", new Tuple<string, string, int, int>(status.Name, color, count, countAll));
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
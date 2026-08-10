using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.Entities;
using DataLayer.Base;
using DataLayer.Api;
using DataLayer.Enumarables;
using Cms.Areas.Store.Security;
namespace Cms.Areas.Store.Controllers
{
    public class ClearingController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
        [AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]

        public ActionResult Index(

            string from = null,
            string to = null,
            string pageIndex = null,
            string pageSize = null)
        {
         
            var currentAccount = DataLayer.Base.BaseWebsite.CurrentAccount;

            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();
            int size = pageSize == null ? 10 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();
          
            ViewBag.FromDatetime = from;
            ViewBag.ToDatetime = to;
            ViewBag.PageIndex = index.ToString();
            ViewBag.PageSize = size.ToString();


            ViewBag.TotalCount = _context.Clearing.SearchCount(
                storeId: currentAccount.StoreId,
                    fromDatetime: fromDate,
                    toDatetime: toDate
                   );

            List<Clearing> listClearing = _context.Clearing.Search(
                                  storeId: currentAccount.StoreId,
                    fromDatetime: fromDate,
                    toDatetime: toDate,
                    pageSize: size,
                    index: index)
                .ToList();

            return View(listClearing.ToView());
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
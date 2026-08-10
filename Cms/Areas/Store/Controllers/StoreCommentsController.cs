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
using Cms.Areas.Store.Security;
using DataLayer.Enumarables;

namespace Cms.Areas.Store.Controllers
{
    public class StoreCommentsController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        // GET: Store/Products
        [AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]

        public ActionResult Index(int index = 1, int pageSize = 10)
        {
            var currentAccount = DataLayer.Base.BaseWebsite.CurrentAccount;
            var storeComment = _context.StoreComment.Search(storeId: currentAccount.StoreId, index: index, pageSize: pageSize);
            ViewBag.TotalCount = _context.StoreComment.SearchCount(storeId: currentAccount.StoreId, accountId: currentAccount.Id);
            ViewBag.pageSize = pageSize.ToString();
            return View(storeComment.ToView());
        }

        // GET: Store/Products/Details/5 
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!BaseStore.StoreCommentId_IsValid(id.Value, _context))
                return View("Invalid");

            StoreComment storeComment = _context.StoreComment.FirstOrDefault(s => s.ID == id);
            if (storeComment == null)
            {
                return HttpNotFound();
            }
            return View(storeComment);
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

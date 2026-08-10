using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class StoreCommentController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Search(int? scnotId = null, int? scaccountId = null, int? scstoreId = null, int? scindex = null, int? scpageSize = null, string scbody = null, string scviewname = null, Boolean? scapproved = null, int? sccommentid = null)
        {
            List<StoreComment> list = _context.StoreComment.Search(
                notId: scnotId,
                accountId: scaccountId,
                storeId: scstoreId,
                index: scindex,
                pageSize: scpageSize,
                body: scbody,
                approved: scapproved,
                id: sccommentid);
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, scviewname, list);
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
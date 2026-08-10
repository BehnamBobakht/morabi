using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class ProductCommentController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Search(int? pconotId = null, int? pcoaccountId = null, int? pcoproductId = null, int? pcoindex = null, int? pcopageSize = null, string pcobody = null, string pcoviewname = null, Boolean? pcapproved = null,int? pccommentid = null)
        {
            List<ProductComment> list = _context.ProductComment.Search(
                notId: pconotId,
                accountId: pcoaccountId,
                productId: pcoproductId,
                index: pcoindex,
                pageSize: pcopageSize,
                body: pcobody,
                approved: pcapproved,
                id: pccommentid);
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, pcoviewname, list);
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
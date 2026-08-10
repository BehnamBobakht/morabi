using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class MallController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? maId, string maIdName = null)
        {
            Mall mall = _context.Mall.GetById(maId);
            mall.VisitCount = mall.VisitCount + 1;
            _context.Mall.Update(mall);
            _context.Save();
            ViewBag.CurrentAccount = _context.Account.GetCurrentAccount();
            return PartialView(BaseController.GetView(this), mall);
        }

        public ActionResult Search(
            int? maIndex = null,
            int? maPageSize = null,
            int? macityId = null,
            string maname = null,
            string maviewName = null)
        {
            maIndex = maIndex != null ? maIndex.Value : 1;
            maPageSize = maPageSize != null ? maPageSize.Value : 10;
            List<Mall> list = _context.Mall.Search(
                cityId: macityId, 
                name: maname, 
                pageSize: maPageSize.Value, 
                pageIndex: maIndex.Value);
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, maviewName, list);
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
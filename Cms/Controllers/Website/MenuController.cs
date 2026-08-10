using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class MenuController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Search(int? mnnotId = null, int? mnparentId = null, string mnname = null, int? mnindex = null, int? mnpageSize = null, string mnviewName = null)
        {
            mnindex = mnindex != null ? mnindex.Value : 1; 
            mnpageSize = mnpageSize != null ? mnpageSize.Value : 10;
            List<Menu> list = _context.Menu.Search(
                notId: mnnotId, 
                index: mnindex.GetValueOrDefault(), 
                pageSize: mnpageSize.GetValueOrDefault(),
                name: mnname,
                parentId: mnparentId);
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, mnviewName, list);
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
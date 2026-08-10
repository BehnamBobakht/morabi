using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class SendTypeController : Controller
    {
        // GET: Merchant
        UnitOfWork _context = new UnitOfWork();
        public ActionResult GetActiveSendType()
        {
            var activeSendTypes = _context.SendType.GetAll();
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, "SendTypes", activeSendTypes);
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
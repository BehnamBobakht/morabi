using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Report.Controllers
{
    public class WebsiteViewController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ReportByWeekDay()
        {
            List<ViewKeyValueString> list = _context.WebsiteView.GetVisitReportByWeekDays();
            return new JsonResult() {
                Data = list,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
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
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Report.Controllers
{
    public class ReportDashboardController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            ViewBag.StatusId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.ORDER_STATUS, false), "ID", "Name");
            ViewBag.Stores = new SelectList(_context.Store.GetAll(), "ID", "Name");
            ViewBag.Satets = new SelectList(_context.State.Where(s=>s.CountryId==118).ToList(), "ID", "Name");
            return View();
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
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
//using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace Panel.Areas.Content.Controllers
{
    public class ExelController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Banner);
            return View(_context.Banner.GetAll());
        }
        public ActionResult Import()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase exelFile)
        {
            //using (var package = new ExcelPackage(exelFile.InputStream))
            //{
            //    var currentSheet = package.Workbook.Worksheets;
            //    var workSheet = currentSheet.First();
            //    var noOfCol = workSheet.Dimension.End.Column;
            //    var noOfRow = workSheet.Dimension.End.Row;

            //    var spqId = (workSheet.Cells[1, 1].Value ?? "").ToString();

                return View();
            //}
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
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class GalleryController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? gaid, string gaidname = "")
        {
            Gallery gallery = _context.Gallery.GetById(gaid);
            return PartialView(BaseController.GetView(this), gallery);
        }

        public ActionResult Search(
            int? ganotId = null,
            int? gawebsiteId = null,
            bool ? showInHome = null,

            int? gaIndex = 1,
            int? gaSize = 1000,
            string ganame = null,
            string gaviewName = null,
            string gacategory = null)
        {
            List<Gallery> list = _context.Gallery.Search(
                websiteId: gawebsiteId,
                index: gaIndex,
                showInHome:showInHome,
                pageSize: gaSize,
                name: ganame,
                notId: ganotId,
                categoryLabel: gacategory);
            ViewBag.gaIndex = gaIndex;
            ViewBag.gaSize = gaSize;
            ViewBag.gacategory = gacategory;

            ViewBag.total = _context.Gallery.Where(s=>s.Category.Label == gacategory).Count();
            return PartialView(BaseController.GetView(this, gaviewName), list);
            //return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, gaviewName, list);
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
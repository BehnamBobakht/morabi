using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class WebsiteFormController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(string frlabel, string frname)
        {
            WebsiteForm form = _context.WebsiteForm.GetByLabel(frlabel);
            return PartialView(BaseController.GetView(this, frlabel), form);
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
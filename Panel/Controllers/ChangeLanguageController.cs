using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Panel.Controllers
{
    public class ChangeLanguageController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(string id, string back = "")
        {
            _context.WebsiteLanguage.SetCurrentLanguage(id);
            return Redirect(back);
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
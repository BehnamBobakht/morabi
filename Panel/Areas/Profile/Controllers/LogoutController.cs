using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Profile.Controllers
{
    public class LogoutController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.SiteUser.Logout();
            return Redirect("~/profile/login");
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
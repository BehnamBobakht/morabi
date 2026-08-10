using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Panel.Controllers
{
    public class LayoutController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Footer()
        {
            return PartialView();
        }

        public ActionResult PublicHeader()
        {
            return PartialView();
        }

        public ActionResult PublicFooter()
        {
            return PartialView();
        }

        public ActionResult TopBar()
        {
            return PartialView();
        }

        public ActionResult Notification()
        {
            return PartialView();
        }

        public ActionResult TableScript()
        {
            return PartialView();
        }

        public ActionResult Navigation()
        {
            List<string> permissions = _context.Permission.GetCurrentPermission();
            return PartialView(permissions);
        }
        
        public ActionResult UserBox()
        {
            return PartialView(_context.SiteUser.GetCurrentUser());
        }
    }
}
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Panel.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            SiteUser user = _context.SiteUser.GetCurrentUser();
            _context.Permission.CheckPagePermission(user, Enum_Permission.Dashboard);
            if (user != null)
            {
                ViewAdminDashboard dashboard = new ViewAdminDashboard();
                List<Shop> listShop = _context.Shop.GetAllShopForUserId(user.ID);
                ViewBag.Shop = listShop.FirstOrDefault();
                return View(_context.Dashboard.GetAllBySiteUserId(user.ID));
            }
            return null;
        }
    }
}
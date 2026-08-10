using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Setting.Controllers
{
    public class PermissionController : Controller
    {
        private UnitOfWork Context = new UnitOfWork();

        public ActionResult Index()
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.Permission);
            List<ViewPermission> list = Context.Permission.GetAllView();
            return View(list);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
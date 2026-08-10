using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Setting.Controllers
{
    public class ModuleController : Controller
    {
        private UnitOfWork Context = new UnitOfWork();

        public ActionResult Index()
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.Module);
            List<Module> list = Context.Module.GetAll();
            ViewBag.Message = BaseMessage.GetMessage();
            return View(list);
        }
        
        [HttpPost]
        public ActionResult Index(int[] SelectedModules)
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.Module);
            List<Module> list = Context.Module.GetAll();
            if (SelectedModules != null)
            {
                foreach (Module item in list)
                {
                    bool IsSelected = SelectedModules.Any(p => p == item.ID);
                    item.IsSelected = IsSelected;
                    Context.Module.Update(item);
                }
                Context.Save();
            }
            ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.SUCCESS, Enum_Message.SUCCESSFULL_SUBMIT);
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
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Setting.Controllers
{
    public class CacheSettingController : Controller
    {
        private UnitOfWork Context = new UnitOfWork();

        public ActionResult Index()
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.CacheSetting);
            return View(Context.CacheSetting.GetAll());
        }

        public ActionResult Edit(int? id)
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.CacheSetting_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CacheSetting cacheSetting = Context.CacheSetting.GetById(id);
            if (cacheSetting == null)
                return HttpNotFound();
            
            ViewBag.Message = BaseMessage.GetMessage();
            return View(cacheSetting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CacheSetting cacheSetting)
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.EmailHost_Edit);
            if (cacheSetting.Duration>0)
            {
                var cacheSetingEntity = Context.CacheSetting.GetById(cacheSetting.ID);
                cacheSetingEntity.Duration = cacheSetting.Duration;
                cacheSetingEntity.Active = cacheSetting.Active;

                Context.CacheSetting.Update(cacheSetingEntity);
                Context.Save();
                return RedirectToAction("Index");
            }
            ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR,Resource.Notify.DurationMustBeGreaterThanZero);

            return View(cacheSetting);
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
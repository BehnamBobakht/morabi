using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Setting.Controllers
{
    public class SmsSettingController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmsSetting);
            return View(_context.SmsSetting.GetAll().ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmsSetting_Details);
            SmsSetting setting = _context.SmsSetting.GetById(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmsSetting_New);
            FillDropDowns(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SmsSetting setting)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmsSetting_New);

            if (IsModelValid(setting))
            {
                _context.SmsSetting.Insert(setting);
                _context.Save();
                return RedirectToAction("Index");
            }

            FillDropDowns(setting);
            return View(setting);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmsSetting_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SmsSetting setting = _context.SmsSetting.GetById(id);
            if (setting == null)
                return HttpNotFound();

            FillDropDowns(setting);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SmsSetting setting)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmsSetting_Edit);
            if (IsModelValid(setting))
            {
                _context.SmsSetting.Update(setting);
                _context.Save();
                return RedirectToAction("Index");
            }
            FillDropDowns(setting);
            return View(setting);
        }

        private bool IsModelValid(SmsSetting setting)
        {
            bool result = false;
            if (setting.NumberId == 0)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_SMS_NUMBER);
            else if (setting.SmsTypeId == 0)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_SMS_TYPE);
            else
                result = true;
            return result;
        }

        private void FillDropDowns(SmsSetting setting)
        {
            ViewBag.SmsTypeId = new SelectList(_context.SmsType.GetAll(), "ID", "Name", setting != null ? setting.SmsTypeId : 0);
            ViewBag.NumberId = new SelectList(_context.SmsNumber.GetAll(), "ID", "Number", setting != null ? setting.NumberId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmsSetting_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SmsSetting setting = _context.SmsSetting.GetById(id);
            if (setting == null)
                return HttpNotFound();

            return View(setting);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmsSetting_Delete);
            SmsSetting setting = _context.SmsSetting.GetById(id);
            _context.SmsSetting.Delete(setting);
            _context.Save();
            return RedirectToAction("Index");
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
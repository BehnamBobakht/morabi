using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Store.Controllers
{
    [ValidateInput(false)]
    public class SmsController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(
            int? index = null,
            int? pagesize = null,
            string name = null
           )
        {

            index = index == null ? 1 : index;
            pagesize = pagesize == null ? 10 : pagesize;
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SendSMS);
            List<Sms> list = _context.Sms.Search(index: index.GetValueOrDefault(), pageSize: pagesize.GetValueOrDefault(), name: name);
            ViewBag.TotalCount = _context.Sms.SearchCount(name: name);
            return View(list);
        }
        private void FillDropDowns(SmsSetting setting)
        {
            ViewBag.TypeId = new SelectList(_context.SmsType.GetAll(), "ID", "Name", setting != null ? setting.SmsTypeId : 0);
            ViewBag.Sender = new SelectList(_context.SmsNumber.GetAll(), "ID", "Number", setting != null ? setting.NumberId : 0);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SendSMS_Details);
            Sms sms = _context.Sms.GetById(id);
            if (sms == null)
            {
                return HttpNotFound();
            }
            return View(sms);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SendSMS_New);
            Sms sms = new Sms();
            FillDropDowns(null);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(sms);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sms sms)
        {
          
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SendSMS_New);

            sms.IsSend = false;
            int providerId = int.Parse(sms.Sender);
           
            string number = _context.SmsNumber.FirstOrDefault(s=>s.ID== providerId).Number.ToString();
            sms.Sender = number;
            _context.Sms.Insert(sms);
            _context.Save();
            FillDropDowns(null);
            return RedirectToAction("Index");
        }
       
        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SendSMS_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Sms sms = _context.Sms.GetById(id);
            if (sms == null)
                return HttpNotFound();
            SmsSetting setting = _context.SmsSetting.Where(s=>s.SmsTypeId==sms.TypeId).FirstOrDefault();
            //if (setting == null)
            //    return HttpNotFound();

            FillDropDowns(setting);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(sms);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Sms competitive)
        //{
        //    _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.CompetitiveFeature_Edit);
        //    if (IsModelValid(competitive))
        //    {
        //        _context.CompetitiveFeature.Update(competitive);
        //        _context.Save();
        //        return RedirectToAction("Index");
        //    }

        //    return View(competitive);
        //}

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SendSMS_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Sms sms = _context.Sms.GetById(id);
            if (sms == null)
                return HttpNotFound();

            return View(sms);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SendSMS_Delete);
            Sms sms = _context.Sms.GetById(id);
            try
            {
                _context.CompetitiveFeature.Delete(sms);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(sms);
            }
           
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
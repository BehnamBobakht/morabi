using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Content.Controllers
{
    public class WebsiteFormController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteForm);
            return View(_context.WebsiteForm.GetAll());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteForm_Details);
            WebsiteForm form = _context.WebsiteForm.GetById(id);
            if (form == null)
            {
                return HttpNotFound();
            }
            return View(form);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteForm_New);
            FillDropDowns(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WebsiteForm form)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteForm_New);

            if (IsModelValid(form))
            {
                _context.WebsiteForm.Insert(form);
                _context.Save();
                return RedirectToAction("Index");
            }

            FillDropDowns(form);
            return View(form);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteForm_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            WebsiteForm form = _context.WebsiteForm.GetById(id);
            if (form == null)
                return HttpNotFound();

            FillDropDowns(form);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(form);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WebsiteForm form)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteForm_Edit);
            if (IsModelValid(form))
            {
                _context.WebsiteForm.Update(form);
                _context.Save();
                return RedirectToAction("Index");
            }
            FillDropDowns(form);
            return View(form);
        }

        private bool IsModelValid(WebsiteForm form)
        {
            bool result = false;
            if (form.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_WEBSITEFORM_NAME);
            else if (form.Label == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_WEBSITEFORM_LABEL);
            else
                result = true;
            return result;
        }

        private void FillDropDowns(WebsiteForm form)
        {
            
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteForm_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            WebsiteForm form = _context.WebsiteForm.GetById(id);
            if (form == null)
                return HttpNotFound();

            return View(form);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteForm_Delete);
            WebsiteForm form = _context.WebsiteForm.GetById(id);
            try
            {
                _context.WebsiteForm.Delete(form);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(form);
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
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Setting.Controllers
{
    public class LanguageController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Language);
            return View(_context.Language.GetAll().ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Language_Details);
            Language language = _context.Language.GetById(id);
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        private void FillDropDowns(Language language)
        {
            ViewBag.DirectionId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.LANGUAGE_DIRECTION), "ID", "Name", language != null ? language.DirectionId : 0);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Language_New);
            FillDropDowns(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Language language)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Language_New);

            if (IsModelValid(language))
            {
                _context.Language.Insert(language);
                _context.Save();
                return RedirectToAction("Index");
            }
            FillDropDowns(language);
            return View(language);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Language_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Language language = _context.Language.GetById(id);
            if (language == null)
                return HttpNotFound();

            FillDropDowns(language);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(language);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Language language)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Language_Edit);
            if (IsModelValid(language))
            {
                _context.Language.Update(language);
                _context.Save();
                return RedirectToAction("Index");
            }
            FillDropDowns(language);
            return View(language);
        }

        private bool IsModelValid(Language language)
        {
            bool result = false;

            if (language.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_LANGUAGE_NAME);
            else if (language.Label == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_LANGUAGE_LABEL);
            else if (language.Culture == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_LANGUAGE_CULTURE);
            else
                result = true;
            return result;
        }
        
        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Language_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Language language = _context.Language.GetById(id);
            if (language == null)
                return HttpNotFound();

            return View(language);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Language_Delete);
            Language language = _context.Language.GetById(id);
            try
            {
                _context.Language.Delete(language);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(language);
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
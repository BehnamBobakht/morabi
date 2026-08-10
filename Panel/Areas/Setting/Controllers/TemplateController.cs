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
    public class TemplateController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Template);
            return View(_context.Template.GetAll().ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Template_Details);
            Template template = _context.Template.GetById(id);
            if (template == null)
            {
                return HttpNotFound();
            }
            return View(template);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Template_New);
            FillDropDowns(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Template template, HttpPostedFileBase file)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Template_New);

            if (IsModelValid(template, file))
            {
                _context.Template.Insert(template);
                _context.Save();
                return RedirectToAction("Index");
            }

            FillDropDowns(template);
            return View(template);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Template_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Template template = _context.Template.GetById(id);
            if (template == null)
                return HttpNotFound();

            FillDropDowns(template);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(template);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Template template, HttpPostedFileBase file)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Template_Edit);
            if (IsModelValid(template, file))
            {
                _context.Template.Update(template);
                _context.Save();
                return RedirectToAction("Index");
            }
            FillDropDowns(template);
            return View(template);
        }

        private bool IsModelValid(Template template, HttpPostedFileBase file)
        {
            bool result = false;

            if (template.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_TEMPLATE_NAME);
            else if (template.Label == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_TEMPLATE_LABEL);
            else
                result = true;
            
            if (file != null)
            {
                template.PictureId = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file).ID;
            }

            return result;
        }

        private void FillDropDowns(Template template)
        {
            ViewBag.TypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.TEMPLATE_TYPE), "ID", "Name", template != null ? template.TypeId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Template_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Template template = _context.Template.GetById(id);
            if (template == null)
                return HttpNotFound();

            return View(template);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Template_Delete);
            Template template = _context.Template.GetById(id);
            try
            {
                _context.Template.Delete(template);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(template);
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
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
    [ValidateInput(false)]

    public class StaticPageController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StaticPage);
            return View(_context.StaticPage.GetAll());
        }


        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StaticPage_New);
            FillDropDowns(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,CategoryId,Html")] StaticPage staticPage, HttpPostedFileBase file)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StaticPage_New);

            if (IsModelValid(staticPage, file))
            {
                _context.StaticPage.Insert(staticPage);
                _context.Save();
                return RedirectToAction("Index");
            }

            FillDropDowns(staticPage);
            return View(staticPage);
        }

        public ActionResult Edit(int? id)
        {
            //_context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StaticPage_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            StaticPage staticPage = _context.StaticPage.GetById(id);
            if (staticPage == null)
                return HttpNotFound();

            FillDropDowns(staticPage);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(staticPage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,CategoryId,Html")]StaticPage staticPage, HttpPostedFileBase file)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StaticPage_Edit);
            if (IsModelValid(staticPage, file))
            {
                _context.StaticPage.Update(staticPage);
                _context.Save();
                return RedirectToAction("Index");
            }
            FillDropDowns(staticPage);
            return View(staticPage);
        }

        private bool IsModelValid(StaticPage staticPage, HttpPostedFileBase file)
        {
            bool result = true;

            if (file != null)
            {
                Picture picture = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file);
                staticPage.PictureId = picture.ID;
            }

            return result;
        }

        private void FillDropDowns(StaticPage staticPage)
        {
            ViewBag.CategoryId = new SelectList(_context.Category.GetAllByType(Enum_Code.CATEGORY_TYPE_CONTENT), "ID", "Name", staticPage != null ? staticPage.CategoryId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StaticPage_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            StaticPage staticPage = _context.StaticPage.GetById(id);
            if (staticPage == null)
                return HttpNotFound();

            return View(staticPage);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StaticPage_Delete);
            StaticPage staticPage = _context.StaticPage.GetById(id);
            _context.StaticPage.Delete(staticPage);
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
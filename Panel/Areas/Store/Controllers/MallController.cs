using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Store.Controllers
{
    public class MallController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Mall);
            return View(_context.Mall.GetAll());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Mall_Details);
            Mall mall = _context.Mall.GetById(id);
            if (mall == null)
            {
                return HttpNotFound();
            }
            return View(mall);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Mall_New);
            FillDropDowns(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,CityId,StartBuildingDate,EndBuildingDate,OpeningDate,Owner,Area,ArchitecturalCompany,Address,Description,VisitCount")] Mall mall, HttpPostedFileBase file)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Mall_New);

            if (IsModelValid(mall, file))
            {
                _context.Mall.Insert(mall);
                _context.Save();
                return RedirectToAction("Index");
            }

            FillDropDowns(mall);
            return View(mall);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Mall_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Mall mall = _context.Mall.GetById(id);
            if (mall == null)
                return HttpNotFound();

            FillDropDowns(mall);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(mall);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,PictureId,CityId,StartBuildingDate,EndBuildingDate,OpeningDate,Owner,Area,ArchitecturalCompany,Address,Description,VisitCount")] Mall mall, HttpPostedFileBase file)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Mall_Edit);
            if (IsModelValid(mall, file))
            {
                _context.Mall.Update(mall);
                _context.Save();
                return RedirectToAction("Index");
            }
            FillDropDowns(mall);
            return View(mall);
        }

        private bool IsModelValid(Mall mall, HttpPostedFileBase file)
        {
            bool result = false;
            if (mall.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_MALL_NAME);
            else
                result = true;

            if (file != null)
            {
                Picture picture = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file);
                mall.PictureId = picture.ID;
            }

            return result;
        }

        private void FillDropDowns(Mall mall)
        {
            ViewBag.CityId = new SelectList(_context.City.GetAll(), "ID", "Name", mall != null ? mall.CityId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Mall_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Mall mall = _context.Mall.GetById(id);
            if (mall == null)
                return HttpNotFound();

            return View(mall);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Mall_Delete);
            Mall mall = _context.Mall.GetById(id);
            _context.Mall.Delete(mall);
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
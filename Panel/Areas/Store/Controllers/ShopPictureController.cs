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
    public class ShopPictureController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_Edit);
            ViewBag.Shop = _context.Shop.GetById(id.GetValueOrDefault());
            ViewBag.ShopId = id;
            return View(_context.ShopPicture.GetAllByShopId(id.Value));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_Edit);
            ShopPicture picture = _context.ShopPicture.GetById(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShopId = picture.ShopId;
            return View(picture);
        }

        public ActionResult Create(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_Edit);
            ViewBag.ShopId = id;
            ShopPicture model = new ShopPicture() {
                ShopId = id.Value
            };
            ViewBag.Message = BaseMessage.GetMessage();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ShopId,PictureId")] ShopPicture picture, HttpPostedFileBase file)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_Edit);

            if (IsModelValid(picture, file))
            {
                _context.ShopPicture.Insert(picture);
                _context.Save();
                return RedirectToAction("Index", new { @id = picture.ShopId });
            }
            ViewBag.ShopId = picture.ShopId;
            return View(picture);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ShopPicture picture = _context.ShopPicture.GetById(id);
            if (picture == null)
                return HttpNotFound();
            
            ViewBag.Message = BaseMessage.GetMessage();
            ViewBag.ShopId = picture.ShopId;
            return View(picture);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ShopId,PictureId")] ShopPicture picture, HttpPostedFileBase file)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_Edit);
            if (IsModelValid(picture, file))
            {
                _context.ShopPicture.Update(picture);
                _context.Save();
                return RedirectToAction("Index", new { @id = picture.ShopId });
            }
            ViewBag.ShopId = picture.ShopId;
            return View(picture);
        }

        private bool IsModelValid(ShopPicture picture, HttpPostedFileBase file)
        {
            bool result = true;
            if (file != null)
            {
                picture.PictureId = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file).ID;
                result = true;
            }
            return result;
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ShopPicture picture = _context.ShopPicture.GetById(id);
            if (picture == null)
                return HttpNotFound();
            ViewBag.ShopId = picture.ShopId;
            return View(picture);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_Edit);
            ShopPicture picture = _context.ShopPicture.GetById(id);
            _context.ShopPicture.Delete(picture);
            _context.Save();
            return RedirectToAction("Index", new { @id = picture.ShopId });
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
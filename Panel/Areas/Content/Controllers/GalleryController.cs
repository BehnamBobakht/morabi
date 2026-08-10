using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.ViewModels;

namespace Panel.Areas.Content.Controllers
{
    public class GalleryController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Gallery);
            return View(_context.Gallery.GetAll().ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Gallery_Details);
            Gallery gallery = _context.Gallery.GetById(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }
        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Gallery_New);
            Gallery gallery = new Gallery()
            {
                Active = true
            };
            FillDropDowns(null);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(gallery);
        }

        [HttpPost]
        public ActionResult Create(Gallery gallery)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Gallery_New);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            List<GalleryLanguage> listLanguage = gallery.GalleryLanguage.ToList();
            gallery.GalleryLanguage.Clear();
            if (IsModelValid(gallery))
            {
                gallery.CreateDateTime = DateTime.Now;
                gallery.UpdateDateTime = DateTime.Now;
                _context.Gallery.Insert(gallery);
                bool result = _context.Save();
                if (result == true)
                {
                    foreach (GalleryLanguage item in listLanguage)
                    {
                        item.GalleryId = gallery.ID;

                        _context.GalleryLanguage.Insert(item);
                    }
                    _context.Save();
                }
               
            }
            return new JsonResult() { Data = error };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Gallery_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Gallery gallery = _context.Gallery.GetById(id);
            if (gallery == null)
                return HttpNotFound();

            FillDropDowns(gallery);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(gallery);
        }

        [HttpPost]

        public ActionResult Edit(Gallery gallery)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Gallery_Edit);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            List<GalleryLanguage> listLanguage = gallery.GalleryLanguage.ToList();
            gallery.GalleryLanguage.Clear();
            _context.GalleryLanguage.DeleteByGalleryId(gallery.ID);


            if (IsModelValid(gallery))
            {
                gallery.ShowDateTime = DateTime.Now;
                gallery.UpdateDateTime = DateTime.Now;
                //gallery.CreateDateTime = DateTime.Now;
                _context.Gallery.Update(gallery);
                _context.Save();

                foreach (GalleryLanguage item in listLanguage)
                {
                    item.GalleryId = gallery.ID;
                    _context.GalleryLanguage.Insert(item);
                }
                _context.Save();
            }
            return new JsonResult() { Data = error };

        }

        private bool IsModelValid(Gallery gallery)
        {
            bool result = false;
            if (gallery.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_GALLERY_NAME);
            else
                result = true;
            return result;
        }

        private void FillDropDowns(Gallery gallery)
        {
            ViewBag.CategoryId = new SelectList(_context.Category.GetAllByType(Enum_Code.CATEGORY_TYPE_GALLERY), "ID", "Name", gallery != null ? gallery.CategoryId : 0);

            ViewBag.WebsiteId = new SelectList(_context.Website.GetAllByType(Enum_Code.SYSTEM_TYPE_CMS, Enum_Code.SYSTEM_TYPE_SHOP), "ID", "Title", gallery != null ? gallery.WebsiteId : 0);
        }


        [HttpGet]
        public ActionResult Picture(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Gallery_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Gallery gallery = _context.Gallery.GetById(id);
            if (gallery == null)
                return HttpNotFound();
            return View(gallery);
        }

        [HttpPost]
        public ActionResult Picture([Bind(Include = "ID")] Gallery gallery, HttpPostedFileBase file, string name = "",string nameEn="")
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Gallery_Edit);

            if (file != null)
            {
                Picture picture = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file);
                GalleryPictures pic = new GalleryPictures()
                {
                    PictureId = picture.ID,
                    GalleryId = gallery.ID,
                    Name = name,
                    NameEn=nameEn
                };
                _context.GalleryPicture.Insert(pic);
                _context.Save();
            }
            gallery = _context.Gallery.GetById(gallery.ID);
            return View(gallery);
        }

        [HttpPost]
        public ActionResult PictureDelete(int GalleryId, int PictureId)
        {
            _context.GalleryPicture.Delete(PictureId);
            _context.Save();
            return RedirectToAction("Picture", "Gallery", new { id = GalleryId });
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Gallery_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Gallery gallery = _context.Gallery.GetById(id);
            if (gallery == null)
                return HttpNotFound();

            return View(gallery);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Gallery_Delete);
            Gallery gallery = _context.Gallery.GetById(id);
            //---
            List<GalleryLanguage> galleryLang = _context.GalleryLanguage.Where(x => x.GalleryId == gallery.ID).ToList();

            if (galleryLang != null && galleryLang.Any())
            {
                foreach (var item in galleryLang)
                {
                    _context.GalleryLanguage.Delete(item);
                }
            }
            //----
            _context.Gallery.Delete(gallery);
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
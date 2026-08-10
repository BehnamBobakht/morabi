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

namespace Panel.Areas.Content.Controllers
{
    public class BannerController : Controller
    {
        private UnitOfWork _context = new UnitOfWork(); 

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Banner);
            return View(_context.Banner.GetAll());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Banner_Details);
            Banner banner = _context.Banner.GetById(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Banner_New);
            FillDropDowns(null);

            return View(new Banner());
        }

        [HttpPost]
        public ActionResult Create(Banner banner)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Banner_New);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };

            List<BannerLanguage> listLanguage = banner.BannerLanguage.ToList();
            banner.BannerLanguage.Clear();
            if (IsModelValid(banner))
            {
                banner.ClickCount = 0;
                _context.Banner.Insert(banner);
                bool result = _context.Save();
                if (result == true)
                {
                    foreach (BannerLanguage item in listLanguage)
                    {
                        item.BannerId = banner.ID;
                        _context.BannerLanguage.Insert(item);
                    }
                    _context.Save();
                }
            }

            return new JsonResult() { Data = error };

        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Banner_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Banner banner = _context.Banner.GetById(id);
            if (banner == null)
                return HttpNotFound();

            FillDropDowns(banner);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(banner);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Banner banner)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Banner_Edit);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            List<BannerLanguage> listLanguage = banner.BannerLanguage.ToList();
            banner.BannerLanguage.Clear();
            _context.BannerLanguage.DeleteByBannerId(banner.ID);
            if (IsModelValid(banner))
            {
                _context.Banner.Update(banner);
                _context.Save();
                foreach (BannerLanguage item in listLanguage)
                {
                    item.BannerId = banner.ID;
                    _context.BannerLanguage.Insert(item);
                }
                _context.Save();
            }
            return new JsonResult() { Data = error };

        }

        private bool IsModelValid(Banner banner)
        {
            bool result = false;
            if (banner.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_BANNER_NAME);
            else
                result = true;


            return result;
        }

        private void FillDropDowns(Banner banner)
        {
            ViewBag.CategoryId = new SelectList(_context.Category.GetAllByType(Enum_Code.CATEGORY_TYPE_BANNER), "ID", "Name", banner != null ? banner.CategoryId : 0);
            ViewBag.CategoryPicture = (banner != null && banner.Category != null && banner.Category.PictureId.HasValue)  ? banner.Category.Picture.GetUrl() : "";
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Banner_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Banner banner = _context.Banner.GetById(id);
            if (banner == null)
                return HttpNotFound();

            return View(banner);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Banner_Delete);
            Banner banner = _context.Banner.GetById(id);
            try
            {
                //---
                List<BannerLanguage> langList = _context.BannerLanguage.Where(x => x.BannerId == banner.ID).ToList();

                if (langList != null && langList.Any())
                {
                    foreach (var item in langList)
                    {
                        _context.BannerLanguage.Delete(item);
                    }
                }
                //----
                _context.Banner.Delete(banner);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(banner);
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
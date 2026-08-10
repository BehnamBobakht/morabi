using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Setting.Controllers
{
    public class WebsiteController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Website);
            return View(_context.Website.GetAll().ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Website_Details);
            Website website = _context.Website.GetById(id);
            if (website == null)
            {
                return HttpNotFound();
            }
            return View(website);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Website_New);
            FillDropDowns(null);
            ViewBag.Message = BaseMessage.GetMessage();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Website website)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Website_New);

            if (IsModelValid(website))
            {
                _context.Website.Insert(website);
                _context.Save();
                return RedirectToAction("Index");
            }

            FillDropDowns(website);
            return View(website);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Website_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Website website = _context.Website.GetById(id);
            if (website == null)
                return HttpNotFound();

            FillDropDowns(website);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(website);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Website website, HttpPostedFileBase file)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Website_Edit);
            if (IsModelValid(website))
            {
                _context.Website.Update(website);
                _context.Save();
                return RedirectToAction("Index");
            }
            FillDropDowns(website);
            return View(website);
        }

        [HttpGet]
        public ActionResult Template(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Website_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Website website = _context.Website.GetById(id);
            if (website == null)
                return HttpNotFound();

            ViewBag.Templates = _context.Template.GetAll();
            ViewBag.Message = BaseMessage.GetMessage();
            return View(website);
        }

        [HttpPost]
        public ActionResult Template(int WebsiteId, int? TemplateId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Website_Edit);

            Website website = _context.Website.GetById(WebsiteId);
            website.TemplateId = TemplateId;
            _context.Website.Update(website);
            _context.Save();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Language(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Website_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Website website = _context.Website.GetById(id);
            if (website == null)
                return HttpNotFound();

            ViewBag.Website = website;
            ViewBag.Message = BaseMessage.GetMessage();
            return View(_context.Language.GetAll());
        }

        [HttpPost]
        public ActionResult Language(int WebsiteId, int[] LanguageId)
        {
            List<WebsiteLanguage> list = _context.WebsiteLanguage.GetAllByWebsiteId(WebsiteId);
            foreach (WebsiteLanguage item in list)
            {
                _context.WebsiteLanguage.Delete(item);
            }
            _context.Save();
            foreach (int item in LanguageId)
            {
                WebsiteLanguage entity = new WebsiteLanguage()
                {
                    LanguageId = item,
                    WebsiteId = WebsiteId
                };
                _context.WebsiteLanguage.Insert(entity);
            }
            _context.Save();
            return RedirectToAction("Index");
        }

        public ActionResult ShopSetting(int? websiteId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Website_Edit);
            if (websiteId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Website website = _context.Website.GetById(websiteId);
            if (website == null)
                return HttpNotFound();
            ShopWebsiteSetting setting = _context.ShopWebsiteSetting.GetSettingByWebsiteId(website.ID);

            ViewBag.Website = website;
            if (setting == null)
            {
                setting = _context.ShopWebsiteSetting.GetDefaultValues();
                setting.WebsiteId = website.ID;
                _context.ShopWebsiteSetting.Insert(setting);
                _context.Save();
            }
            ViewBag.Message = BaseMessage.GetMessage();
            return View(setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShopSetting(ShopWebsiteSetting setting)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Website_Edit);

            _context.ShopWebsiteSetting.Update(setting);
            _context.Save();
            return RedirectToAction("Index");
        }

        private bool IsModelValid(Website website)
        {
            bool result = false;

            if (website.Title == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_WEBSITE_TITLE);
            else
                result = true;

            return result;
        }

        private void FillDropDowns(Website website)
        {
            ViewBag.TypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.SYSTEM_TYPE), "ID", "Name", website != null ? website.TypeId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Website_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Website website = _context.Website.GetById(id);
            if (website == null)
                return HttpNotFound();

            return View(website);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Website_Delete);
            Website website = _context.Website.GetById(id);
            try
            {
                List<WebsiteDomain> listDomains = website.WebsiteDomain.ToList();
                foreach (WebsiteDomain item in listDomains)
                {
                    _context.WebsiteDomain.Delete(item);
                }

                List<WebsiteLanguage> listLanguages = website.WebsiteLanguage.ToList();
                foreach (WebsiteLanguage item in listLanguages)
                {
                    _context.WebsiteLanguage.Delete(item);
                }
                _context.Website.Delete(website);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(website);
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
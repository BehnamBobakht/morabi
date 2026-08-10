using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Setting.Controllers
{
    public class DomainController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();
        
        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.WebsiteId = id;
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteDomain);
            return View(_context.WebsiteDomain.GetAllByWebsiteId(id.Value).ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteDomain_Details);
            WebsiteDomain domain = _context.WebsiteDomain.GetById(id);
            if (domain == null)
            {
                return HttpNotFound();
            }
            ViewBag.WebsiteId = domain.WebsiteId;
            return View(domain);
        }

        public ActionResult Create(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteDomain_New);
            
            Website website = _context.Website.GetById(id.Value);
            WebsiteDomain domain = new WebsiteDomain() {
                Website = website,
                WebsiteId = id.Value,
                ActivationKey = Guid.NewGuid().ToString(),
            };
            
            ViewBag.WebsiteId = domain.WebsiteId;
            ViewBag.Message = BaseMessage.GetMessage();
            FillDropDowns(new WebsiteDomain() { WebsiteId = id.GetValueOrDefault() });
            return View(domain);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int WebsiteId, string Domain, string ActivationKey, bool IsShortUrl, int LanguageId, string RedirectUrl, string PostUrl)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteDomain_New);

            WebsiteDomain domain = new WebsiteDomain()
            {
                Domain = Domain,
                WebsiteId = WebsiteId,
                IsShortUrl = IsShortUrl,
                LanguageId = LanguageId,
                RedirectUrl = RedirectUrl,
                ActivationKey = ActivationKey,
                PostUrl =PostUrl ,

            };

            if (IsModelValid(domain))
            {
                _context.WebsiteDomain.Insert(domain);
                _context.Save();
                return RedirectToAction("Index", new { @id = domain.WebsiteId });
            }

            ViewBag.WebsiteId = domain.WebsiteId;
            return View(domain);
        }

        private void FillDropDowns(WebsiteDomain domain)
        {
            List<Language> listLanguage = new List<Language>() {
                new Language() { ID = 0, Name = "انتخاب" }
            };
            foreach (WebsiteLanguage websiteLanguage in _context.WebsiteLanguage.GetAllByWebsiteId(domain.WebsiteId))
            {
                listLanguage.Add(websiteLanguage.Language);
            }
            ViewBag.LanguageId = new SelectList(listLanguage, "ID", "Name", domain != null && domain.LanguageId != null ? domain.LanguageId : 0);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteDomain_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            WebsiteDomain domain = _context.WebsiteDomain.GetById(id);
            if (domain == null)
                return HttpNotFound();

            FillDropDowns(domain);

            ViewBag.WebsiteId = domain.WebsiteId;
            ViewBag.Message = BaseMessage.GetMessage();
            return View(domain);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int ID, int WebsiteId, string Domain, string ActivationKey, bool IsShortUrl, int LanguageId, string RedirectUrl, string PostUrl)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteDomain_Edit);

            WebsiteDomain domain = new WebsiteDomain() {
                ID = ID,
                Domain = Domain,
                WebsiteId = WebsiteId,                
                IsShortUrl = IsShortUrl,
                LanguageId = LanguageId,
                RedirectUrl = RedirectUrl,
                ActivationKey = ActivationKey,
                PostUrl = PostUrl,

            };

            if (IsModelValid(domain))
            {
                _context.WebsiteDomain.Update(domain);
                _context.Save();
                return RedirectToAction("Index", new { @id = domain.WebsiteId });
            }
            ViewBag.WebsiteId = domain.WebsiteId;
            return View(domain);
        }

        private bool IsModelValid(WebsiteDomain domain)
        {
            bool result = false;
            if (domain.Domain == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_DOMAIN);
            else
                result = true;
            domain.LanguageId = domain.LanguageId == 0 ? default(int?) : domain.LanguageId;
            return result;
        }
        
        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteDomain_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            WebsiteDomain domain = _context.WebsiteDomain.GetById(id);
            if (domain == null)
                return HttpNotFound();

            ViewBag.WebsiteId = domain.WebsiteId;
            return View(domain);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteDomain_Delete);
            WebsiteDomain domain = _context.WebsiteDomain.GetById(id);
            try
            {
                _context.WebsiteDomain.Delete(domain);
                _context.Save();
                return RedirectToAction("Index", new { @id = domain.WebsiteId });
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(domain);
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
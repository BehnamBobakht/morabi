using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Crm.Controllers
{
    public class NewsletterGroupController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup);
            return View(_context.NewsletterGroup.GetAll().ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup_Details);
            NewsletterGroup group = _context.NewsletterGroup.GetById(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup_New);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewsletterGroup group)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup_New);

            if (IsModelValid(group))
            {
                _context.NewsletterGroup.Insert(group);
                _context.Save();
                return RedirectToAction("Index");
            }
            
            return View(group);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            NewsletterGroup group = _context.NewsletterGroup.GetById(id);
            if (group == null)
                return HttpNotFound();
            
            ViewBag.Message = BaseMessage.GetMessage();
            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewsletterGroup group)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup_Edit);
            if (IsModelValid(group))
            {
                _context.NewsletterGroup.Update(group);
                _context.Save();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        public ActionResult Sms(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup_Sms);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            NewsletterGroup group = _context.NewsletterGroup.GetById(id);
            if (group == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sms(int ID, string Body)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup_Sms);
            NewsletterGroup group = _context.NewsletterGroup.GetById(ID);
            if (group == null)
                return HttpNotFound();
            if (Body == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_SMS_BODY);
            else
            {
                foreach (NewsletterItem item in group.NewsletterItem)
                {
                    Newsletter entity = item.Newsletter;
                    if (entity.Mobile != null && entity.Mobile != "")
                    {
                        _context.Sms.SaveNewSms(entity.Mobile, Enum_SmsType.NEWSLETTER, Body);
                    }
                    _context.Save();
                }

                ViewBag.Body = Body;
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.SUCCESS, Enum_Message.SUCCESSFULL_NEWSLETTER_SENT);
            }
            return View(group);
        }

        private bool IsModelValid(NewsletterGroup group)
        {
            bool result = false;
            if (group.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_NEWSLETTERGROUP_NAME);
            else if (group.Label == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_NEWSLETTERGROUP_LABEL);
            else
                result = true;
            return result;
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            NewsletterGroup group = _context.NewsletterGroup.GetById(id);
            if (group == null)
                return HttpNotFound();

            return View(group);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup_Delete);
            NewsletterGroup group = _context.NewsletterGroup.GetById(id);
            try
            {
                _context.NewsletterGroup.Delete(group);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(group);
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
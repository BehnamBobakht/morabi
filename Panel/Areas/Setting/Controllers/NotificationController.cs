using DataLayer.Api;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Helpers.Notification;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Setting.Controllers
{
    public class NotificationController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NotificationProject);
            return View(_context.NotificationProject.GetAll().ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NotificationProject_Details);
            NotificationProject notify = _context.NotificationProject.GetById(id);
            if (notify == null)
            {
                return HttpNotFound();
            }
            return View(notify);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NotificationProject_New);
            FillDropDowns(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NotificationProject notify)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NotificationProject_New);

            if (IsModelValid(notify))
            {
                _context.NotificationProject.Insert(notify);
                _context.Save();

                return RedirectToAction("Index");
            }

            FillDropDowns(notify);
            return View(notify);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NotificationProject_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            NotificationProject notify = _context.NotificationProject.GetById(id);
            if (notify == null)
                return HttpNotFound();

            FillDropDowns(notify);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(notify);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NotificationProject notify)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NotificationProject_Edit);
            if (IsModelValid(notify))
            {
                _context.NotificationProject.Update(notify);
                _context.Save();
                
                FillProjectApp(notify.ID);

                return RedirectToAction("Index");
            }
            FillDropDowns(notify);
            return View(notify);
        }

        private bool IsModelValid(NotificationProject notify)
        {
            return true;
        }

        private void FillProjectApp(int id)
        {
            NotificationProject notify = _context.NotificationProject.GetById(id);
            List<ApiHeaderParameter> listHeader = new List<ApiHeaderParameter>() {
                new ApiHeaderParameter() {
                    Name = "Authorization",
                    Value = "Basic " + notify.AuthorizationValue
                }
            };
            ApiResult result = ApiRequest.CreateApiRequest<List<OneSignalApp>>("https://onesignal.com/api/v1/apps", Enum_RequestMethod.GET, Enum_Api.NONE, listHeader);
            List<OneSignalApp> list = (List<OneSignalApp>)result.Value;
            OneSignalApp app = list.FirstOrDefault(p => p.name == notify.ProjectName);
            foreach (NotificationApp item in notify.NotificationApp)
            {
                _context.NotificationApp.Delete(item);
            }
            _context.Save();
            if (app != null)
            {
                NotificationApp entity = new NotificationApp()
                {
                    ApnsCertificates = app.apns_certificates,
                    ApnsEnv = app.apns_env,
                    BasicAuthKey = app.basic_auth_key,
                    ChromeKey = app.chrome_key,
                    ChromeWebGcmSenderId = app.chrome_web_gcm_sender_id,
                    ChromeWebIcon = app.chrome_web_default_notification_icon,
                    ChromeWebKey = app.chrome_key,
                    ChromeWebOrigin = app.chrome_web_origin,
                    ChromeWebSubdomain = app.chrome_web_sub_domain,
                    CreateDatetime = app.created_at,
                    GcmKey = app.gcm_key,
                    MessageablePlayers = app.messageable_players,
                    Name = app.name,
                    Players = app.players,
                    ProjectId = notify.ID,
                    SenderId = app.id,
                    SiteName = app.site_name,
                    UpdateDatetime = app.updated_at
                };
                _context.NotificationApp.Insert(entity);
                _context.Save();
            }
            
        }

        private void FillDropDowns(NotificationProject notify)
        {
            ViewBag.TypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.NOTIFICATION_SENDER), "ID", "Name", notify != null ? notify.TypeId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NotificationProject_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            NotificationProject notify = _context.NotificationProject.GetById(id);
            if (notify == null)
                return HttpNotFound();

            return View(notify);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NotificationProject_Delete);
            NotificationProject notify = _context.NotificationProject.GetById(id);
            _context.NotificationProject.Delete(notify);
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
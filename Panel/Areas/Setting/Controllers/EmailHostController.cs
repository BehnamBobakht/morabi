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
    public class EmailHostController : Controller
    {
        private UnitOfWork Context = new UnitOfWork();

        public ActionResult Index()
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.EmailHost);
            return View(Context.EmailHost.GetAllView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.EmailHost_Details);
            EmailHost emailHost = Context.EmailHost.GetById(id);
            if (emailHost == null)
            {
                return HttpNotFound();
            }
            return View(emailHost);
        }

        public ActionResult Create()
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.EmailHost_New);

            EmailHost entity = new EmailHost() {
                Active = true,
                AutoSync = true,
                CreateDatetime = DateTime.Now,
                UserCreatorId = Context.SiteUser.GetCurrentUser().ID
            };

            ViewBag.Message = BaseMessage.GetMessage();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmailHost emailHost)
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.EmailHost_New);

            if (IsModelValid(emailHost))
            {
                Context.EmailHost.Insert(emailHost);
                Context.Save();
                return RedirectToAction("Index");
            }
            
            return View(emailHost);
        }

        public ActionResult Edit(int? id)
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.EmailHost_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            EmailHost emailHost = Context.EmailHost.GetById(id);
            if (emailHost == null)
                return HttpNotFound();
            
            ViewBag.Message = BaseMessage.GetMessage();
            return View(emailHost);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmailHost emailHost)
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.EmailHost_Edit);
            if (IsModelValid(emailHost))
            {
                Context.EmailHost.Update(emailHost);
                Context.Save();
                return RedirectToAction("Index");
            }
            return View(emailHost);
        }

        private bool IsModelValid(EmailHost emailHost)
        {
            /*
            if (emailHost.Name != null)
            {
                return true;
            }
            else
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_EmailHost_NAME);
            return false;
            */
            return true;
        }
        
        public ActionResult Delete(int? id)
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.EmailHost_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            EmailHost emailHost = Context.EmailHost.GetById(id);
            if (emailHost == null)
                return HttpNotFound();

            return View(emailHost);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.EmailHost_Delete);
            EmailHost emailHost = Context.EmailHost.GetById(id);
            Context.EmailHost.Delete(emailHost);
            Context.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
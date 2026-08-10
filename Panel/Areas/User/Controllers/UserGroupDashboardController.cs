using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.User.Controllers
{
    public class UserGroupDashboardController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int usergroupId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.UserGroup_Edit);
            ViewBag.UserGroup = _context.UserGroup.GetById(usergroupId);
            return View(_context.UserGroupDashboard.GetAllByUserGroupId(usergroupId));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.UserGroup_Edit);
            UserGroupDashboard dashboard = _context.UserGroupDashboard.GetById(id);
            if (dashboard == null)
            {
                return HttpNotFound();
            }
            return View(dashboard);
        }

        public ActionResult Create(int usergroupId)
        {
            UserGroupDashboard dashboard = new UserGroupDashboard() {
                UserGroupId = usergroupId
            };
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.UserGroup_Edit);
            FillDropDowns(null);
            ViewBag.Message = BaseMessage.GetMessage();
            ViewBag.UserGroup = _context.UserGroup.GetById(usergroupId);
            return View(dashboard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserGroupDashboard dashboard)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.UserGroup_Edit);

            if (IsModelValid(dashboard))
            {
                _context.UserGroupDashboard.Insert(dashboard);
                _context.Save();
                return RedirectToAction("Index", new { usergroupId = dashboard.UserGroupId });
            }

            FillDropDowns(dashboard);
            ViewBag.UserGroup = _context.UserGroup.GetById(dashboard.UserGroupId);
            return View(dashboard);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.UserGroup_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            UserGroupDashboard dashboard = _context.UserGroupDashboard.GetById(id);
            if (dashboard == null)
                return HttpNotFound();

            FillDropDowns(dashboard);
            ViewBag.Message = BaseMessage.GetMessage();
            ViewBag.UserGroup = _context.UserGroup.GetById(dashboard.UserGroupId);
            return View(dashboard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserGroupDashboard dashboard)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.UserGroup_Edit);
            if (IsModelValid(dashboard))
            {
                _context.UserGroupDashboard.Update(dashboard);
                _context.Save();
                return RedirectToAction("Index", new { usergroupId = dashboard.UserGroupId });
            }
            FillDropDowns(dashboard);
            ViewBag.UserGroup = _context.UserGroup.GetById(dashboard.UserGroupId);
            return View(dashboard);
        }

        private bool IsModelValid(UserGroupDashboard dashboard)
        {
            bool result = false;
            if (dashboard.DashboardId == 0)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_USERGROUP_DASHBOARD);
            else
                result = true;
            return result;
        }

        private void FillDropDowns(UserGroupDashboard dashboard)
        {
            ViewBag.DashboardId = new SelectList(_context.Dashboard.GetAll(), "ID", "Name", dashboard != null ? dashboard.DashboardId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.UserGroup_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            UserGroupDashboard dashboard = _context.UserGroupDashboard.GetById(id);
            if (dashboard == null)
                return HttpNotFound();

            return View(dashboard);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.UserGroup_Edit);
            UserGroupDashboard dashboard = _context.UserGroupDashboard.GetById(id);
            try
            {
                int usergroupId = dashboard.UserGroupId;
                _context.UserGroupDashboard.Delete(dashboard);
                _context.Save();
                return RedirectToAction("Index", new { @usergroupId = usergroupId });
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(dashboard);
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
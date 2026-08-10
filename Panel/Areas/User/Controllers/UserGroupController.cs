using DataLayer.Entities;
using DataLayer.ViewModels;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.Enumarables;

namespace Panel.Areas.User.Controllers
{
    public class UserGroupController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.UserGroup);
            return View(_context.UserGroup.GetAll().ToView());
        }

        public ActionResult Permission(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.UserGroup_Permission);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Permission> permissions = _context.Permission.GetAllActiveForWebsite();
            List<UserGroupPermission> usergroupPermissions = _context.UserGroupPermission.GetAllByUserGroupId(id.Value);
            List<ViewUserGroupPermissions> model = new List<ViewUserGroupPermissions>();
            foreach (Permission item in permissions)
            {
                model.Add(new ViewUserGroupPermissions()
                {
                    PermissionId = item.ID,
                    PermissionName = item.Name,
                    ParentId = item.ParentId,
                    HasPermission = usergroupPermissions.Any(p => p.PermissionId == item.ID)
                });
            }
            ViewBag.UserGroupId = id;
            return View(model);
        }

        public ActionResult Users(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.UserGroup_Users);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            List<ViewSiteUserUserGroup> model = _context.SiteUserUserGroup.GetAllView(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult Permission(int UserGroupId, int[] PermissionList)
        {
            PermissionList = PermissionList == null ? new Int32[0] : PermissionList;
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.UserGroup_Permission);
            _context.UserGroupPermission.UpdateByUserGroupId(UserGroupId, PermissionList);
            return new JsonResult() { Data = new ViewMessage() };
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.UserGroup_Details);
            UserGroup
            usergroup = _context.UserGroup.GetById(id);
            if (usergroup == null)
            {
                return HttpNotFound();
            }
            return View(usergroup);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.UserGroup_New);
            FillDropDowns(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserGroup usergroup)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.UserGroup_New);

            if (IsModelValid(usergroup))
            {
                _context.UserGroup.Insert(usergroup);
                _context.Save();
                return RedirectToAction("Index");
            }
            FillDropDowns(null);
            return View(usergroup);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.UserGroup_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            UserGroup
            usergroup = _context.UserGroup.GetById(id);
            if (usergroup == null)
                return HttpNotFound();
            FillDropDowns(usergroup);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(usergroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserGroup usergroup)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.UserGroup_Edit);
            if (IsModelValid(usergroup))
            {
                _context.UserGroup.Update(usergroup);
                _context.Save();
                return RedirectToAction("Index");
            }
            FillDropDowns(usergroup);
            return View(usergroup);
        }

        private bool IsModelValid(UserGroup usergroup)
        {
            if (usergroup.Name != null)
            {
                return true;
            }
            else
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_USERGROUP_NAME);
            return false;
        }

        private void FillDropDowns(UserGroup group)
        {
            ViewBag.UserRoleId = new SelectList(_context.UserRole.GetAll(), "ID", "Name", group != null ? group.UserRoleId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.UserGroup_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            UserGroup
            usergroup = _context.UserGroup.GetById(id);
            if (usergroup == null)
                return HttpNotFound();

            return View(usergroup);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.UserGroup_Delete);
            UserGroup usergroup = _context.UserGroup.GetById(id);
            try
            {
                _context.UserGroup.Delete(usergroup);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(usergroup);
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
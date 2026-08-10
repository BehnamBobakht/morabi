using DataLayer.Entities;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.ViewModels;
using DataLayer.Enumarables;

namespace Panel.Areas.User.Controllers
{
    public class ShopSiteUserController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SiteUserShop);
            return View(_context.SiteUser.Where(u=>u.ID!=1).ToList().ToView());
        }
        public ActionResult Password(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SiteUserShop_NewPassword);
            if (id == null || id==1)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SiteUser user = _context.SiteUser.GetById(id);
            if (user == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(user);
        }

        [HttpPost]
        public ActionResult Password(int? id, string newPassword, string confirmPassword)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SiteUserShop_NewPassword);
            if (id == null || id == 1)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SiteUser user = _context.SiteUser.GetById(id);
            if (user == null)
                return HttpNotFound();

            bool result = false;
            if (newPassword == "")
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PASSWORD);
            else if (confirmPassword == "")
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PASSWORD_CONFIRM);
            else if (newPassword != confirmPassword)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_PASSWORD_CONFIRM);
            else
                result = true;

            if (result == true)
            {
                user.Password = BaseSecurity.HashMd5(newPassword);
                _context.SiteUser.Update(user);
                _context.Save();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public ActionResult SiteUserPassword()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SiteUserShop_ChangePassword);


            SiteUser user = _context.SiteUser.GetById(_context.SiteUser.GetCurrentUser().ID);
            if (user == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(user);
        }

        [HttpPost]
        public ActionResult SiteUserPassword(int? id,string oldPassword, string newPassword, string confirmPassword)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SiteUserShop_ChangePassword);
            if (id == null || id==1)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SiteUser user = _context.SiteUser.GetById(id);
            if (user == null)
                return HttpNotFound();

            bool result = false;
           if( user.Password != BaseSecurity.HashMd5(oldPassword))
            {
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_OLD_PASSWORD);
            }
            if (newPassword == "")
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PASSWORD);
            else if (confirmPassword == "")
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PASSWORD_CONFIRM);
            else if (newPassword != confirmPassword)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_PASSWORD_CONFIRM);
            else
                result = true;

            if (result == true)
            {
                user.Password = BaseSecurity.HashMd5(newPassword);
                _context.SiteUser.Update(user);
                _context.Save();
                return RedirectToAction("Index");
            }

            return View(user);
        }
     
        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SiteUserShop_Edit);
            if (id == null || id==1)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SiteUser user = _context.SiteUser.GetById(id);
            if (user == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserCreatorId,PictureId,FullName,Password,Email,Mobile,Address,CreateDatetime,UniqueIdentifier,Active")] SiteUser user, HttpPostedFileBase file)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SiteUserShop_Edit);
            if (user.ID == 1)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (IsModelValid(user, file))
            {
                _context.SiteUser.Update(user);
                _context.Save();

                return RedirectToAction("Index");
            }
            return View(user);
        }

        private bool IsModelValid(SiteUser user, HttpPostedFileBase file)
        {
            bool result = false;
            if (user.FullName == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_SITEUSER_FULLNAME);
            else if (user.Email == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_EMAIL);
            else if (user.Password == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PASSWORD);
            else if (_context.SiteUser.IsUniqueEmail(user))
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.UNIQUE_EMAIL);
            else if (_context.SiteUser.IsUniqueMobile(user))
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.UNIQUE_MOBILE);
            else
                result = true;

            if (file != null && result == true)
            {
                user.PictureId = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file).ID;
            }

            return result;
        }
        
        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SiteUser_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SiteUser user = _context.SiteUser.GetById(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SiteUser_Delete);
            SiteUser user = _context.SiteUser.GetById(id);
            try
            {
                _context.ShopUser.DeleteByUserId(user.ID);
                _context.SiteUserUserGroup.DeleteBySiteUserId(user.ID);
                _context.Save();

                _context.SiteUser.Delete(user);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(user);
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
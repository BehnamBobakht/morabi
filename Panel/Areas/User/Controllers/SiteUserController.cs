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
    public class SiteUserController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SiteUser);
            return View(_context.SiteUser.GetAll().ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SiteUser_Details);
            SiteUser user = _context.SiteUser.GetById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SiteUser_New);
            SiteUser user = new SiteUser() {
                Active = true,
                CreateDatetime = DateTime.Now,
                UserCreatorId = _context.SiteUser.GetCurrentUser().ID,
                UniqueIdentifier = Guid.NewGuid()
            };
            ViewBag.Message = BaseMessage.GetMessage();
            return View(user);
        }

        public ActionResult Password(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SiteUser_Password);
            if (id == null)
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
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SiteUser_Password);
            if (id == null)
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

        public ActionResult Groups(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SiteUser_Groups);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SiteUser user = _context.SiteUser.GetById(id);
            if (user == null)
                return HttpNotFound();

            List<UserGroup> listGroup = _context.UserGroup.GetAll();
            List<SiteUserUserGroup> listUsers = _context.SiteUserUserGroup.GetAllBySiteUserId(user.ID);
            List<ViewSiteUserUserGroup> model = new List<ViewSiteUserUserGroup>();
            foreach (UserGroup item in listGroup)
            {
                model.Add(new ViewSiteUserUserGroup()
                {
                    GroupId = item.ID,
                    IsSelected = listUsers.Any(p => p.UserGroupId == item.ID),
                    UserGroupName = item.Name
                });
            }

            ViewBag.SiteUserId = id;
            ViewBag.Message = BaseMessage.GetMessage();
            return View(model);
        }

        [HttpPost]
        public ActionResult Groups(int? id, int[] UserGroupList)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SiteUser_Groups);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SiteUser user = _context.SiteUser.GetById(id);
            if (user == null)
                return HttpNotFound();

            _context.SiteUserUserGroup.DeleteBySiteUserId(user.ID);

            if (UserGroupList != null)
            {
                foreach (int item in UserGroupList)
                {
                    SiteUserUserGroup entity = new SiteUserUserGroup() {
                        SiteUserId = user.ID,
                        UserGroupId = item
                    };
                    _context.SiteUserUserGroup.Insert(entity);
                }
            }
            _context.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserCreatorId,PictureId,FullName,Password,Email,Mobile,Address,CreateDatetime,UniqueIdentifier,Active")] SiteUser user, HttpPostedFileBase file, bool isShopUser = false)
        {
            SiteUser CurrentUser = _context.SiteUser.GetCurrentUser();
            _context.Permission.CheckPagePermission(CurrentUser, Enum_Permission.SiteUser_New);
            ViewBag.isShopUser = isShopUser;
            if (IsModelValid(user, file))
            {
                user.UserCreatorId = CurrentUser.ID;
                user.Password = BaseSecurity.HashMd5(user.Password);
                _context.SiteUser.Insert(user);
                _context.Save();

                if (isShopUser  == true)
                {
                    Website website = _context.Website.GetFirstByType(Enum_Code.SYSTEM_TYPE_SHOP);
                    Shop shop = new Shop()
                    {
                        Active = true,
                        Approved = false,
                        Name = "",
                        Label = "",
                        WebsiteId = website.ID,
                        UserCreatorId = CurrentUser.ID
                    };
                    _context.Shop.Insert(shop);
                    _context.Save();

                    UserGroup group = _context.UserGroup.GetFirstByRole(Enum_UserRole.SHOP);
                    SiteUserUserGroup usergroupuser = new SiteUserUserGroup()
                    {
                        SiteUserId = user.ID,
                        UserGroupId = group.ID,
                    };
                    _context.SiteUserUserGroup.Insert(usergroupuser);
                    ShopUser shopuser = new ShopUser()
                    {
                        UserId = user.ID,
                        ShopId = shop.ID
                    };
                    _context.ShopUser.Insert(shopuser);
                    _context.Save();
                    return RedirectToAction("profile", new { area = "store", controller = "shop", id = shop.ID });
                }
                else
                    return RedirectToAction("index");
            }

            return View(user);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SiteUser_Edit);
            if (id == null)
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
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SiteUser_Edit);
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
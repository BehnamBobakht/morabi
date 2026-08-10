using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.Api;
using DataLayer.Api.Model;

namespace Cms.Controllers
{
    public class SiteUserController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Register()
        {
            return PartialView(BaseController.GetView(this));
        }

        [HttpPost]
        public ActionResult Register(SiteUser user, string ConfirmPassword)
        {
            if (IsModelValid(user, ConfirmPassword))
            {
                user.Password = BaseSecurity.HashMd5(user.Password);
                user.UniqueIdentifier = Guid.NewGuid();
                user.CreateDatetime = DateTime.Now;
                _context.SiteUser.Insert(user);
                _context.Save();

                if (user.Active == false)
                {
                    string html = user.UniqueIdentifier.ToString();
                    BaseEmail.SendSimpleEmail(user.Email, "فعال سازی حساب کاربری", html);
                    return RedirectToAction("confirm");
                }
                else
                {
                    return RedirectToAction("register", "shop", new { @id = user.ID });
                }
            }
            return PartialView(BaseController.GetView(this), user);
        }

        public ActionResult Login()
        {
            SiteUser user = _context.SiteUser.GetCurrentUser();
            if (user != null)
                return Redirect("~/");
            return PartialView(BaseController.GetView(this));
        }

        [HttpPost]
        public ActionResult Login(SiteUser siteuser)
        {
            string password = BaseSecurity.HashMd5(siteuser.Password);
            SiteUser user = _context.SiteUser.GetFromEmailAndPassword(siteuser.Email, password);
            if (user != null)
            {
                if (user.Active == true)
                {
                    _context.SiteUser.SetCurrentUser(user, true);
                    return Redirect("~/");
                }
                else
                    ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.USER_ID_INACTIVE);
            }
            else
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_USERNAME_PASSWORD);
            return PartialView(BaseController.GetView(this), user);
        }

        [HttpGet]
        public ActionResult Logout(string back = "/siteuser/login")
        {
            _context.SiteUser.Logout();
            return Redirect(back);
        }

        public ActionResult ActivationCreateShop(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            if (id.IsGuid())
            {
                SiteUser user = _context.SiteUser.GetByUniqueCode(Guid.Parse(id));
                if (user != null)
                {
                    user.Active = true;
                    _context.SiteUser.Update(user);
                    _context.Save();
                    
                    UserGroup group = _context.UserGroup.GetFirstByRole(Enum_UserRole.SHOP);

                    SiteUserUserGroup usergroupuser = new SiteUserUserGroup()
                    {
                        SiteUserId = user.ID,
                        UserGroupId = group.ID,
                    };
                    _context.SiteUserUserGroup.Insert(usergroupuser);
                    _context.Save();

                    Shop shop = new Shop()
                    {
                        Name = "",
                        Active = true,
                        Approved = false,
                        WebsiteId = BaseWebsite.WebsiteId
                    };
                    _context.Shop.Insert(shop);
                    _context.Save();

                    ShopUser shopuser = new ShopUser()
                    {
                        UserId = user.ID,
                        ShopId = shop.ID
                    };
                    _context.ShopUser.Insert(shopuser);
                    _context.Save();

                    return RedirectToAction("edit", "shop");
                }
                else
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        public ActionResult Confirm()
        {
            return PartialView(BaseController.GetView(this));
        }

        public new ActionResult Profile()
        {
            SiteUser tempUser = _context.SiteUser.GetCurrentUser();
            if (tempUser == null)
                return RedirectToAction("login", new { controller = "siteuser", back = "profile" });
            tempUser = _context.SiteUser.GetById(tempUser.ID);
            return PartialView(BaseController.GetView(this), tempUser);
        }

        private bool IsModelValid(SiteUser user, string ConfirmPassword)
        {
            bool result = false;
            if (user.FullName == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_SITEUSER_FULLNAME);
            else if (user.Email == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_SITEUSER_EMAIL);
            else if (user.Mobile == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_SITEUSER_MOBILE);
            else if (BaseSecurity.IsValidInput(user.Email, Enum_Validation.EMAIL) == false)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.INVALID_SITEUSER_EMAIL);
            else if (user.Password == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_SITEUSER_PASSWORD);
            else if (_context.SiteUser.GetByEmail(user.Email, true) != null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.DUPLICATED_SITEUSER_EMAIL);
            else if (user.Password != ConfirmPassword)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.INVALID_PASSWORD_CONFIRM);
            else if (user.UserCreatorId != null)
            {
                SiteUser creator = _context.SiteUser.GetById(user.UserCreatorId);
                if (creator != null)
                {
                    user.Active = true;
                    result = true;
                }
                else
                    ViewBag.Message = BaseMessage.GetMessage(Enum_Message.INVALID_SITEUSER_CREATORID);
            }
            else
                result = true;

            return result;
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
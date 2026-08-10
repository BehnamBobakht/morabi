using DataLayer.Entities;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Enumarables;

namespace Panel.Areas.Profile.Controllers
{
    public class LoginController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ActionResult Index()
        {
            SiteUser user = _context.SiteUser.GetCurrentUser();
            if (user != null)
            {
                if (user.Active == true)
                    return Redirect("/");
                else
                    ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.USER_ID_INACTIVE);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password, bool remember = false)
        {
            ViewBag.Username = username;
            ViewBag.Remember = remember;
            password = BaseSecurity.HashMd5(password);
            SiteUser user = _context.SiteUser.GetFromEmailAndPassword(username.Trim(), password);
            if (user != null)
            {
                if (user.Active == true)
                {
                    _context.SiteUser.SetCurrentUser(user, remember);
                    return Redirect("~/");
                }
                else
                    ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.USER_ID_INACTIVE);
            }
            else
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_USERNAME_PASSWORD);
            return View("Index");
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
using DataLayer.Entities;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.Enumarables;

namespace Panel.Areas.Profile.Controllers
{
    public class RecoverController : Controller
    {
        UnitOfWork Context = new UnitOfWork();
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string email)
        {
            if (email == null || email.Trim() == "")
            {
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_EMAIL);
                return View();
            }
            
            SiteUser user = Context.SiteUser.GetByEmail(email, false);
            if (user == null)
            {
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_RECOVER_EMAIL);
                return View();
            }

            RecoverPassword recover = new RecoverPassword()
            {
                Datetime = DateTime.Now,
                IsUsed = false,
                UniqueId = Guid.NewGuid(),
                UserId = user.ID
            };

            Context.RecoverPassword.Insert(recover);
            Context.Save();
            return RedirectToAction("sent");
        }

        public ActionResult Done(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            RecoverPassword recover = Context.RecoverPassword.GetByUniqueId(id.Value);
            if (recover == null)
                return HttpNotFound();

            ViewBag.UniqueId = id;

            ViewBag.Message = BaseMessage.GetMessage();

            return View();
        }

        [HttpPost]
        public ActionResult Done(Guid uniqueid, string password, string confirm)
        {
            bool result = false;
            RecoverPassword recover = Context.RecoverPassword.GetByUniqueId(uniqueid);
            if (password == "")
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PASSWORD);
            else if (confirm == "")
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PASSWORD_CONFIRM);
            else if (password != confirm)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_PASSWORD_CONFIRM);
            else if (recover == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_RECOVER_UNIQUE_ID);
            else
                result = true;

            if (result == true)
            {
                recover.IsUsed = true;
                Context.RecoverPassword.Update(recover);

                SiteUser user = recover.SiteUser;
                user.Password = BaseSecurity.HashMd5(password);
                Context.SiteUser.Update(user);
                Context.SiteUser.SetCurrentUser(user);
                Context.Save();
            }

            ViewBag.UniqueId = uniqueid;

            Response.Redirect("/");

            return View();
        }

        public ActionResult Sent()
        {
            return View();
        }
    }
}
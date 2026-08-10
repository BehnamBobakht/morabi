using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Entities;
using DataLayer.ViewModels;

namespace Panel.Areas.Email.Controllers
{
    public class InboxController : Controller
    {
        UnitOfWork Context = new UnitOfWork();

        public ActionResult Index()
        {
            SiteUser user = Context.SiteUser.GetCurrentUser();
            user = Context.SiteUser.GetById(user.ID);
            ViewBag.EmailStatus = new ViewInbox(user);
            List<EmailInbox> list = Context.EmailInbox.GetAllBySiteUser(user.ID);
            return View(list);
        }

        public ActionResult Item(int? id)
        {
            SiteUser user = Context.SiteUser.GetCurrentUser();
            user = Context.SiteUser.GetById(user.ID);
            ViewBag.EmailStatus = new ViewInbox(user);
            
            EmailInbox email = Context.EmailInbox.GetById(id);

            email.IsRead = true;
            Context.Save();

            return View(email);
        }

        public ActionResult Compose()
        {
            return View();
        }
    }
}
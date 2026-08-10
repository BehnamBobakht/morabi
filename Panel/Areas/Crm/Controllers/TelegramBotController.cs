using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Crm.Controllers
{
    public class TelegramBotController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot);
            return View(_context.TelegramBot.GetAll().ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Details);
            TelegramBot bot = _context.TelegramBot.GetById(id);
            if (bot == null)
            {
                return HttpNotFound();
            }
            return View(bot);
        }

        public ActionResult Account(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Account);
            TelegramBot bot = _context.TelegramBot.GetById(id);
            if (bot == null)
            {
                return HttpNotFound();
            }
            return View(bot.TelegramAccount.ToList().ToView());
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_New);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TelegramBot bot)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_New);

            if (IsModelValid(bot))
            {
                bot.CreateDatetime = DateTime.Now;
                bot.UpdateDatetime = DateTime.Now;
                bot.UserId = _context.SiteUser.GetCurrentUser().ID;
                bot.Offset = 0;
                _context.TelegramBot.Insert(bot);
                _context.Save();
                return RedirectToAction("Index");
            }
            
            return View(bot);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TelegramBot bot = _context.TelegramBot.GetById(id);
            if (bot == null)
                return HttpNotFound();
            
            ViewBag.Message = BaseMessage.GetMessage();
            return View(bot);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TelegramBot bot)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Edit);
            if (IsModelValid(bot))
            {
                bot.UpdateDatetime = DateTime.Now;
                _context.TelegramBot.Update(bot);
                _context.Save();
                return RedirectToAction("Index");
            }
            return View(bot);
        }

        private bool IsModelValid(TelegramBot bot)
        {
            bool result = false;
            if (bot.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_TELEGRAMBOT_NAME);
            else if (bot.Token == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_TELEGRAMBOT_TOKEN);
            else
                result = true;
            return result;
        }
        
        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TelegramBot bot = _context.TelegramBot.GetById(id);
            if (bot == null)
                return HttpNotFound();

            return View(bot);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Delete);
            TelegramBot bot = _context.TelegramBot.GetById(id);
            try
            {
                _context.TelegramBot.Delete(bot);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(bot);
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
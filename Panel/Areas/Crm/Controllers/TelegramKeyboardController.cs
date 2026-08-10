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
    public class TelegramKeyBoardController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int botId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Keyboard);
            ViewBag.TelegramBot = _context.TelegramBot.GetById(botId);
            return View(_context.TelegramKeyBoard.GetAllByBotId(botId).ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Keyboard);
            TelegramKeyBoard keyboard = _context.TelegramKeyBoard.GetById(id);
            if (keyboard == null)
            {
                return HttpNotFound();
            }
            return View(keyboard);
        }

        public ActionResult Create(int botId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Keyboard);
            FillDropDowns(null);
            ViewBag.TelegramBot = _context.TelegramBot.GetById(botId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TelegramKeyBoard keyboard)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Keyboard);

            if (IsModelValid(keyboard))
            {
                _context.TelegramKeyBoard.Insert(keyboard);
                _context.Save();
                return RedirectToAction("Index", new { botId = keyboard.BotId });
            }
            else
                ViewBag.TelegramBot = _context.TelegramBot.GetById(keyboard.BotId);

            FillDropDowns(keyboard);
            return View(keyboard);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Keyboard);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TelegramKeyBoard keyboard = _context.TelegramKeyBoard.GetById(id);
            if (keyboard == null)
                return HttpNotFound();

            FillDropDowns(keyboard);
            ViewBag.Message = BaseMessage.GetMessage();
            ViewBag.TelegramBot = _context.TelegramBot.GetById(keyboard.BotId);
            return View(keyboard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TelegramKeyBoard keyboard)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Keyboard);
            if (IsModelValid(keyboard))
            {
                _context.TelegramKeyBoard.Update(keyboard);
                _context.Save();
                return RedirectToAction("Index", new { botId = keyboard.BotId });
            }
            else
                ViewBag.TelegramBot = _context.TelegramBot.GetById(keyboard.BotId);
            FillDropDowns(keyboard);
            return View(keyboard);
        }

        private bool IsModelValid(TelegramKeyBoard keyboard)
        {
            bool result = false;
            if (keyboard.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_TELEGRAM_KEYBOARD_NAME);
            else
                result = true;
            return result;
        }

        private void FillDropDowns(TelegramKeyBoard keyboard)
        {
            ViewBag.TypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.TELEGRAM_KEYBOARD_TYPE), "ID", "Name", keyboard != null ? keyboard.TypeId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Keyboard);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TelegramKeyBoard keyboard = _context.TelegramKeyBoard.GetById(id);
            if (keyboard == null)
                return HttpNotFound();

            return View(keyboard);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Keyboard);
            TelegramKeyBoard keyboard = _context.TelegramKeyBoard.GetById(id);
            int botId = keyboard.BotId;
            _context.TelegramKeyBoard.Delete(keyboard);
            _context.Save();
            return RedirectToAction("Index", new { botId = botId });
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
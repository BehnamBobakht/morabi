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
    public class TelegramKeyboardItemController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int keyboardId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Keyboard);
            ViewBag.TelegramKeyBoard = _context.TelegramKeyBoard.GetById(keyboardId);
            return View(_context.TelegramKeyBoardItem.GetAllByKeyBoardId(keyboardId).ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Keyboard);
            TelegramKeyBoardItem keyboardItem = _context.TelegramKeyBoardItem.GetById(id);
            if (keyboardItem == null)
            {
                return HttpNotFound();
            }
            return View(keyboardItem);
        }

        public ActionResult Create(int keyboardId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Keyboard);
            TelegramKeyBoard keyboard = _context.TelegramKeyBoard.GetById(keyboardId);
            ViewBag.TelegramKeyBoard = keyboard;
            FillDropDowns(null, keyboard.BotId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TelegramKeyBoardItem keyboardItem)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Keyboard);
            
            if (IsModelValid(keyboardItem))
            {
                _context.TelegramKeyBoardItem.Insert(keyboardItem);
                _context.Save();
                return RedirectToAction("Index", new { keyboardId = keyboardItem.KeyboardId });
            }
            else
            {
                TelegramKeyBoard keyboard = _context.TelegramKeyBoard.GetById(keyboardItem.KeyboardId);
                ViewBag.TelegramKeyBoard = keyboard;
                FillDropDowns(keyboardItem, keyboard.BotId);
            }
            
            return View(keyboardItem);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Keyboard);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TelegramKeyBoardItem keyboardItem = _context.TelegramKeyBoardItem.GetById(id);
            if (keyboardItem == null)
                return HttpNotFound();

            TelegramKeyBoard keyboard = _context.TelegramKeyBoard.GetById(keyboardItem.KeyboardId);
            ViewBag.TelegramKeyBoard = keyboard;
            FillDropDowns(keyboardItem, keyboard.BotId);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(keyboardItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TelegramKeyBoardItem keyboardItem)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Keyboard);
            if (IsModelValid(keyboardItem))
            {
                _context.TelegramKeyBoardItem.Update(keyboardItem);
                _context.Save();
                return RedirectToAction("Index", new { keyboardId = keyboardItem.KeyboardId });
            }
            else
            {
                TelegramKeyBoard keyboard = _context.TelegramKeyBoard.GetById(keyboardItem.KeyboardId);
                ViewBag.TelegramKeyBoard = keyboard;
                FillDropDowns(keyboardItem, keyboard.ID);
            }
            return View(keyboardItem);
        }

        private bool IsModelValid(TelegramKeyBoardItem keyboardItem)
        {
            bool result = false;
            if (keyboardItem.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_TELEGRAMKEYBOARDITEM_NAME);
            else
                result = true;
            return result;
        }

        private void FillDropDowns(TelegramKeyBoardItem keyboardItem, int BotId)
        {
            ViewBag.CommandId = new SelectList(_context.TelegramCommand.GetAllByBotId(BotId), "ID", "Name", keyboardItem != null ? keyboardItem.CommandId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Keyboard);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TelegramKeyBoardItem keyboardItem = _context.TelegramKeyBoardItem.GetById(id);
            if (keyboardItem == null)
                return HttpNotFound();

            return View(keyboardItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Keyboard);
            TelegramKeyBoardItem keyboardItem = _context.TelegramKeyBoardItem.GetById(id);
            int keyboardId = keyboardItem.KeyboardId;
            _context.TelegramKeyBoardItem.Delete(keyboardItem);
            _context.Save();
            return RedirectToAction("Index", new { keyboardId = keyboardId });
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
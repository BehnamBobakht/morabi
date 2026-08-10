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
    public class TelegramCommandController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int botId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Command);
            ViewBag.TelegramBot = _context.TelegramBot.GetById(botId);
            return View(_context.TelegramCommand.GetAllByBotId(botId).ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Command);
            TelegramCommand command = _context.TelegramCommand.GetById(id);
            if (command == null)
            {
                return HttpNotFound();
            }
            return View(command);
        }

        public ActionResult Create(int botId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Command);
            FillDropDowns(null, botId);
            ViewBag.TelegramBot = _context.TelegramBot.GetById(botId);
            ViewBag.Message = BaseMessage.GetMessage();
            TelegramCommand command = new TelegramCommand() {
                DisableNotification = true,
                DisableWebPagePreview = true
            };
            return View(command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TelegramCommand entity)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Command);

            if (IsModelValid(entity))
            {
                _context.TelegramCommand.Insert(entity);
                _context.Save();
                return RedirectToAction("Index", new { botId = entity.BotId });
            }
            else
                ViewBag.TelegramBot = _context.TelegramBot.GetById(entity.BotId);

            FillDropDowns(entity, entity.BotId);
            return View(entity);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Command);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TelegramCommand command = _context.TelegramCommand.GetById(id);
            if (command == null)
                return HttpNotFound();

            FillDropDowns(command, command.BotId);
            ViewBag.TelegramBot = _context.TelegramBot.GetById(command.BotId);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TelegramCommand entity)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Command);
            if (IsModelValid(entity))
            {
                _context.TelegramCommand.Update(entity);
                _context.Save();
                return RedirectToAction("Index", new { botId = entity.BotId });
            }
            else
            {
                ViewBag.TelegramBot = _context.TelegramBot.GetById(entity.BotId);
                FillDropDowns(entity, entity.BotId);
            }
            return View(entity);
        }

        private bool IsModelValid(TelegramCommand command)
        {
            bool result = false;
            if (command.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_TELEGRAMCOMMAND_NAME);
            else if (command.Command == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_TELEGRAMCOMMAND_COMMAND);
            else
                result = true;
            return result;
        }

        private void FillDropDowns(TelegramCommand command, int BotId)
        {
            ViewBag.TypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.TELEGRAM_COMMAND_TYPE), "ID", "Name", command != null ? command.TypeId : 0);
            ViewBag.KeyBoardId = new SelectList(_context.TelegramKeyBoard.GetAllByBotId(BotId), "ID", "Name", command != null ? command.KeyboardId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Command);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TelegramCommand command = _context.TelegramCommand.GetById(id);
            if (command == null)
                return HttpNotFound();

            return View(command);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.TelegramBot_Command);
            TelegramCommand command = _context.TelegramCommand.GetById(id);
            try
            {
                _context.TelegramCommand.Delete(command);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(command);
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
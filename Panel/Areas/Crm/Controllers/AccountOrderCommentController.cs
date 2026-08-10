using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Crm.Controllers
{
    public class AccountOrderCommentController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            ViewBag.Order = _context.AccountOrder.GetById(id);
            return View(_context.AccountOrderComment.GetAllByOrderId(id));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            AccountOrderComment comment = _context.AccountOrderComment.GetById(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        public ActionResult Create(int? id)
        {
            SiteUser user = _context.SiteUser.GetCurrentUser();
            _context.Permission.CheckPagePermission(user, Enum_Permission.AccountOrder_Details);
            ViewBag.Order = _context.AccountOrder.GetById(id);
            ViewBag.Message = BaseMessage.GetMessage();
            AccountOrderComment comment = new AccountOrderComment { OrderId = id, SiteUserId = user.ID };
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccountOrderComment comment, bool sendMessage=false)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);

            if (IsModelValid(comment))
            {
                comment.Datetime = DateTime.Now;
                _context.AccountOrderComment.Insert(comment);
                _context.Save();
                if (sendMessage)
                {
                    var order = _context.AccountOrder.FirstOrDefault(s => s.ID == comment.OrderId);
                    if (order != null)
                    {
                        StringBuilder str = new StringBuilder();
                        string token1 = "";
                        string token2 = "";
                        string token3 = "";
                        str.AppendLine(comment.Body);
                        _context.Sms.SaveNewSms(order.Account.Mobile, Enum_SmsType.ORDER_COMMENT, str.ToString(), token1, token2, token3);
                        _context.Email.SaveNewEmail(order.Account.Email, Enum_EmailType.ORDER_COMMENT, str.ToString(), "یادداشت سفارش");
                        _context.Save();
                    }

                }
                return RedirectToAction("index", new { id = comment.OrderId });
            }

            return View(comment);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            AccountOrderComment comment = _context.AccountOrderComment.GetById(id);
            if (comment == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccountOrderComment comment)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            if (IsModelValid(comment))
            {
                comment.Datetime = DateTime.Now;
                _context.AccountOrderComment.Update(comment);
                _context.Save();
                return RedirectToAction("index", new { id = comment.OrderId });
            }
            return View(comment);
        }

        private bool IsModelValid(AccountOrderComment comment)
        {
            bool result = false;
            if (comment.Body == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_ORDERCOMMENT_BODY);
            else
                result = true;
            return result;
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            AccountOrderComment comment = _context.AccountOrderComment.GetById(id);
            if (comment == null)
                return HttpNotFound();

            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            AccountOrderComment comment = _context.AccountOrderComment.GetById(id);
            try
            {
                int orderId = comment.OrderId.GetValueOrDefault();
                _context.AccountOrderComment.Delete(comment);
                _context.Save();
                return RedirectToAction("index", new { id = orderId });
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(comment);
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
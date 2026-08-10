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
    public class SmsSenderController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            return View(_context.Sms.GetAll());
        }
        public ActionResult Sms()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            Sms sms = new Sms();
            ViewBag.Message = BaseMessage.GetMessage();
            ViewBag.ProductId = new SelectList(_context.Product.GetAll().ToList(), "ID", "Name");
            return View(sms);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sms(int ProductId, string Body)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);

            var list = _context.AccountOrderProduct.Where(s => s.ProductId == ProductId && s.AccountOrder.StatusId == 2068).ToList();

            if (Body == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_SMS_BODY);
            else
            {
                foreach (var  item in list)
                {
                  
                    if (item.AccountOrder.Account.Mobile != null && item.AccountOrder.Account.Mobile != "")
                    {
                        _context.Sms.SaveNewSms(item.AccountOrder.Account.Mobile, Enum_SmsType.NEWSLETTER, Body);
                    }
                    _context.Save();
                }

                ViewBag.Body = Body;
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.SUCCESS, Enum_Message.SUCCESSFULL_NEWSLETTER_SENT);
            }
            return RedirectToAction("Index");
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
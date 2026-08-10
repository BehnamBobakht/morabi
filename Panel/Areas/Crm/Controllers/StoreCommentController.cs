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
    public class StoreCommentController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StoreComment);
            return View(_context.StoreComment.Search(pageSize: 1000).ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StoreComment_Details);
            StoreComment comment = _context.StoreComment.GetById(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        public ActionResult Answer(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StoreComment_Answer);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            StoreComment comment = _context.StoreComment.GetById(id);
            if (comment == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Answer(StoreComment comment)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StoreComment_Answer);
            if (IsModelValid(comment))
            {
                comment.UserId = _context.SiteUser.GetCurrentUser().ID;
                comment.AnswerDatetime = DateTime.Now;
                _context.StoreComment.Update(comment);
                _context.Save();

                if (comment.Approved == true)
                {
                    //Store entity = _context.Store.GetById(comment.StoreId);
                    Account account = _context.Account.GetById(comment.AccountId);

                    string token_1 = BaseWebsite.ShopUrl + "/st/" + comment.StoreId + "/1";
                    string token_2 = "";
                    string token_3 = "";

                    StringBuilder body = new StringBuilder();
                    body.AppendLine("پاسخ نظر شما در سایت " + BaseWebsite.ShopWebsite.Title + " داده شد");
                    body.AppendLine(token_1);

                    _context.Sms.SaveNewSms(account.Mobile, Enum_SmsType.COMMENT, body.ToString(), token_1, token_2, token_3);
                    _context.Email.SaveNewEmail(account.Email, Enum_EmailType.COMMENT, body.ToString(), "پاسخ نظر شما در سایت " + BaseWebsite.ShopWebsite.Title);
                    _context.Save();
                }

                return RedirectToAction("Index");
            }
            else
            {
                comment.Store = _context.Store.GetById(comment.StoreId);
                comment.Account = _context.Account.GetById(comment.AccountId);
            }
            return View(comment);
        }

        private bool IsModelValid(StoreComment comment)
        {
            bool result = false;
            if (comment.Body == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCTCOMMENT_BODY);
            else
                result = true;
            return result;
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StoreComment_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            StoreComment comment = _context.StoreComment.GetById(id);
            if (comment == null)
                return HttpNotFound();

            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StoreComment_Delete);
            StoreComment comment = _context.StoreComment.GetById(id);
            try
            {
                _context.StoreComment.Delete(comment);
                _context.Save();
                return RedirectToAction("Index");
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
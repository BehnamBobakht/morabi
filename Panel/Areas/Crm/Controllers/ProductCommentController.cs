using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DocumentFormat.OpenXml.Vml.Office;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Crm.Controllers
{
    public class ProductCommentController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductComment);
            return View(_context.ProductComment.Search(pageSize: 1000).ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductComment_Details);
            ProductComment comment = _context.ProductComment.GetById(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }
        
        public ActionResult Answer(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductComment_Answer);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductComment comment = _context.ProductComment.GetById(id);
            if (comment == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Answer(ProductComment comment)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductComment_Answer);
            if (IsModelValid(comment))
            {
                comment.UserId = _context.SiteUser.GetCurrentUser().ID;
                comment.AnswerDatetime = DateTime.Now;
                _context.ProductComment.Update(comment);
                _context.Save();
                    Account account = _context.Account.GetById(comment.AccountId);

                if (comment.Approved == true)
                {
                    Product entity = _context.Product.GetById(comment.ProductId);
                    
                    string token_1 = entity.GetLinkWithUrl();
                    string token_2 = "";
                    string token_3 = "";

                    StringBuilder body = new StringBuilder();
                    body.AppendLine("پاسخ نظر شما در سایت " + BaseWebsite.ShopWebsite.Title + " داده شد");
                    body.AppendLine(token_1);

                    _context.Sms.SaveNewSms(account.Mobile, Enum_SmsType.COMMENT, body.ToString(), token_1, token_2, token_3);
                    _context.Email.SaveNewEmail(account.Email, Enum_EmailType.COMMENT, body.ToString(), "پاسخ نظر شما در سایت " + BaseWebsite.ShopWebsite.Title);
                    _context.Save();
                }
                else if(comment.Approved == false)
                {
                    string token_1 = account.FullName;
                    string token_2 = "";
                    string token_3 = "";

                    StringBuilder body = new StringBuilder();
                    body.AppendLine("با عرض پوزش نظر شما در سایت قابل نمایش نمی باشد");
                    body.AppendLine(token_1);

                    _context.Sms.SaveNewSms(account.Mobile, Enum_SmsType.COMMENT_FAIL, body.ToString(), token_1, token_2, token_3);
                    _context.Save();
                }

                return RedirectToAction("Index");
            }
            else
            {
                comment.Product = _context.Product.GetById(comment.ProductId);
                comment.Account = _context.Account.GetById(comment.AccountId);
            }
            return View(comment);
        }

        private bool IsModelValid(ProductComment comment)
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
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductComment_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductComment comment = _context.ProductComment.GetById(id);
            if (comment == null)
                return HttpNotFound();

            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductComment_Delete);
            ProductComment comment = _context.ProductComment.GetById(id);
            try
            {
                _context.ProductComment.Delete(comment);
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
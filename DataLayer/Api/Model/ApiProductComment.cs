using DataLayer.Base;
using DataLayer.Enumarables;
using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiProductComment : ApiResponse
    {
        public static ApiResult Get(UnitOfWork _context, int? notId = null, int? accountId = null, int? productId = null, int? index = 1, int? pageSize = null, string body = null, bool? approved = true, bool notSee = false)
        {
            List<ProductComment> list = _context.ProductComment.Search(
                notId: notId,
                accountId: accountId,
                approved: approved,
                body: body,
                index: index,
                notSee: notSee,
                pageSize: pageSize,
                productId: productId);
            return CreateSuccessResult(list);
        }

        public static ApiResult Post(UnitOfWork _context, ViewProductComment comment, int accountId)
        {
            comment.Body = comment.Body == null ? "" : comment.Body.Trim();
            if (string.IsNullOrEmpty(comment.Body) == true)
                return CreateErrorResult(Enum_Message.REQUIRED_PRODUCTCOMMENT_BODY);

            ProductComment entity = new ProductComment()
            {
                AccountId = accountId,
                Body = comment.Body,
                Datetime = DateTime.Now,
                NameFamily = comment.NameFamily,
                EmailAddress = comment.EmailAddress,
                ProductId = comment.Product.Id.GetInteger(),
                Rate = comment.Rate
            };

            _context.ProductComment.Insert(entity);
            _context.Save();
            if (accountId != 0)
            {
                var account = _context.Account.GetById(accountId);
                StringBuilder str = new StringBuilder();
                string token1 = account.FullName;
                string token2 = "";
                string token3 = "";
                str.AppendLine(token1);
                str.AppendLine(".عزیز نظر شما با موفقیت ثبت شد و پس از تایید نمایش داده میشود");

                _context.Sms.SaveNewSms(account.Mobile, Enum_SmsType.ACCOUNT_COMMENT, str.ToString(), token1, token2, token3);
                _context.Email.SaveNewEmail(account.Email, Enum_EmailType.ACCOUNT_COMMENT, str.ToString(), "ثبت نظر");
                _context.Save();

            }


            return CreateSuccessResult(Enum_Message.SUCCESSFULL_COMMENT);
        }
    }
}

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
    public class ApiStoreComment : ApiResponse
    {
        public static ApiResult Get(UnitOfWork _context, int? notId = null, int? accountId = null, int? storeId = null, int? index = 1, int? pageSize = null, string body = null, bool? approved = true, bool notSee = false)
        {
            List<StoreComment> list = _context.StoreComment.Search(
                notId: notId,
                accountId: accountId,
                approved: approved,
                body: body,
                index: index,
                notSee: notSee,
                pageSize: pageSize,
                storeId: storeId);
            return CreateSuccessResult(list);
        }

        public static ApiResult Post(UnitOfWork _context, ViewStoreComment comment, int accountId)
        {
            comment.Body = comment.Body == null ? "" : comment.Body.Trim();
            if (string.IsNullOrEmpty(comment.Body) == true)
                return CreateErrorResult(Enum_Message.REQUIRED_PRODUCTCOMMENT_BODY);

            StoreComment entity = new StoreComment()
            {
                AccountId = accountId,
                Body = comment.Body,
                Datetime = DateTime.Now,
                NameFamily = comment.NameFamily,
                EmailAddress = comment.EmailAddress,
                StoreId = comment.Store.ID/*.GetInteger()*/,
                Rate=comment.Rate
            };

            _context.StoreComment.Insert(entity);
            _context.Save();
            return CreateSuccessResult(Enum_Message.SUCCESSFULL_COMMENT);
        }
    }
}

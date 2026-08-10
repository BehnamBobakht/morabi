using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class StoreCommentController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpPost]
        public ApiResult Post(ViewStoreComment comment)
        {
            ViewAccount CurrentUser = _context.Account.GetCurrentAccount();
            if (CurrentUser == null)
                return ApiResponse.CreateInvalidKeyResult();
            return ApiStoreComment.Post(_context, comment, CurrentUser.Id);
        }
        [HttpGet]
        public ApiResult Get(int id)
        {
            var CurrentUserComment = _context.StoreComment.GetById(id);
            ViewUserStoreComment comment = new ViewUserStoreComment();
            comment.ID = CurrentUserComment.ID;
            comment.Rate = CurrentUserComment.Rate;
            comment.StoreId = CurrentUserComment.StoreId; 
            comment.Body = CurrentUserComment.Body;
            comment.AnswerString = CurrentUserComment.AnswerString;
            comment.Approved = CurrentUserComment.Approved;
            comment.Datetime = CurrentUserComment.Datetime;
            comment.AnswerDatetime= CurrentUserComment.AnswerDatetime;
            comment.NameStore = CurrentUserComment.Store.Name;
            return ApiResponse.CreateSuccessResult(comment);
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

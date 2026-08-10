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
    public class ProductCommentController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpPost]
        public ApiResult Post(ViewProductComment comment)
        {
            ViewAccount CurrentUser = _context.Account.GetCurrentAccount();
            if (CurrentUser == null)
                return ApiResponse.CreateInvalidKeyResult();
            return ApiProductComment.Post(_context, comment, CurrentUser.Id);
        }
        [HttpGet]
        public ApiResult Get(int id)
        {
            var CurrentUserComment = _context.ProductComment.GetById(id);
            ViewUserComment comment = new ViewUserComment();
            comment.ID = CurrentUserComment.ID;
            comment.Rate = CurrentUserComment.Rate;
            comment.ProductId = CurrentUserComment.ProductId; 
            comment.Body = CurrentUserComment.Body;
            comment.AnswerString = CurrentUserComment.AnswerString;
            comment.Approved = CurrentUserComment.Approved;
            comment.Datetime = CurrentUserComment.Datetime;
            comment.AnswerDatetime= CurrentUserComment.AnswerDatetime;
            comment.NameProduct = CurrentUserComment.Product.Name;
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

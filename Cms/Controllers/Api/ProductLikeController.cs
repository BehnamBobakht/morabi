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
    public class ProductLikeController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ApiResult Get()
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return ApiProductLike.CreateInvalidKeyResult();
            else
                return ApiProductLike.Get(_context, account.Id);
        }

        [HttpPost]
        public ApiResult Post(int productId)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return ApiResponse.CreateInvalidKeyResult();
            else
                return ApiProductLike.Post(_context, account.Id, productId);
        }

        [HttpDelete]
        public ApiResult Delete(int productId)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return ApiResponse.CreateInvalidKeyResult();
            else
                return ApiProductLike.Delete(_context, account.Id, productId);
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

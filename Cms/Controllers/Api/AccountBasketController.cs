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
    public class AccountBasketController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ApiResult Get()
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            int? accountId = account != null ? account.Id : default(int?);
            return ApiBasket.Get(_context, accountId);
        }

        [HttpPost]
        public ApiResult Post(int productId, int? colorId = null, int? sizeId = null, int? optionItemId=null, int? resellerId = null, int? storeId = null,int count=1)
        {
            _context.AccountBasket.RemoveOutOfSendStore();
            ViewAccount account = _context.Account.GetCurrentAccount();
            int? accountId = account != null ? account.Id : default(int?);

            return ApiBasket.Post(_context: _context, accountId: accountId, productId: productId, colorId: colorId, sizeId: sizeId,optionItemId:optionItemId, resellerId: resellerId, storeId: storeId,count:count);
        }
        
        [HttpDelete]
        public ApiResult Delete(int? basketId, int productId, int? colorId = null, int? sizeId = null, int? OptionItemId = null, int? storeId = null)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account != null)
                return ApiBasket.Delete(_context, account.Id, basketId.GetValueOrDefault());
            else
                return ApiBasket.Delete(_context, productId, colorId, sizeId,OptionItemId,storeId);
        }

        [HttpDelete]
        public ApiResult ClearAll()
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account != null)
                return ApiBasket.Delete(_context, account.Id);
            else
                return ApiBasket.Delete(_context);
        }

        [HttpPut]
        public ApiResult Put(string actionType = "add", int? basketId = null, int? productId = null, int? colorId = null, int? sizeId = null, int? OptionItemId = null, int? storeId=null)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            int? accountId = account != null ? account.Id : default(int?);
            if (accountId != null)
                return ApiBasket.Put(_context: _context, accountId: accountId, basketId: basketId.GetValueOrDefault(), actionType: actionType);
            else
                return ApiBasket.Put(_context: _context, actionType: actionType, productId: productId.GetValueOrDefault(), colorId: colorId, sizeId: sizeId, optionItemId: OptionItemId, storeId: storeId);
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

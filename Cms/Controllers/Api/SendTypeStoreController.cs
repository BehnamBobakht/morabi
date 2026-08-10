using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Helpers.Google;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class SendTypeStoreController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();
        [HttpGet]
        public ApiResult Get()
        {
            List<ViewSendTypeStores> list = new List<ViewSendTypeStores>();
            list = _context.Account.GetSendTypeList();
            return ApiResponse.CreateSuccessResult(list);
        }
        [HttpGet]
        public ApiResult Post(int storeId,int id)
        {
      
            try
            {
                List<ViewSendTypeStores> list = new List<ViewSendTypeStores>();
                list.Add(new ViewSendTypeStores { Id = id, StoreId = storeId });
                _context.Account.SetSendTypeStore(list);
                return ApiResponse.CreateSuccessResult();
               
            }
            catch
            {
                return ApiResponse.CreateErrorResult(DataLayer.Enumarables.Enum_Message.ERROR_REBATE_USED_BEFORE);
            }

        }

        [HttpGet]
        public ApiResult Remove(int id)
        {
            var currentAccount = BaseWebsite.CurrentAccount;
            var shipping = _context.ShippingCity.Where(s => s.StoreId == currentAccount.StoreId && s.ShippingCityId == id).FirstOrDefault();
            if (shipping != null)
            {
                _context.ShippingCity.Delete(shipping);
                _context.Save();
                return ApiResponse.CreateSuccessResult();
            }
            else
            {
                return ApiResponse.CreateErrorResult(DataLayer.Enumarables.Enum_Message.NONE);
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

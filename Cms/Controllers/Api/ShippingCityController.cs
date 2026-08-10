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
    public class ShippingCityController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();
        [HttpGet]
        public ApiResult Get()
        {
            var currentAccount = BaseWebsite.CurrentAccount;
            List<ViewShippingCity> list = new List<ViewShippingCity>();
            list = _context.ShippingCity.Where(s => s.StoreId == currentAccount.StoreId).ToApi();
            return ApiResponse.CreateSuccessResult(list);
        }
        [HttpGet]
        public ApiResult Post(int stateId, int cityId , int countryId , int sendTime , decimal sendPrice=0,int ? SendTimePostP=null,int ? SendPricePost=null,int ? SendPricePostP=null,int ? SendTimePost=null)
        {
            ApiResult result = new ApiResult();
            var currentAccount = BaseWebsite.CurrentAccount; 
            try
            {
                if (stateId == (int)DataLayer.Enumarables.ShippingCityType.All)
                {
                    var listShipping = _context.ShippingCity.Where(s => s.StoreId == currentAccount.StoreId).ToList();
                    foreach (var item in listShipping)
                    {
                        _context.ShippingCity.Delete(item);
                    }
                    _context.Save();
                }
                if (cityId == (int)DataLayer.Enumarables.ShippingCityType.All)
                {
                    var listShipping = _context.ShippingCity.Where(s => s.StoreId == currentAccount.StoreId && s.StateId == stateId).ToList();
                    foreach (var item in listShipping)
                    {
                        _context.ShippingCity.Delete(item);
                    }
                    _context.Save();
                }
                
                if(_context.ShippingCity.Where(s=>s.CityId==cityId && s.StateId==stateId && s.StoreId == currentAccount.StoreId).FirstOrDefault() == null)
                {
                    ShippingCity model = new ShippingCity();
                    model.StoreId = currentAccount.StoreId;
                    model.StateId = stateId==-1?0: stateId ;
                    model.CityId = cityId==-1?0: cityId;
                    model.CountryId = countryId;
                    model.SendPrice = sendPrice;
                    model.SendTime = sendTime;
                    model.SendPricePost = SendPricePost;
                    model.SendPricePostP = SendPricePostP;
                    model.SendTimePost = SendTimePost;
                    model.SendTimePostP = SendTimePostP;
                    _context.ShippingCity.Insert(model);
                    _context.Save();
                    return ApiResponse.CreateSuccessResult();
                }
                else
                {
                    return ApiResponse.CreateErrorResult(DataLayer.Enumarables.Enum_Message.NONE);
                }
               
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

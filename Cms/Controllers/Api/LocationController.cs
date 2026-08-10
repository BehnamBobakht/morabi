using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Entities;
using DataLayer.ViewModels;
using DataLayer.Helpers.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class LocationController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ApiResult Post(int countryId, int cityId, int stateId,int addressId)
        {
            _context.AccountBasket.RemoveOutOfSendStore();
            _context.Account.SetCurrentLocation(countryId, cityId, stateId, addressId);
            return ApiResponse.CreateSuccessResult();
        }
        [HttpGet]
        public ApiResult Get()
        {
            var location = _context.Account.GetCurrentLocation();
            return ApiResponse.CreateSuccessResult(location);
        }
        [HttpDelete]
        public ApiResult Delete()
        {
            _context.AccountBasket.RemoveOutOfSendStore();
            return ApiResponse.CreateSuccessResult();
        }
        [HttpGet]
        public ApiResult Get(bool OutOfSendStore)
        {
            var location = _context.AccountBasket.GetAllSetOutOfSendStoreFromCookie();
            return ApiResponse.CreateSuccessResult(location);
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

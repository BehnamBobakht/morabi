using DataLayer.Api;
using DataLayer.Base;
using DataLayer.Api.Model;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class ShopResellerController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ApiResult Get()
        {
            SiteUser user = BaseWebsite.CurrentSiteUser;
            if (user == null)
                return ApiResponse.CreateInvalidKeyResult();
            ShopReseller temp = _context.ShopReseller.GetByUserId(user.ID);
            if (temp == null)
                return ApiResponse.CreateInvalidKeyResult();
            return ApiShopReseller.Get(_context, temp);
        }

        [HttpPut]
        public ApiResult Put(ShopReseller reseller)
        {
            SiteUser user = BaseWebsite.CurrentSiteUser;
            if (user == null)
                return ApiResponse.CreateInvalidKeyResult();
            ShopReseller temp = _context.ShopReseller.GetByUserId(user.ID);
            if (temp == null)
                return ApiResponse.CreateInvalidKeyResult();
            reseller.ID = temp.ID;
            return ApiShopReseller.Put(_context, reseller);
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
